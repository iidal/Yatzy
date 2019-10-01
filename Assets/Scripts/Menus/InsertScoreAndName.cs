using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsertScoreAndName : MonoBehaviour
{

    public LetterRoller[] rollers;

    string playerName;

    string[] allChars;
    int charCount = 0;
    public TextMeshProUGUI pointsText;
    
    void Awake()
    {
        pointsText.text = "You got " +GameManager.instance.playerInTurn.points + " points";
        AssignChars();
        foreach(LetterRoller lr in rollers){
            lr.GetChars(allChars, charCount);
        }
    }

    public void GetName(){
        playerName = "";
        foreach(LetterRoller lr in rollers){
            playerName += allChars[lr.currentCharIndex];
        }
        SaveLoad.SaveSoloResults(playerName, GameManager.instance.playerInTurn.points);
        Debug.Log(playerName);
        GameNotificationManager.instance.ShowNotification("GameOverPanel");
        this.gameObject.SetActive(false);

    }
    void AssignChars(){

        TextAsset textAsset = Resources.Load<TextAsset>("NameCharacters");
        allChars = textAsset.text.Split(',');
        charCount = allChars.Length;
    }


}
