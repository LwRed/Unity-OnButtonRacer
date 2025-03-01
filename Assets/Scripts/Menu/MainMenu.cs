using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Text UIBestLapTime;
    public Text UINumberOfLaps;

    //Declaration qui permet de rattacher les composants au script MainMenu.cs
    public Slider VolumeSlider;
    //public GameObject OptionsMenu;

    private void Awake()
    {
        
        //Load BestLapTime
        float @float = PlayerPrefs.GetFloat("BestHotLap");
        this.UIBestLapTime.text = ((@float < 1000000f) ? string.Format("Meilleur tour : {0}:{1:00.000}", (int)@float / 60, @float % 60f) : "Meilleur tour : Aucun");            


        //Load NumberOfLaps Raced
        int @int = PlayerPrefs.GetInt("LapsCompleted");
        this.UINumberOfLaps.text = ((@int > 1) ? string.Format("Tours effectués : {0}", @int) : "Aucun tour");
        //this.UINumberOfLaps.text = string.Format("Tours effectués : {0}", @int);

        //OptionsMenu.gameObject.SetActive(true);

        //Volume Audio Get temporary float
        float loadedVolume = PlayerPrefs.GetFloat("MusicVol",1f);
        Debug.Log ("Volume : " + loadedVolume);
        //Make Volume with Prefs Load
        AudioListener.volume = loadedVolume;

        //Set Slider with Loaded Prefs
        VolumeSlider.value = loadedVolume;

        //DeActivate OptionsMenu
        //OptionsMenu.gameObject.SetActive(false);
    }

    public void OnePlayerGame ()
    {
        SceneManager.LoadScene("jumptrack2PAuto");
        Debug.Log("1 joueur Time Trial");
    }

    public void TwoPlayerGame ()
    {
        SceneManager.LoadScene("jumptrack2P");
        Debug.Log("2 joueurs - 10 tours");
    }

     public void QuitGame ()
    {
        Debug.Log("Quitter");
        Application.Quit();       
    }

}
