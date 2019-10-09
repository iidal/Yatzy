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
        List<SavedSheetLine>tempLines = SheetManager.instance.SavePlayedSheet();
        SerializableVector3[] tempPositions = DiceParent.instance.SaveDices("position");
        SerializableVector3[] tempRotations = DiceParent.instance.SaveDices("rotation");
        SaveLoad.SaveGameState(tempLines, DiceParent.instance.throwsUsed, tempPositions, tempRotations);
    }
    public void LoadGameState(){
        //loading the sheet is done in sheet manager no need to move it here atm
        //can switch between vector3 and the serializable one with the implicit operator thing

        SavedStateOther tempState = SaveLoad.LoadGameStateOther();
        if(tempState == null){
            return;
        }
        DiceParent.instance.throwsUsed = tempState.throwsUsed;
        DiceParent.instance.SetThrowsLeftText();
        foreach(Vector3 sv3 in tempState.dicePositions){
            Debug.Log(sv3.x + " " + sv3.y + " "+  sv3.z);
        }
        foreach(Vector3 sv3 in tempState.diceRotations){
            Debug.Log(sv3.x + " " + sv3.y + " "+  sv3.z);
        }
     
    

    }
}
