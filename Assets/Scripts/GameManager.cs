using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

  
    public int[] numbers = new int[5];

    public PlayerScript[] players = new PlayerScript[1];
    [HideInInspector]
    public PlayerScript playerInTurn;
    [HideInInspector]
    public int throwsUsedPerRound;  //easier access for this info here
    [HideInInspector]
    public int throwsPerRound = 3;

    public bool roundEnded;

    public TextMeshProUGUI throwsLeftText;

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
        throwsLeftText.text = "throws left: 3";
    }


    public void WhenThrowing(int throwsUsed) {
        int throwsLeft = throwsPerRound - throwsUsed;
        throwsLeftText.text = "throws left: " +throwsLeft.ToString();
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
        DiceParent.instance.StartNewRound();
        throwsUsedPerRound = 0;
        roundEnded = false;

        //showing that 3 throws left after round has been played, will be changed to something else in the future
        WhenThrowing(0);
    }


}
