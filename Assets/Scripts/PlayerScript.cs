using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    int points = 0;
    int throwsPerRound = 3;
    int throwsUsed = 0;

    public List<string> linesPlayed = new List<string>();

    public TextMeshProUGUI playersPointsText;


    // Start is called before the first frame update
    void Start()
    {
        playersPointsText.text = points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToPoints(int result) {

        points += result;
        throwsUsed = 0;
        playersPointsText.text = points.ToString();
    }


    public void MakeThrow() {
        if (throwsUsed < throwsPerRound) {
            DiceParent.instance.ThrowDices();

            throwsUsed++;
        }
        else {
            Debug.Log("three throws");
            GameManager.instance.WaitingForNextTurn();
        }
    }
}
