using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    int points = 0;
   // int throwsPerRound;
    //int throwsUsed = 0;

    public List<string> linesPlayed = new List<string>();

    public TextMeshProUGUI playersPointsText; 


    // Start is called before the first frame update
    void Start()
    {
        OnGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToPoints(int result) {

        points += result;
      //  throwsUsed = 0;
        playersPointsText.text = points.ToString();
    }


    public void MakeThrow() {

        DiceParent.instance.ThrowDices();
        //if (throwsUsed < throwsPerRound) {
        //    DiceParent.instance.ThrowDices();
        //    throwsUsed++;
        //    GameManager.instance.WhenThrowing(throwsUsed);
        //}
        //if (throwsUsed >= throwsPerRound ) {
        //    GameManager.instance.WaitingForNextTurn();
        //}
    }

    public void OnGameStart() {

      //  throwsPerRound = GameManager.instance.throwsPerRound;

        points = 0;

        playersPointsText.text = points.ToString();
    }
}
