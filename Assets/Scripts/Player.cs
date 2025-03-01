using System;
using UnityEngine;


public class Player : MonoBehaviour
{ 
public enum WhichPlayer { Player1, Player2, AI}

public Player.WhichPlayer ThisPlayer = WhichPlayer.AI;



public float BestLapTime { get; private set; } = Mathf.Infinity;
public float LastLapTime{ get; private set; } = 0;
public float CurrentLapTime { get; private set; } = 0;
public int CurrentLap { get; private set; } = 0;
public double CurrentSpeed { get; private set; } = 0;

private float lapTimer;
private int lastCheckpointPassed = 0;

private Transform checkpointsParent;
private int checkpointCount;
private int checkpointLayer;
private CarController carController;
private Rigidbody rb;

private void Awake()
{ 
this.checkpointsParent = GameObject.Find("Checkpoints").transform;
this.checkpointCount = this.checkpointsParent.childCount;
this.checkpointLayer = LayerMask.NameToLayer("Checkpoint");
this.carController = base.GetComponent<CarController>();
rb = gameObject.GetComponent<Rigidbody>();

}

private void Update()
{ 

this.CurrentLapTime = ((this.lapTimer > 0f) ? (Time.time - this.lapTimer) : 0f);
//Get Speed
this.CurrentSpeed = rb.linearVelocity.magnitude*3.6;

//if (this.controlType == Player.ControlType.HumanInput)
//{ 
//this.carController.Steer = GameManager.Instance.InputController.SteerInput;
//this.carController.Throttle = GameManager.Instance.InputController.ThrottleInput;
//}
}

private void StartLap()
{ 
int currentLap = this.CurrentLap;
this.CurrentLap = currentLap + 1;
this.lastCheckpointPassed = 1;
this.lapTimer = Time.time;
AudioSource.PlayClipAtPoint(GameManager.Instance.lapSoundEffect, base.transform.position);
}

private void EndLap()
{ 
this.LastLapTime = Time.time - this.lapTimer;
this.BestLapTime = Mathf.Min(this.LastLapTime, this.BestLapTime);
}

private void OnTriggerEnter(Collider collider)
{ 
    if (collider.gameObject.layer != this.checkpointLayer)
    { 
        return;
    }

    if (collider.gameObject.name == "1")
    { 
        if (this.lastCheckpointPassed == this.checkpointCount)
        { 
            this.EndLap();
        }

        if (this.CurrentLap == 0 || this.lastCheckpointPassed == this.checkpointCount)
        { 
            this.StartLap();
        }
    return;
    }

    if (collider.gameObject.name == (this.lastCheckpointPassed + 1).ToString())
    { 
        this.lastCheckpointPassed++;
    }
}
}