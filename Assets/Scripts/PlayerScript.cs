using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// At the moment a bit useless, but once there are more players than one and they are instantiated from game manager
/// </summary>
public class PlayerScript : MonoBehaviour
{
    int points = 0; //amount of points player has collected
    public TextMeshProUGUI playersPointsText; //show points in UI
    //public List<string> linesPlayed = new List<string>();   //storing what lines have been played, not used correctly

    

    void Start()
    {
        //setting all up for the game
        OnGameStart();
    }

    public void AddToPoints(int result) {
        //updating played points to here
        points += result;
        playersPointsText.text = points.ToString();
    }

    public void MakeThrow() {
        //player making a throw, might be moved elsewhere, for example throwing is called from the button no matter whose turn it is
        DiceParent.instance.ThrowDices();
    }

    public void OnGameStart() {
        //setting all up for the game
        points = 0;
        playersPointsText.text = points.ToString();
    }
}
