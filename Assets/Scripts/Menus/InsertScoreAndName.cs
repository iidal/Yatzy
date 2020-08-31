using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsertScoreAndName : MonoBehaviour
{

    public LetterRoller[] rollers;  //used to choose the character

    string playerName;

    string[] allChars;  //all possible chars that can be used, gotten from a text file
    int charCount = 0; //how many possible chars
    public TextMeshProUGUI pointsText;
    
    void Awake()    //called when game ends and panel for this is activated
    {
        pointsText.text = "You got " +GameManager.instance.playerInTurn.points + " points"; //show points in panel
        AssignChars(); //get chars from file
        foreach(LetterRoller lr in rollers){
            lr.GetChars(allChars, charCount);   //pass chars to individual rollers
        }
    }

    public void GetName(){  //when player has chosen name, save it with score
        playerName = "";
        foreach(LetterRoller lr in rollers){
            playerName += allChars[lr.currentCharIndex];
        }
        SaveLoad.SaveSoloResults(playerName, GameManager.instance.playerInTurn.points);
        GameNotificationManager.instance.ShowNotification("GameOverPanel");
        this.gameObject.SetActive(false);       //hide this panel

    }
    void AssignChars(){

        TextAsset textAsset = Resources.Load<TextAsset>("NameCharacters");
        allChars = textAsset.text.Split(',');
        charCount = allChars.Length;
    }


}
