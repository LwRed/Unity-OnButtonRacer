using System;
using UnityEngine;


public class InputController : MonoBehaviour
{ 
public string inputSteerAxis = "Horizontal";

public string inputThrottleAxis = "Vertical";

public float ThrottleInput
{ 
get;
private set;
}

public float SteerInput
{ 
get;
private set;
}

private void Start()
{ 
}

private void Update()
{ 
this.SteerInput = Input.GetAxis(this.inputSteerAxis);
this.ThrottleInput = Input.GetAxis(this.inputThrottleAxis);
}
}
