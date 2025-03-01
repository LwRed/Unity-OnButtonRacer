using System;
using UnityEngine;


public class CarController : MonoBehaviour
{ 
public Transform centerOfMass;

public float motorTorque = 1500f;
public bool boostEngaged = false;
public float boostForce = 1000f;
public float maxSteer = 20f;

[Header("ENGINE")]
public float maxEngineRPM = 4000f;

public float minEngineRPM = 1000f;

public AnimationCurve torqueCurve;

private float _engineRPM;

[Header("GEARS")]
public float[] GearRatio = new float[]
{ 
0f,
4f,
3f,
2f,
1.5f,
1f
};

public float finalDriveRatio = 3.5f;

private int _currentGear = 1;

public AudioSource engineAudioSource;

public AudioSource engineAudioSourceDistorted;

public AudioSource crashAudioSource;

public WheelCollider[] engineSoundWheels;

private Wheel[] wheels;

private Rigidbody _rigidbody;

private float engineSoundRPM;

private float lastCrashVolume;

public float Steer
{ 
get;
set;
}

public float Throttle
{ 
get;
set;
}

private void Start()
{ 
this.wheels = base.GetComponentsInChildren<Wheel>();
this._rigidbody = base.GetComponent<Rigidbody>();
this._rigidbody.centerOfMass = this.centerOfMass.localPosition;
}

private void OnCollisionEnter(Collision _col)
{ 
float num = Mathf.Clamp01(_col.relativeVelocity.magnitude / 20f);
if (!this.crashAudioSource.isPlaying || num > this.lastCrashVolume)
{ 
this.crashAudioSource.volume = num;
this.lastCrashVolume = num;
this.crashAudioSource.Play();
}
if (_col.collider.tag == "Off" || _col.collider.tag == "Water")
		{
            DropReset();
        }
}

private void Update()
{ 
    boostEngaged = (Input.GetKey(KeyCode.RightShift) || Input.GetButton("Fire4"));
    //Steer = GameManager.Instance.InputController.SteerInput;
    //Throttle = GameManager.Instance.InputController.ThrottleInput;
    foreach (var wheel in wheels)
    {
        wheel.SteerAngle  = Steer * maxSteer;
        
        if (boostEngaged){
            wheel.Torque = Throttle * (motorTorque + boostForce);
        } else
        {
            wheel.Torque = Throttle * motorTorque;
        }
    }

    if (Input.GetKeyDown(KeyCode.RightControl)){
         DropReset();
    } 

this.engineSoundRPM = 0f;
WheelCollider[] array2 = this.engineSoundWheels;
for (int i = 0; i < array2.Length; i++)
{ 
WheelCollider wheelCollider = array2[i];
this.engineSoundRPM += wheelCollider.rpm;
}
this.engineSoundRPM /= (float)this.engineSoundWheels.Length;
this._engineRPM = this.minEngineRPM + this.engineSoundRPM * this.finalDriveRatio * this.GearRatio[this._currentGear];
if (this._engineRPM > this.maxEngineRPM)
{ 
this._engineRPM = this.maxEngineRPM;
}
if (this._engineRPM < this.minEngineRPM)
{ 
this._engineRPM = this.minEngineRPM;
}
this.ShiftGears();
float pitch = Mathf.Lerp(this.engineAudioSource.pitch, Mathf.Abs((2f * Mathf.Abs(this._engineRPM / this.maxEngineRPM) + 0.2f) * 1.2f), Time.deltaTime * 10f);
this.engineAudioSource.pitch = pitch;
this.engineAudioSourceDistorted.pitch = pitch;
this.engineAudioSourceDistorted.volume = this._engineRPM / this.maxEngineRPM;
}

private void ShiftGears()
{ 
int currentGear = this._currentGear;
if (this._engineRPM >= this.maxEngineRPM * 0.8f)
{ 
currentGear = this._currentGear;
for (int i = 1; i < this.GearRatio.Length; i++)
{ 
if (this.engineSoundRPM * this.finalDriveRatio * this.GearRatio[i] < this.maxEngineRPM * 0.8f)
{ 
currentGear = i;
break;
}
}
this._currentGear = currentGear;
return;
}
if (this._engineRPM < this.minEngineRPM * 2.8f)
{ 
currentGear = this._currentGear;
for (int j = this.GearRatio.Length - 1; j >= 1; j--)
{ 
if (this.engineSoundRPM * this.finalDriveRatio * this.GearRatio[j] > this.minEngineRPM * 2f)
{ 
currentGear = j;
break;
}
}
}
this._currentGear = currentGear;
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
    
}
