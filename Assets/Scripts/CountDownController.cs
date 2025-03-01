using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CountDownController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public AudioClip countdownAudio;
    //public GameObject Player;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public GameObject Player5;
    public GameObject Player6;
    //public GameObject Player2;

    //Laps
    public int numberOfLaps = 3;
    private int winner = 0;
    public GameObject UIPanelPause;
    public Text UIWinner;

    //B-Spec
    public GameObject ReglageVmaxP1;
    public GameObject ReglageVmaxP2;
    public GameObject ReglageCollP1;
    public GameObject ReglageCollP2;

    private void Start()
        {
            StartCoroutine(CountDownToStart());
            AudioSource.PlayClipAtPoint(countdownAudio, transform.position, 1f);
        }

    private void Update()
        {
            checkWinner();

            //Update VitesseMaximum
            Player1.GetComponent<CarEngineSemiAi>().maximumSpeed = ReglageVmaxP1.GetComponent<Slider>().value;
            Player2.GetComponent<CarEngineSemiAi>().maximumSpeed = ReglageVmaxP2.GetComponent<Slider>().value;
            //Update AntiCollision
            Player1.GetComponent<CarEngineSemiAi>().sensorLength = ReglageCollP1.GetComponent<Slider>().value;
            Player2.GetComponent<CarEngineSemiAi>().sensorLength = ReglageCollP2.GetComponent<Slider>().value;
        }

        private void checkWinner() 
        {
            if (Player1.GetComponent<Player>().CurrentLap > numberOfLaps){
                winner = 1;
                Debug.Log("Joueur 1 a gagné !");
            } else if (Player2.GetComponent<Player>().CurrentLap > numberOfLaps) {
                winner = 2;
                Debug.Log("Joueur 2 a gagné !");
            } else if (Player3.GetComponent<Player>().CurrentLap > numberOfLaps) {
                winner = 3;
                Debug.Log("Joueur 3 a gagné !");
            } else if (Player4.GetComponent<Player>().CurrentLap > numberOfLaps) {
                winner = 4;
                Debug.Log("Joueur 4 a gagné !");
            } else if (Player5.GetComponent<Player>().CurrentLap > numberOfLaps) {
                winner = 5;
                Debug.Log("Joueur 5 a gagné !");
            } else if (Player6.GetComponent<Player>().CurrentLap > numberOfLaps) {
                winner = 6;
                Debug.Log("Joueur 6 a gagné !");
            } 

            if (winner > 0) {
                Debug.Log("MenuPause");
                UIPanelPause.SetActive(true);
                UIWinner.text = "Vainquer : Joueur " + string.Format("{0}", winner);
            }

        }

    IEnumerator CountDownToStart()
    {
        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(0.8f);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);

        //Start Session Player1
        Player1.GetComponent<CarEngineSemiAi>().enabled = true;
        //Player1.GetComponent<CarEngineSemiAi>().enabled = true;
        Player2.GetComponent<CarEngineSemiAi>().enabled = true;
        //Player1.GetComponent<CarEngine>().enabled = true;
        //Player2.GetComponent<CarEngine>().enabled = true;
        Player3.GetComponent<CarEngine>().enabled = true;
        Player4.GetComponent<CarEngine>().enabled = true;
        Player5.GetComponent<CarEngine>().enabled = true;
        Player6.GetComponent<CarEngine>().enabled = true;

    }

}
