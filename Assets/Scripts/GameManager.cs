using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

  
    public int[] numbers = new int[5];  //numbers on dices

    public PlayerScript[] players = new PlayerScript[1];    //all players
    [HideInInspector]
    public PlayerScript playerInTurn;   //player who is playing now
    //[HideInInspector]
   // public int throwsUsedPerRound;  //easier access for this info here
    [HideInInspector]
    public int throwsPerRound = 3;

    public int roundsPerGame = 0;
    int currentRound = 1;

    public bool roundEnded;

    //UI
   // public TextMeshProUGUI throwsLeftText;

    public GameObject GameDonePanel;

    private void Awake()
    {
        if (instance != null)
           Destroy(this);
        else
           instance = this;
    }


    void Start()
    {
        playerInTurn = players[0];
       // throwsLeftText.text = "throws left: 3";

    }


    public void WhenThrowing(int throwsUsed) {
        int throwsLeft = throwsPerRound - throwsUsed;
        //throwsLeftText.text = "throws left: " +throwsLeft.ToString();
    }


    public void GetNumbers(List<int> results) {
        

        for (int i = 0; i<results.Count; i++) {
            numbers[i] = results[i];
        }
        SheetManager.instance.CalculateLines(numbers);
    }

    public void WaitingForNextTurn() {
        DiceParent.instance.RoundEnded();
        roundEnded = true;
    }
    public void StartNextTurn() {
        currentRound++;
        //check if we have played all the rounds of the game, if yes, end game, if no, continue normally
        if (currentRound > roundsPerGame) {
            EndGame();
            return;
        }

        DiceParent.instance.StartNewRound();
       // throwsUsedPerRound = 0;
        roundEnded = false;

        //showing that 3 throws left after round has been played, will be changed to something else in the future
        WhenThrowing(0);
    }

    void EndGame() {
        Debug.Log("game end");
        GameDonePanel.SetActive(true);
    }
    public void NewGame() {

        SheetManager.instance.ResetSheet();
        DiceParent.instance.OnNewGameStart();
        playerInTurn.OnGameStart();
        currentRound = 1;
        GameDonePanel.SetActive(false);
    }


}
