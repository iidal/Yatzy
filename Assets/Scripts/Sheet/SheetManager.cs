﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Managing things about showing points and playing lines
/// </summary>

public class SheetManager : MonoBehaviour
{
    public static SheetManager instance;

    public Button playButton; //play a line 

    public int[] currentDices = new int [5];    //current dice row

    public SingleLine[] sheetLines; //scripts for the lines
    public GameObject[] lineObjects;    //lines as gameobjects
    public GameObject linePrefab;   //line gameobject to be instantiated
    public GameObject lineParent; //holds the lines
    public ToggleGroup lineToggleGroup; // access to all lines toggles

    public GameObject clickBlocker; //block clicks on the sheet when throwing etc

    int upperPoints = 0;
    public SingleLine upperBonusLine;  //reference to the upper lines bonus (if the sheet has one) 

    //if needed
    //Dictionary<int, string> lineNames = new Dictionary<int, string>();
    //Dictionary<int, int> lineScores = new Dictionary<int, int>();
    //Dictionary<int, bool> playedLines = new Dictionary<int, bool>();


    /////////////

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }
    private void Start()
    {
        playButton.interactable = false;
        CreateSheet();    //get lines from a text file and set up both line dictionaries
        clickBlocker.SetActive(true);
    }


    //line has been chosen, play it
    public void PlayRound() {  

        //get the chosen line and do needed things in it
        Toggle chosenToggle = lineToggleGroup.ActiveToggles().FirstOrDefault();
        SingleLine sl = chosenToggle.GetComponent<SingleLine>();
        sl.PlayThis();

        clickBlocker.SetActive(true);

        GameManager.instance.playerInTurn.AddToPoints(sl.points); //send points to the players score
        
        //check the upper section if needed
        if (sl.lineType.Contains("upper")) {
            CheckUpperLine(sl);
        }

       
        GameManager.instance.StartNextTurn();       //new round
    }


    public void CalculateLines(int[] line) {

        currentDices = line;
        LineCalculator.StartCalculating(sheetLines, currentDices);  // send the used sheet and the numbers to be calculated. the results are passed on to SingleLines of the sheetlines

        clickBlocker.SetActive(false);          //lines can be clicked
        if (!GameManager.instance.roundEnded) { //let player throw again once the dices have stopped moving, except when all throws have been used(round ended p much)
            DiceParent.instance.ThrowButton.interactable = true;
        }
    }
    
    //check the points in upper section and if bonus can be given
    void CheckUpperLine(SingleLine line) {
        upperPoints += line.points;
        if (upperPoints >= 63 && upperBonusLine.hasBeenPlayed ==false) //so that the bonus points are not given mopre than once
        {
            upperBonusLine.SetOtherPoints(true);
            upperBonusLine.PlayThis();
            GameManager.instance.playerInTurn.AddToPoints(upperBonusLine.points);
        }
    }
    //If a line is selected play button can be pressed
    public void CheckPlayButton() {
        if (lineToggleGroup.AnyTogglesOn() == true)
            playButton.interactable = true;
        else
            playButton.interactable = false;

    }

    //doesnt clear played points, but the toggle that has been clicked but not confirmed
    //used when throwing 
    public void ClearSheet() {
        lineToggleGroup.SetAllTogglesOff();
    }

    /*
     *Creating lines
     */
    void CreateSheet()
    {
        bool hasUpperBonus = false; //does the sheet contain line for upperbonus. if not, the game has as many rounds as the sheet has lines. if yes rounds = sheet length -1. checked after the for loop

        TextAsset textAsset = Resources.Load<TextAsset>("YatzyLinesVer1");
        string[] lineArray = textAsset.text.Split('\n');
        sheetLines = new SingleLine[lineArray.Length];
        lineObjects = new GameObject[lineArray.Length];
        for (int i = 0; i < lineArray.Length; i++) {
            string[] tempLine = lineArray[i].Split('-');        //splitting line from text asset to id name and score
            GameObject lineGo = Instantiate(linePrefab);        //creating new line
            lineGo.transform.SetParent(lineParent.transform);     //parenting it
            lineGo.name = tempLine[1];                          //change gameobject name
            lineObjects[i] = lineGo;                            //store the lines as gameobjects
            SingleLine sl = lineGo.GetComponent<SingleLine>();  //temp reference to the SingleLine of the object
            sheetLines[i] = sl;                                 //adding lines script to this scripts array
            sl.id = System.Int32.Parse(tempLine[0]);            //setting line's id, line name and possible score
            sl.lineName = tempLine[1];
            sl.lineType = tempLine[2];
            sl.pointsDefault = System.Int32.Parse(tempLine[3]);
            lineGo.GetComponent<Toggle>().group = lineParent.GetComponent<ToggleGroup>();   //set lines toggle group

            if (sl.lineType == "upBonus") { //line calculating the bonus for the lines 1-6 is not handled where other lines are calculated (because it does not rely on the dices, but scores from the upper section)
                upperBonusLine = sl;
                hasUpperBonus = true;
            }


        }
        //game has as many rounds as the sheet has lines, minus the upper bonus line
        if (hasUpperBonus)
        {
            GameManager.instance.roundsPerGame = sheetLines.Length - 1; 
        }
        else {
            GameManager.instance.roundsPerGame = sheetLines.Length;
        }
    }

    /*
     Resetting for new game
     */

        //delete and create new sheet
    public void ResetSheet() {
        upperPoints = 0;
        //delete previous line objects before creating new ones
        foreach (GameObject go in lineObjects) {
            Destroy(go);
        }
        //create new sheet
        CreateSheet();
    }

}
