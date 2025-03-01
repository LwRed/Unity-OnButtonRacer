using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CountDownControllerAuto : MonoBehaviour
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

    private void Start()
        {
            StartCoroutine(CountDownToStart());
            AudioSource.PlayClipAtPoint(countdownAudio, transform.position, 1f);
        }

    private void Update()
        {
            if (Player1.GetComponent<Player>().CurrentLap > numberOfLaps){
                winner = 1;
                Debug.Log("Player 1 Wins");
            } else if (Player2.GetComponent<Player>().CurrentLap > numberOfLaps) {
                winner = 2;
                Debug.Log("Player 2 Wins");
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

        //Start Session
        //Player1.GetComponent<CarEngineSemiAi>().enabled = true;
        //Player2.GetComponent<CarEngineSemiAiP2>().enabled = true;
        Player1.GetComponent<CarEngine>().enabled = true;
        Player2.GetComponent<CarEngine>().enabled = true;
        Player3.GetComponent<CarEngine>().enabled = true;
        Player4.GetComponent<CarEngine>().enabled = true;
        Player5.GetComponent<CarEngine>().enabled = true;
        Player6.GetComponent<CarEngine>().enabled = true;

    }

}
