using System;
using UnityEngine;


public class CameraLookat : MonoBehaviour
{ 
public Transform target;

public float RotateSmoothTime = 0.1f;

private float AngularVelocity;

private void LateUpdate()
{ 
Quaternion b = Quaternion.LookRotation(this.target.position - base.transform.position);
float num = Quaternion.Angle(base.transform.rotation, b);
if (num > 0f)
{ 
float num2 = Mathf.SmoothDampAngle(num, 0f, ref this.AngularVelocity, this.RotateSmoothTime);
num2 = 1f - num2 / num;
base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, num2);
}
}
}
