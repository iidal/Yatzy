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
    }


}
