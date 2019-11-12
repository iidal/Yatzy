using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Genereal stuff about the game handled here
/// </summary>


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int[] numbers = new int[5];  //numbers on dices stored here

    public PlayerScript[] players = new PlayerScript[1];    //all players (later should be instantiated from here)
    [HideInInspector]
    public PlayerScript playerInTurn;   //player who is playing now

    public int throwsPerRound = 3;

    public int roundsPerGame = 0;   //changed to the length of the line sheet that is used for the game
    public int currentRound = 1;   

    public bool roundEnded;     //true after a line has been played

    public GameObject GameDonePanel; // when game is finished show this, a new game can be started
    
    public int[] testnums;  //for testing when not getting results from dices but as input from inspector   (code for using them needs to be added again since for some reason commenting it out didnt seem to work....)


    private void Awake()
    {
        if (instance != null)
           Destroy(this);
        else
           instance = this;
    }


    void Start()
    {
        //while players are not instantiated from here and there is only one player, the player is accessed like this
        playerInTurn = players[0];
        Invoke("LoadGameState", 0.8f);
    }
  

    //storing current dice row here and passing them along to the sheet manager for checking lines etc
    public void GetNumbers(List<int> results) {
        for (int i = 0; i<results.Count; i++) {
            numbers[i] = results[i];
        }


        //sending the numbers for checking
        SheetManager.instance.CalculateLines(numbers);
    }

    //A line has been played, starting a new round
    public void StartNextTurn() {
        currentRound++;
        //check if we have played all the rounds of the game, if yes, end game, if no, continue normally
        if (currentRound > roundsPerGame) {
            EndGame();
            return;
        }

        DiceParent.instance.StartNewRound();     //set everything up for a new round with dices

        roundEnded = false;
    }

    void EndGame() {
        //show the panel for a finished game
        GameNotificationManager.instance.ShowNotification("SetPlayerNamePanel");
        SaveLoad.DeleteFile("gameState");
    }
    public void NewGame() {

        SheetManager.instance.ResetSheet(); //create a new sheet
        DiceParent.instance.OnNewGameStart();   //reset dices for new game
        playerInTurn.OnGameStart(); //resetting player (might be temporary)
        currentRound = 1;   //reset rounds
        GameDonePanel.SetActive(false); //hide game done panel
    }


    public void SaveGame(){
        //when leaving scene when game is not yet finished
    
        List<SavedSheetLine>tempLines = SheetManager.instance.SavePlayedSheet();    //SheetManager handles saving the sheet
        
        //gathering info that is going to be saved
        int throwsTemp = DiceParent.instance.throwsUsed;
        bool yatzyPlayedTemp = SheetManager.instance.yatzyPlayed;
        bool yatzyPlayedZeroTemp = SheetManager.instance.yatzyZeroPoints;
        bool collectedTemp = false; //if this is true, positions and rotation dont need to be used
        if(throwsTemp == 0) 
            collectedTemp = true;
        
        //getting dice positions and rotations from DiceParent that handles saving this data
        SerializableVector3[] tempPositions = DiceParent.instance.SaveDices("position");  
        SerializableVector3[] tempRotations = DiceParent.instance.SaveDices("rotation");

        SaveLoad.SaveGameState(tempLines, throwsTemp, yatzyPlayedTemp, yatzyPlayedZeroTemp, collectedTemp, tempPositions, tempRotations);
    }
    public void LoadGameState()
    {
        //loading the sheet is done in sheet manager no need to move it here atm
        //can switch between vector3 and the serializable one with the implicit operator thing

        SavedStateOther tempState = SaveLoad.LoadGameStateOther();
        if (tempState == null)
        {
            return;
        }
        //if a saved game has been found, proceed:
        DiceParent.instance.throwsUsed = tempState.throwsUsed;
        DiceParent.instance.SetThrowsLeftText();
        SheetManager.instance.yatzyZeroPoints = tempState.yatzyPlayedZero;
        SheetManager.instance.yatzyPlayed = tempState.yatzyPlayed;

        if (!tempState.dicesCollected){//if the dices are collected, their positions and rotations dont need to be updated
    
            StartCoroutine(DiceParent.instance.DicesLoaded(tempState.dicePositions, tempState.diceRotations));
            
        }
        if(tempState.throwsUsed == throwsPerRound){ //if all throws have been used dont set throw button interactable
            //DiceParent.instance.ThrowButton.interactable = false;
            DiceParent.instance.throwBut.enabled = false;
           
        }
     
    

    }
}
