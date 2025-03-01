using System;
using UnityEngine;


public class WheelSmoke : MonoBehaviour
{ 
private Rigidbody rbody;

private WheelCollider wheelCollider;

private WheelHit wheelHitInfo;

private ParticleSystem particleSys;

private void Start()
{ 
this.rbody = base.GetComponentInParent<Rigidbody>();
this.particleSys = base.GetComponent<ParticleSystem>();
this.wheelCollider = base.GetComponentInParent<WheelCollider>();
}

private void Update()
{ 
if (this.wheelCollider.GetGroundHit(out this.wheelHitInfo))
{ 
ParticleSystem.EmissionModule emission = this.particleSys.emission;
float constant = Mathf.Clamp(Mathf.Abs(this.wheelHitInfo.sidewaysSlip) - 0.3f, 0f, 100f) * 8f;
Mathf.Clamp(Mathf.Abs(this.wheelHitInfo.forwardSlip) -0.3f, 0f, 100f);
emission.rateOverDistance = constant;
return;
}
//this.particleSys.emission.rateOverDistance = 0f;
}
}
