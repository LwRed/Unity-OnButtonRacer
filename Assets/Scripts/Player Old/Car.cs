using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public Transform centerOfMass;
    
    //public Light leftLight;
    //public Light rightLight;
    
    public float motorTorque = 1000f;
    public bool boostEngaged = false;
    public float boostForce = 1000f;
    public float maxSteerAngle = 40f;

    public float Steer {get; set; }
    public float Throttle {get; set; }

    private Rigidbody _rigidbody;
    private Wheel[] wheels;

    public AudioSource crashAudioSource;
    private float lastCrashVolume;


    // Start is called before the first frame update
    void Start()
    {
        this.wheels = GetComponentsInChildren<Wheel>();
        this._rigidbody = GetComponent<Rigidbody>();
        this._rigidbody.centerOfMass = centerOfMass.localPosition;

        Debug.Log("Brrm!  Car script started.");     
    }

 
    void Update(){
    
        Steer = GameManager.Instance.InputController.SteerInput;
        Throttle = GameManager.Instance.InputController.ThrottleInput;

        
        foreach (var wheel in wheels)
        {
            wheel.SteerAngle  = Steer * maxSteerAngle;
            wheel.Torque = Throttle * motorTorque;
        }

        if (Input.GetButtonDown("Fire1")){
            DropReset();
        } 

    }

    void DropReset(){

        Rigidbody rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 180, 0);
        //Placement sur la ligne de depart
        transform.position = new Vector3(-53,0,10);
        //Drop
        transform.Translate(0, 5, 0);
        //Annulation de la rotation
        rb.angularVelocity = new Vector3(0,0,0);
        //Annulation de la vitesse
        rb.linearVelocity = new Vector3(0,0,0);
        
    }

    private void OnCollisionEnter(Collision col)
    { 
        float num = Mathf.Clamp01(col.relativeVelocity.magnitude / 20f);
        if (!this.crashAudioSource.isPlaying || num > this.lastCrashVolume)
        { 
            this.crashAudioSource.volume = num;
            this.lastCrashVolume = num;
            this.crashAudioSource.Play();
        }

        if (col.collider.tag == "Off" || col.collider.tag == "Water")
		{
            DropReset();
        }
    }

}

