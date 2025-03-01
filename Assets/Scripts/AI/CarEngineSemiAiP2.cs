using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarEngineSemiAiP2 : MonoBehaviour {

	public KeyCode goKeyP2 = KeyCode.Z;
	public KeyCode stopKeyP2 = KeyCode.S;
	public Transform path;
	public float maxDistanceToNodes = 4f;
	public float maxSteerAngle = 40f;
	public float turnSpeed = 5f;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;
	public float maxMotorTorque = 50f;
	public float maxBrakeTorque = 150f;
	public float currentSpeed;
	public float maximumSpeed = 100f;
	public Vector3 centerOfMass;

	[Header("Sensors")]
	public float sensorLength = 3f;
	public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
	public float sideSensorPosition = 0.3f;
	public float frontSensorAngle = 30f;


    private Rigidbody rb;
	private List<Transform> nodes;
	private int currentNode = 0;
	private float currentTorque = 0;
	private bool avoiding = false;
	private float targetSteerAngle = 0;
    
    private float reset = 5;

	private void Start () {
        rb = GetComponent<Rigidbody>();
		rb.centerOfMass = centerOfMass;
		// Get child transforms
		Transform[] pathTransforms = path.GetComponentsInChildren<Transform> ();
		nodes = new List<Transform> ();

		// create list skipping the parent transform
		for (int i = 0; i < pathTransforms.Length; i++) {
			if (pathTransforms [i] != path.transform) {
				nodes.Add (pathTransforms [i]);
			}
		}
	}

	private void OnCollisionEnter(Collision _col) { 
	if (_col.collider.tag == "Off" || _col.collider.tag == "Water")
		{
            autoResetCar();
        }
	}
	

	private void FixedUpdate() {
		Sensors ();
		ApplySteer ();
		Drive();
		CheckWaypointDistance ();
		lerpToSteerAngle ();
        checkStopMoving();
		getKeys();
	}

	private void getKeys()
	{
		// KeyBinding avec recuperation du KeyCode sauvegardé
		if (PlayerPrefs.HasKey("goKeyP2")) {goKeyP2 = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("goKeyP2"));}
		if (PlayerPrefs.HasKey("stopKeyP2")) {stopKeyP2 = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("stopKeyP2"));}
	}

	private void Sensors ()
	{
		RaycastHit hit;
		Vector3 sensorStartPos = transform.position;
		sensorStartPos += transform.forward * frontSensorPosition.z;
		sensorStartPos += transform.up * frontSensorPosition.y;
		float avoidMultiplier = 0;
		avoiding = false;

		// front right sensor
		sensorStartPos += transform.right * sideSensorPosition;
		if (Physics.Raycast (sensorStartPos, transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag ("Terrain")) {
				Debug.DrawLine (sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier -= 1f;
			}
		}

		// front right angle sensor
		else if (Physics.Raycast (sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag ("Terrain")) {
				Debug.DrawLine (sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier -= 0.3f; //0.5f
			}
		}

		// front left sensor
		sensorStartPos -= transform.right * 2f * sideSensorPosition;
		if (Physics.Raycast (sensorStartPos, transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag ("Terrain")) {
				Debug.DrawLine (sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier += 1f;
			}
		}
		// front right angle sensor
		else if (Physics.Raycast (sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag ("Terrain")) {
				Debug.DrawLine (sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier += 0.3f; //0.5f
			}
		}

		if (avoidMultiplier == 0) {
			// front center sensor
			sensorStartPos += transform.right * sideSensorPosition;
			if (Physics.Raycast (sensorStartPos, transform.forward, out hit, sensorLength)) {
				if (!hit.collider.CompareTag ("Terrain")) {
					Debug.DrawLine (sensorStartPos, hit.point);
					avoiding = true;
					if (hit.normal.x < 0) {
						avoidMultiplier = -1;
					} else {
						avoidMultiplier = 1;
					}
				}
			}
		}

		if (avoiding) {
			targetSteerAngle = maxSteerAngle * avoidMultiplier;
		}

	}

	private void ApplySteer() {
		if (avoiding) return;
		Vector3 relativeVector = transform.InverseTransformPoint (nodes [currentNode].position);
		float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
		targetSteerAngle = newSteer;
	}

	private void Drive(){
		currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
		if (currentSpeed < maximumSpeed) {
			//KeyCodes
			if (Input.GetKey(goKeyP2)) 
			{
				//On met les gas
				currentTorque = maxMotorTorque;
				//On coupe les freins
				wheelRL.brakeTorque = 0f;
				wheelRR.brakeTorque = 0f;

			} else if (Input.GetKey(stopKeyP2)) {
				//On inverse les gas
				//currentTorque = -maxMotorTorque;
				//On coupe les freins
				//wheelRL.brakeTorque = 0f;
				//wheelRR.brakeTorque = 0f;
				autoResetCar();

			} else {
				//On coupe les gas
				currentTorque = 0;
				//On met les freins
				wheelRL.brakeTorque = maxBrakeTorque;
				wheelRR.brakeTorque = maxBrakeTorque;
			}
			
		} else {
			currentTorque = 0f;
		}
		wheelFL.motorTorque = currentTorque;
		wheelFR.motorTorque = currentTorque;
	}
    
    private void checkStopMoving(){
		//Quelques checks pour voir la situation de la voiture
        if (currentSpeed < 3 || currentTorque == 0 || transform.position.y < -0.98 || transform.rotation.x > 50 || transform.rotation.x < -50){
            reset -= Time.deltaTime;
            if (reset < 0){
                autoResetCar();
                //Debug.Log(reset);
            }
        }else{
            reset = 3;
        }
    }

	private void CheckWaypointDistance(){
		float distanceToNode = Vector3.Distance (transform.position, nodes [currentNode].position);
		if ( distanceToNode < maxDistanceToNodes) {
			if (currentNode == nodes.Count - 1) {
				currentNode = 0;
			} else {
				currentNode++;
			}
		}
	}

	private void lerpToSteerAngle(){
		wheelFL.steerAngle = Mathf.Lerp (wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
		wheelFR.steerAngle = Mathf.Lerp (wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
	}
    
    private void autoResetCar(){
        Rigidbody rb = GetComponent<Rigidbody>();
       //if (currentNode > 0 && currentNode <= nodes.Count){
		//rb.transform.position = nodes[currentNode-1].position;
		//rb.transform.rotation = nodes[currentNode-1].rotation;
		//} else if (currentNode == 0) {
		//rb.transform.position = nodes[nodes.Count-1].position;
		//rb.transform.rotation = nodes[nodes.Count-1].rotation;
		//}
		if (currentNode > 0){
		rb.transform.position = nodes[currentNode-1].position;
		rb.transform.rotation = nodes[currentNode-1].rotation;
		} else {
		//rb.transform.position = nodes[0].position;
		//rb.transform.rotation = nodes[0].rotation;
		}
		//Drop
        rb.transform.Translate(0,3,0);
		//rb.transform.Translate = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        //Annulation de la rotation
        rb.angularVelocity = new Vector3(0,0,0);
        //Annulation de la vitesse
        rb.linearVelocity = new Vector3(0,0,0);
		
		
        
    }
        
        
}
