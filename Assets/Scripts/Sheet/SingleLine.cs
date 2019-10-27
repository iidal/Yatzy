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
    public int extraPoints; //ie multiyatzys. this variable is changed from elsewhere at the moment and is not checked in this script (checked in sheetmanager)
    Toggle toggle;  // reference to this lines toggle

    public bool hasBeenPlayed = false;  //has the line been played

    //Text objects
    public TextMeshProUGUI labelText;   //line's name
    public TextMeshProUGUI scoreText;   //showing current score, the points the player gets if they play this line
    public TextMeshProUGUI chosenScoreText; //when the line is played this text is enabled with the played points and stays up
    public TextMeshProUGUI extraPointsText; // for multiple yatzy points
    public TextMeshProUGUI upperBonusCheckingText;  // tracking upper section points and showing them to player



    private void Start()
    {
        SetTextObjects();   //getting references to text objects and setting them up
        toggle = GetComponent<Toggle>();
        toggle.isOn = false;    //turning toggles off in the beginning
        if (lineType == "upBonus")
        {    //line for upper bonus is always turned off
            toggle.interactable = false;
        }

    }

    //if points are calculated from dices, this is used
    //with lines like 3x and pairs calcpoints is 0 if requirements are not met 
    public void SetDicePoints(int calcPoints)
    {

        points = calcPoints;
        scoreText.text = points.ToString();

    }
    //if points are always the same, for example with yatzy and full house, this is used
    //lineok = points can be given, if false, give 0
    public void SetOtherPoints(bool lineOK)
    {
        if (lineOK)
        {
            points = pointsDefault; //get the points for this line
            scoreText.text = points.ToString();
        }
        else
        {
            points = 0;
            scoreText.text = "0";
        }
    }
    //when a sheet is loaded from file, use this to set the points
    public void SetPointsFromFile(int p, int ep)
    {
        points = p;

        scoreText.text = points.ToString();
        if (ep != 0)
        {
            ShowExtraPoints(ep);
        }
        PlayThis();
    }
    public void ShowExtraPoints(int ep)
    {
        extraPoints = ep;
        extraPointsText.text = extraPoints.ToString();
    }
    //this line has been chosen to be played this round
    public void PlayThis()
    {
        scoreText.enabled = false;
        chosenScoreText.text = points.ToString();   //shows the received points 
        toggle.interactable = false;    //cant ble clicked again
        toggle.isOn = false;    //toggle is not chosen
        hasBeenPlayed = true;
    }

    //this is only needed for upper bonus line
    public void UpperBonusUpdating(int currentPoints){
        upperBonusCheckingText.text = currentPoints.ToString() + "/63";
    }

    public void ValueChange()
    { //lines toggle has been clicked
        SheetManager.instance.CheckPlayButton();    //check if any toggle is on, if not, play button cant be clicked
    }

    void SetTextObjects()
    {//setting up the text objects
        TextMeshProUGUI[] tempObjs = GetComponentsInChildren<TextMeshProUGUI>();
        labelText = tempObjs[0];
        scoreText = tempObjs[1];
        chosenScoreText = tempObjs[2];
        if(lineType == "upBonus"){
            upperBonusCheckingText = tempObjs[3];
        }else{
            extraPointsText = tempObjs[3];
        }
        labelText.text = lineName;
    }
}
