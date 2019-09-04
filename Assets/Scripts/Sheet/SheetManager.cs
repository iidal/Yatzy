using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SheetManager : MonoBehaviour
{

    public static SheetManager instance;
    public Button playButton;


    public int[] currentDices = new int [5];

    public SingleLine[] sheetLines;
    public GameObject[] lineObjects;
    public GameObject linePrefab;
    public GameObject lineParent; //holds the lines

    public ToggleGroup lineToggleGroup;

    public GameObject clickBlocker;

    int upperPoints = 0;
    SingleLine upperBonusLine;

    //if needed
    Dictionary<int, string> lineNames = new Dictionary<int, string>();
    Dictionary<int, int> lineScores = new Dictionary<int, int>();
    Dictionary<int, bool> playedLines = new Dictionary<int, bool>();


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



    public void PlayRound() {

        Toggle chosenToggle = lineToggleGroup.ActiveToggles().FirstOrDefault();
        SingleLine sl = chosenToggle.GetComponent<SingleLine>();
        sl.PlayThis();

        GameManager.instance.playerInTurn.AddToPoints(sl.points);
        GameManager.instance.StartNextTurn();

        clickBlocker.SetActive(true);

        if (sl.lineType.Contains("upper")) {
            upperPoints += sl.points;
            if (upperPoints >= 63) {
                Debug.Log("63");
                upperBonusLine.SetOtherPoints(true);
                upperBonusLine.PlayThis();
            }
        }

    }


    public void CalculateLines(int[] line) {

        currentDices = line;
        LineCalculator.StartCalculating(sheetLines, currentDices);

        clickBlocker.SetActive(false);          //lines can be clicked
        if (!GameManager.instance.roundEnded) { //let player throw again once the dices have stopped moving, except when all throws have been used(round ended p much)
            DiceParent.instance.ThrowButton.interactable = true;
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
