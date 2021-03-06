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
    public string sheetVersion = "YatzyLinesVer1";

    public Button playButton; //play a line 
    public Animator playButtonAnimator; // for using the 3D button
    public ColliderButton playBut; // 3DButton
    public int[] currentDices = new int[5];    //current dice row

    public SingleLine[] sheetLines; //scripts for the lines
    public GameObject[] lineObjects;    //lines as gameobjects
    public GameObject linePrefab;   //line gameobject to be instantiated
    public GameObject upperBonusLinePrefab; // upperbonusline to be instatiated
    public GameObject lineParent; //holds the lines
    public ToggleGroup lineToggleGroup; // access to all lines toggles

    public GameObject clickBlocker; //block clicks on the sheet when throwing etc

    int upperPoints = 0;
    public SingleLine upperBonusLine;  //reference to the upper lines bonus (if the sheet has one) 

    //for determining what points should be given considering a possible yatzy
    public bool currentLineIsYatzy = false;    //is the current line yatzy (not played yet)
    public bool yatzyPlayed = false;       //has yatzy line been played?   (check if sheet has yatzy line)
    public bool yatzyZeroPoints = false;    //if the yatzy line has been played with 0 or 50 points. if zero, the player will not be awarded an extra 50 points on other lines for five same dices
    public int YatzyExtraPoints = 50;  //will be set again in CreateSheet() in case yatzy points are not 50p


    List<SavedSheetLine> savedLines = new List<SavedSheetLine>();  //saved lines go here

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
        //playButton.interactable = false;

        playBut.enabled = false;
        playButtonAnimator.Play("ColorDisabled");

        clickBlocker.SetActive(true);

        CreateSheet();    //get lines from a text file and set up everything
        //check if there is a saved game state
        savedLines = SaveLoad.LoadGameStateSheet();
        if (savedLines == null)
        {
            //do nothing
            Debug.Log("no saved game");
        }
        else
        {
            Debug.Log("saved game found");
            Invoke("LoadPlayedSheet", 1);

        }
    }


    //line has been chosen, play it
    public void PlayRound()
    {

        //get the chosen line and do needed things in it
        Toggle chosenToggle = lineToggleGroup.ActiveToggles().FirstOrDefault();
        SingleLine sl = chosenToggle.GetComponent<SingleLine>();
        sl.PlayThis();

        clickBlocker.SetActive(true);

        GameManager.instance.playerInTurn.AddToPoints(sl.points); //send points to the players score

        //check the upper section if needed for the upper bonus
        if (sl.lineType.Contains("upper"))
        {
            upperPoints += sl.points;
            CheckUpperLine();
            upperBonusLine.UpperBonusUpdating(upperPoints);     //show upper points in sheet
        }
        //if line is first yatzy
        if (sl.lineType == "yatzy" && !yatzyPlayed)
        {

            if (currentLineIsYatzy)
            { //if yatzy has been played as zero, later yatzys wont give extra 50 points
                yatzyZeroPoints = false;
            }
            else
            {
                yatzyZeroPoints = true;
            }
            yatzyPlayed = true;
        }
        //if line is not yatzy, but dices are the same number and yatzy has not been played as 0. so give EXTRA POINTS!
        else if (yatzyPlayed && !yatzyZeroPoints && currentLineIsYatzy)
        {
            GameManager.instance.playerInTurn.AddToPoints(YatzyExtraPoints);
            //sl.extraPoints = YatzyExtraPoints;
            sl.ShowExtraPoints(YatzyExtraPoints);
            Debug.Log("EXTRA FIDDY");
        }
        ///////
        GameManager.instance.StartNextTurn();       //new round
    }


    public void CalculateLines(int[] line)
    {

        currentDices = line;
        LineCalculator.StartCalculating(sheetLines, currentDices);  // send the used sheet and the numbers to be calculated. the results are passed on to SingleLines of the sheetlines

        clickBlocker.SetActive(false);          //lines can be clicked
        if (!GameManager.instance.roundEnded && DiceParent.instance.throwsUsed != GameManager.instance.throwsPerRound)
        { //let player throw again once the dices have stopped moving, except when all throws have been used(round ended p much)
            //DiceParent.instance.ThrowButton.interactable = true;
            DiceParent.instance.throwBut.enabled = true;
            DiceParent.instance.throwButtonAnimator.Play("ColorEnabled");

        }
    }

    //check the points in upper section and if bonus can be given
    void CheckUpperLine()
    {
        //upperPoints += line.points;
        if (upperPoints >= 63 && upperBonusLine.hasBeenPlayed == false) //so that the bonus points are not given mopre than once
        {
            upperBonusLine.SetOtherPoints(true);
            upperBonusLine.PlayThis();
            GameManager.instance.playerInTurn.AddToPoints(upperBonusLine.points);
        }
    }
    //If a line is selected play button can be pressed

    public void IsItYatzy(bool isYatzy)
    {    //for checkign if current line is 5 same digits

        currentLineIsYatzy = isYatzy;

    }

    public void CheckPlayButton()
    { //should the play button be interactable (is some line chosen)
        if (lineToggleGroup.AnyTogglesOn() == true)
        {
            //playButton.interactable = true;
            playBut.enabled = true;
            playButtonAnimator.Play("ColorEnabled");
        }
        else
        {
            //playButton.interactable = false;
            playBut.enabled = false;
            playButtonAnimator.Play("ColorDisabled");
        }

    }

    //doesnt clear played points, but the toggle that has been clicked but not confirmed
    //used when throwing 
    public void ClearSheet()
    {
        lineToggleGroup.SetAllTogglesOff();

    }

    /*
     *Creating lines
     */
    void CreateSheet()
    {
        bool hasUpperBonus = false; //does the sheet contain line for upperbonus. if not, the game has as many rounds as the sheet has lines. if yes rounds = sheet length -1. checked after the for loop

        TextAsset textAsset = Resources.Load<TextAsset>(sheetVersion);
        string[] lineArray = textAsset.text.Split('\n');
        sheetLines = new SingleLine[lineArray.Length];
        lineObjects = new GameObject[lineArray.Length];
        for (int i = 0; i < lineArray.Length; i++)
        {

            string[] tempLine = lineArray[i].Split('-');        //splitting line from text asset to id name and score
            GameObject lineGo;

            //creating new line
            if (tempLine[2] == "upBonus")
            {
                lineGo = Instantiate(upperBonusLinePrefab);
            }
            else
            {
                lineGo = Instantiate(linePrefab);
            }
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

            if (sl.lineType == "upBonus")
            { //line calculating the bonus for the lines 1-6 is not handled where other lines are calculated (because it does not rely on the dices, but scores from the upper section)
                upperBonusLine = sl;
                hasUpperBonus = true;
            }
            if (sl.lineType == "yatzy")
            {  //if sheet has yatzy, it will have extra points for extra yatzys
                YatzyExtraPoints = sl.pointsDefault;

            }


        }
        //game has as many rounds as the sheet has lines, minus the upper bonus line
        if (hasUpperBonus)
        {
            GameManager.instance.roundsPerGame = sheetLines.Length - 1;
        }
        else
        {
            GameManager.instance.roundsPerGame = sheetLines.Length;
        }


    }

    public List<SavedSheetLine> SavePlayedSheet()
    {
        savedLines = new List<SavedSheetLine>();
        foreach (SingleLine sl in sheetLines)
        {
            SavedSheetLine lineToSave = new SavedSheetLine() { id = sl.id, lineName = sl.lineName, lineType = sl.lineType, pointsDefault = sl.pointsDefault, points = sl.points, extraPoints = sl.extraPoints, hasBeenPlayed = sl.hasBeenPlayed };
            savedLines.Add(lineToSave);
        }
        return savedLines;

    }

    void LoadPlayedSheet()
    {
        //getting data from saved sheet and assigning it to new sheet

        int playedRounds = 0;
        int pointsFromFile = 0;
        foreach (SavedSheetLine ssl in savedLines)
        {
            if (ssl.hasBeenPlayed)  //skip saved lines that have not been played
            {

                foreach (SingleLine sl in sheetLines)
                {
                    if (ssl.lineType == sl.lineType)    //matching saved lines with new lines
                    {
                        //setting points
                        sl.SetPointsFromFile(ssl.points, ssl.extraPoints);
                        pointsFromFile += ssl.points;
                        pointsFromFile += ssl.extraPoints;
                        if (sl.lineType.Contains("upper"))
                        {
                            upperPoints += ssl.points;
                        }
                        //updating rounds
                        if (sl.lineType != "upBonus")
                        {
                            playedRounds++;
                        }
                        //no need to foreach the rest of the lines in not saved sheetlines after a match has been found. Move on to next saved line
                        break;
                    }
                }
            }
        }
        //setting other info
        GameManager.instance.playerInTurn.AddToPoints(pointsFromFile);
        GameManager.instance.currentRound += playedRounds;
        CheckUpperLine();
        upperBonusLine.UpperBonusUpdating(upperPoints);

    }



    /*
     Resetting for new game
     */

    //delete and create new sheet
    public void ResetSheet()
    {
        upperPoints = 0;
        yatzyPlayed = false;
        yatzyZeroPoints = false;
        currentLineIsYatzy = false;
        //delete previous line objects before creating new ones
        foreach (GameObject go in lineObjects)
        {
            Destroy(go);
        }
        //create new sheet
        CreateSheet();
    }

}
