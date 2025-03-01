using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{ 
public GameObject UIPanelRaceTop;
public GameObject UIPanelRaceDown;

public GameObject UIPanelPause;

public Text UITextCurrentLap;

public Text UITextCurrentSpeed;

public Text UITextCurrentTime;

public Text UITextLastLapTime;

public Text UITextBestLapTime;

public Text UITextBestHotLap;

public Text UITextLapsCompleted;

//public Button UIButtonController;

private int currentLap = -1;

private double currentSpeed = 0;

private float currentLapTime;

private float lastLapTime;

private float bestLapTime;

public Player UpdateUIForPlayer;

public void ResumeGame()
{
    Time.timeScale = 1f;
    Debug.Log("Reprise");
    UIPanelRaceTop.SetActive(true);
    UIPanelRaceDown.SetActive(true);
    UIPanelPause.SetActive(false);
}

public void QuitToMenu()
{
    Time.timeScale = 1f;
    Debug.Log("Quit To Menu");
    SceneManager.LoadScene("Menu");
}

public void RestartGame()
{
    Time.timeScale = 1f;
    Debug.Log("Restart");
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
    UIPanelRaceTop.SetActive(true);
    UIPanelRaceDown.SetActive(true);
    UIPanelPause.SetActive(false);
}

private void Update()
{ 
      //Exit
    if (Input.GetKey("escape"))
        {
            if (Time.timeScale == 1.0f){
                Time.timeScale = 0f;
                Debug.Log("MenuPause");
                UIPanelRaceTop.SetActive(false);
                UIPanelRaceDown.SetActive(false);
                UIPanelPause.SetActive(true);
                
            } else {
                //Application.Quit();
            }
        }

    

    if (this.UpdateUIForPlayer == null)
        { 
            return;
        }
 
    if (this.UpdateUIForPlayer.CurrentLap != this.currentLap)
    { 
        this.currentLap = this.UpdateUIForPlayer.CurrentLap;
        this.UITextCurrentLap.text = string.Format("TOUR: {0}", this.currentLap);
        //this.UITextLapsCompleted.text = "Nombre de tours : " + this.currentLap;
        if (this.currentLap > 1)
        { 
            PlayerPrefs.SetInt("LapsCompleted", PlayerPrefs.GetInt("LapsCompleted") + 1);
            this.UITextLapsCompleted.text = "Nombre de tours : " + this.currentLap;
        }
    }
    //Show Speed !
    if (this.UpdateUIForPlayer.CurrentSpeed != this.currentSpeed)
    { 
        this.currentSpeed = this.UpdateUIForPlayer.CurrentSpeed;
        this.UITextCurrentSpeed.text = string.Format("VITESSE: {0} km/h", (int)this.currentSpeed);
    }
    if (this.UpdateUIForPlayer.CurrentLapTime != this.currentLapTime)
    { 
        this.currentLapTime = this.UpdateUIForPlayer.CurrentLapTime;
        this.UITextCurrentTime.text = string.Format("TEMPS: {0}:{1:00.000}", (int)this.currentLapTime / 60, this.currentLapTime % 60f);
    }
    if (this.UpdateUIForPlayer.LastLapTime != this.lastLapTime)
    { 
        this.lastLapTime = this.UpdateUIForPlayer.LastLapTime;
        this.UITextLastLapTime.text = string.Format("DERNIER: {0}:{1:00.000}", (int)this.lastLapTime / 60, this.lastLapTime % 60f);
    }
    if (this.UpdateUIForPlayer.BestLapTime != this.bestLapTime)
    { 
        //Bug ici
        this.bestLapTime = this.UpdateUIForPlayer.BestLapTime;
        this.UITextBestLapTime.text = ((this.bestLapTime < 1000000f) ? string.Format("MEILLEUR: {0}:{1:00.000}", (int)this.bestLapTime / 60, this.bestLapTime % 60f) : "MEILLEUR: Aucun");
        this.UITextBestHotLap.text = ((this.bestLapTime < 1000000f) ? string.Format("Meilleur Temps : {0}:{1:00.000}", (int)this.bestLapTime / 60, this.bestLapTime % 60f) : "Meilleur temps : Aucun");
        if (this.bestLapTime < PlayerPrefs.GetFloat("BestHotLap"))
        { 
            PlayerPrefs.SetFloat("BestHotLap", this.bestLapTime);
            //this.UITextBestHotLap.text = "Nombre de tours : " + this.bestLapTime;
        } else {
            //Test MaJ Ecran Principal
            PlayerPrefs.SetFloat("BestHotLap", this.bestLapTime);
        }
    }
}
}
