using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IDEAS: max steer angle is fun for reverse handbrake turns - stunt driving / stunt parking...
//From unity docs:
//You might want to decrease physics timestep length in the Time window to get more stable car physics, especially if it’s a racing car that can achieve high velocities.
//To keep a car from flipping over too easily you can lower its Rigidbody
//center of mass a bit from script, and apply “down pressure” force that depends on car velocity.

public class CarScript2 : MonoBehaviour
{
    public Transform centerOfMass;

    public WheelCollider wheelColliderLeftFront;
    public WheelCollider wheelColliderLeftBack;
    public WheelCollider wheelColliderRightFront;
    public WheelCollider wheelColliderRightBack;

    public Transform wheelLeftFront;
    public Transform wheelLeftBack;
    public Transform wheelRightFront;
    public Transform wheelRightBack;
    
    public Light leftLight;
    public Light rightLight;
    
    public float motorTorque = 2000f;
    public bool boostEngaged = false;
    public float boostForce = 1000f;
    public float maxSteerAngle = 90f;

    private Rigidbody _rigidbody;




    // Start is called before the first frame update
    void Start()
    {
        // var lights = GetComponents<Light>();
        // leftLight = lights[0];
        // rightLight = lights[1];
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        Debug.Log("Truck prêt : ZS/QD");        
    }

    void SetLights(bool state){
            leftLight.enabled = state;
            rightLight.enabled = state;        
    }

    void FixedUpdate() {
        Rigidbody rb = GetComponent<Rigidbody>();
        var torque = motorTorque * (boostEngaged ? 10 : 1);
        wheelColliderLeftFront.motorTorque = Input.GetAxis("Verticalp2") * torque;
        wheelColliderRightFront.motorTorque = Input.GetAxis("Verticalp2") * torque;
        wheelColliderLeftFront.steerAngle = Input.GetAxis("Horizontalp2") * maxSteerAngle;
        wheelColliderRightFront.steerAngle = Input.GetAxis("Horizontalp2") * maxSteerAngle;
        if (boostEngaged){
            rb.AddForce(transform.forward * boostForce);
        }
    }

    void DropReset(){

         Rigidbody rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 180, 0);
        //Placement sur la ligne de depart
        transform.position = new Vector3(-47,0,10);
        //Drop
        transform.Translate(0, 5, 0);
        //Annulation de la rotation
        rb.angularVelocity = new Vector3(0,0,0);
        //Annulation de la vitesse
        rb.linearVelocity = new Vector3(0,0,0);
        
    }
    void policeLights(){

        float duration = 0.2f;
        Color color0 = Color.red;
        Color color1 = Color.blue;
        // set light color
        float t = Mathf.PingPong(Time.time, duration) / duration;
        leftLight.color = Color.Lerp(color0, color1, t);
        rightLight.color = Color.Lerp(color1, color0, t);
    }
    void Update(){
        
        boostEngaged = (Input.GetButton("Fire3") || Input.GetButton("Fire4"));

        if (Input.GetButtonDown("Fire2")){
            DropReset();
            SetLights(false);
        }        
        if (Input.GetButtonDown("Fire3")){
            SetLights(true);
        }        
        policeLights();
        
        //Update the visuals of the wheels
        //Align each wheel (visual element) to where the wheel collider is (in position and rotation)
        placeWheelAtCollider(wheelLeftFront, wheelColliderLeftFront, false);
        placeWheelAtCollider(wheelLeftBack, wheelColliderLeftBack, false);
        placeWheelAtCollider(wheelRightFront, wheelColliderRightFront, true);
        placeWheelAtCollider(wheelRightBack,  wheelColliderRightBack,  true);
        
    }

    void placeWheelAtCollider(Transform wheel, WheelCollider wheelCollider, bool shouldFlipOnZ){
        
        var pos = Vector3.zero;
        var rot = Quaternion.identity;
        wheelCollider.GetWorldPose(out pos, out rot);

        wheel.position = pos;
        if (shouldFlipOnZ){
            wheel.rotation = rot * Quaternion.Euler(0, 180, 0);
        }
        else {
            wheel.rotation = rot;
        }        
    }

    void OnCollisionEnter (Collision col)
	{
		if (col.collider.tag == "Off" || col.collider.tag == "Water")
		{
            DropReset();
        }
    }
}
