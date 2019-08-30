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

    public void SetPoint(int calcPoints) {

        points = calcPoints;
        scoreText.text = points.ToString();

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
