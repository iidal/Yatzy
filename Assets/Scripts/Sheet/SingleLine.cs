using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleLine : MonoBehaviour
{
    public int id;  //id for this line
    public string lineName;
    public string lineType;
    public int points; //if zero, points are calculated from dices
    int playedPoints;   //
    bool linePlayed;
    Toggle toggle;
    //Text objects
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI chosenScoreText;

    

    private void Start()
    {
        SetTextObjects();

        toggle = GetComponent<Toggle>();
        toggle.isOn = false;
        if (lineType == "upperBonus") {
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
            scoreText.text = points.ToString();
        }
        else {
            scoreText.text = "0";
        }
    }

    public void PlayThis() {
        scoreText.enabled = false;
        chosenScoreText.text = points.ToString();
        toggle.interactable = false;
        toggle.isOn = false;
    }

    public void ValueChange() {
        SheetManager.instance.CheckPlayButton();
    }

    void SetTextObjects() {
        TextMeshProUGUI[] tempObjs = GetComponentsInChildren<TextMeshProUGUI>();
        labelText = tempObjs[0];
        scoreText = tempObjs[1];
        chosenScoreText = tempObjs[2];

        labelText.text = lineName;
    }
}
