using System;
using UnityEngine;


public class CarAudio : MonoBehaviour
{ 
public float topSpeed = 100f;

private float currentSpeed;

private float pitch;

private void Update()
{ 
this.currentSpeed = base.transform.GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f;
this.pitch = this.currentSpeed / this.topSpeed;
base.GetComponent<AudioSource>().pitch = this.pitch;
}
}
