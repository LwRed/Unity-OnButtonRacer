using System;
using UnityEngine;


[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour
{ 
public AudioSource wheelSkidSfx;

public AnimationCurve wheelSkidVolumeCurve;

public AnimationCurve wheelSkidPitchCurve;

private Rigidbody rb;

private SkidmarksController skidmarksController;

private WheelCollider wheelCollider;

private WheelHit wheelHitInfo;

private const float SKID_FX_SPEED = 0.5f;

private const float MAX_SKID_INTENSITY = 20f;

private const float WHEEL_SLIP_MULTIPLIER = 10f;

private int lastSkid = -1;

private float lastFixedUpdateTime;

protected void Start()
{ 
this.rb = base.GetComponentInParent<Rigidbody>();
this.skidmarksController = GameManager.Instance.SkidmarksController;
this.wheelCollider = base.GetComponent<WheelCollider>();
this.lastFixedUpdateTime = Time.time;
}

protected void FixedUpdate()
{ 
this.lastFixedUpdateTime = Time.time;
}

protected void LateUpdate()
{ 
if (this.wheelCollider.GetGroundHit(out this.wheelHitInfo))
{ 
float num = Mathf.Abs(this.wheelHitInfo.sidewaysSlip) * 20f;
num += Mathf.Abs(this.wheelHitInfo.forwardSlip) * 5f;
if (num < 0.5f)
{ 
if (this.wheelSkidSfx != null)
{ 
this.wheelSkidSfx.volume = this.wheelSkidVolumeCurve.Evaluate(0f);
this.wheelSkidSfx.pitch = this.wheelSkidPitchCurve.Evaluate(0f);
}
this.lastSkid = -1;
return;
}
float num2 = Mathf.Clamp01(num / 20f);
Vector3 pos = this.wheelHitInfo.point + this.rb.linearVelocity * (Time.time - this.lastFixedUpdateTime);
this.lastSkid = this.skidmarksController.AddSkidMark(pos, this.wheelHitInfo.normal, num2, this.lastSkid);
if (this.wheelSkidSfx != null)
{ 
this.wheelSkidSfx.volume = this.wheelSkidVolumeCurve.Evaluate(num2);
this.wheelSkidSfx.pitch = this.wheelSkidPitchCurve.Evaluate(num2);
return;
}
}
else
{ 
if (this.wheelSkidSfx != null)
{ 
this.wheelSkidSfx.volume = this.wheelSkidVolumeCurve.Evaluate(0f);
this.wheelSkidSfx.pitch = this.wheelSkidPitchCurve.Evaluate(0f);
}
this.lastSkid = -1;
}

}
}
