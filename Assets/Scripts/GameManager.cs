using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{ 

public AudioClip lapSoundEffect;

public int AskedFrameRate = 60;

public static GameManager Instance { get; private set; }

public InputController InputController{ get; private set; }

public SkidmarksController SkidmarksController{ get; private set; }

private void Awake()
{

Application.targetFrameRate = AskedFrameRate;
GameManager.Instance = this;
this.InputController = base.GetComponentInChildren<InputController>();
this.SkidmarksController = base.GetComponentInChildren<SkidmarksController>();

//Volume Audio Get temporary float
float loadedVolume = PlayerPrefs.GetFloat("MusicVol",1f);
Debug.Log ("Valeur Volume chargée : " + loadedVolume);
//Make Volume with Prefs Load
AudioListener.volume = loadedVolume;

}
}
