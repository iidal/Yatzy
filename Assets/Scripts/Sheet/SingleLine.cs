using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// script for an individual line, holds info on line points, name etc
/// </summary>

public class SingleLine : MonoBehaviour
{
    //info of the line, id, name, type, points
    public int id;  
    public string lineName;
    public string lineType; //upper section, a straight, pair....
    public int pointsDefault; //if zero, points are calculated from dices
    public int points;   //points that were played and added to score

    Toggle toggle;  // reference to this lines toggle

    public bool hasBeenPlayed = false;  //has the line been played

    //Text objects
    public TextMeshProUGUI labelText;   //line's name
    public TextMeshProUGUI scoreText;   //showing current score, the points the player gets if they play this line
    public TextMeshProUGUI chosenScoreText; //when the line is played this text is enabled with the played points and stays up

    

    private void Start()
    {
        SetTextObjects();   //getting references to text objects and setting them up
        toggle = GetComponent<Toggle>();
        toggle.isOn = false;    //turning toggles off in the beginning
        if (lineType == "upBonus") {    //line for upper bonus is always turned off
            toggle.interactable = false;
        }
        
    }

    //if points are calculated from dices, this is used
    //with lines like 3x and pairs calcpoints is 0 if requirements are not met 
    public void SetDicePoints(int calcPoints) {

        points = calcPoints;
        scoreText.text = points.ToString();

    }
    //if points are always the same, for example with yatzy and full house, this is used
    //lineok = points can be given, if false, give 0
    public void SetOtherPoints(bool lineOK) {
        if (lineOK)
        {
            points = pointsDefault; //get the points for this line
            scoreText.text = points.ToString();
        }
        else {
            points = 0;
            scoreText.text = "0";
        }
    }
    //when a sheet is loaded from file, use this to set the points
    public void SetPointsFromFile(int p){
        points = p;
        scoreText.text = points.ToString();
        PlayThis();
    }
    //this line has been chosen to be played this round
    public void PlayThis() {    
        scoreText.enabled = false;
        chosenScoreText.text = points.ToString();   //shows the received points 
        toggle.interactable = false;    //cant ble clicked again
        toggle.isOn = false;    //toggle is not chosen
        hasBeenPlayed = true;
    }

    public void ValueChange() { //lines toggle has been clicked
        SheetManager.instance.CheckPlayButton();    //check if any toggle is on, if not, play button cant be clicked
    }

    void SetTextObjects() {//setting up the text objects
        TextMeshProUGUI[] tempObjs = GetComponentsInChildren<TextMeshProUGUI>();
        labelText = tempObjs[0];
        scoreText = tempObjs[1];
        chosenScoreText = tempObjs[2];

        labelText.text = lineName;
    }
}
