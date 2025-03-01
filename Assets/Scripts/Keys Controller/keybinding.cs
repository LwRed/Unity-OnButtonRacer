using UnityEngine;
using System;
using TMPro;
//using UnityEngine.UI; // Si Text au lieu de TextMeshProUGUI

public class keybinding : MonoBehaviour
{
[Header("Objects")]
[SerializeField] private TextMeshProUGUI goKeyP1;
[SerializeField] private TextMeshProUGUI goKeyP2;

private void Start()
{
    if (PlayerPrefs.HasKey("Player1")) {goKeyP1.text = PlayerPrefs.GetString("Player1");}
    if (PlayerPrefs.HasKey("Player2")) {goKeyP2.text = PlayerPrefs.GetString("Player2");}
}

private void Update()
{
    if (goKeyP1.text == "Attente touche")
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keycode))
            {
                goKeyP1.text = keycode.ToString();
                PlayerPrefs.SetString("Player1", keycode.ToString());
                PlayerPrefs.Save();
            }
        }
    } else if (goKeyP2.text == "Attente touche")
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keycode))
            {
                goKeyP2.text = keycode.ToString();
                PlayerPrefs.SetString("Player2", keycode.ToString());
                PlayerPrefs.Save();
            }
        }
    }

}

public void ChangegokeyP1()
{
    goKeyP1.text = "Attente touche";
}
public void ChangegokeyP2()
{
    goKeyP2.text = "Attente touche";
}

}
