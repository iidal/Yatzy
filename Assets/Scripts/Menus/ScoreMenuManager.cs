using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreMenuManager : MonoBehaviour
{
    public TextMeshProUGUI scoreBoardText;
    string scoresText;
    List<SavedResult> resultList = new List<SavedResult>();

    void Awake()
    {
        LoadScoreBoard();
    }

    void LoadScoreBoard(){
        scoresText = "";
        scoreBoardText.text = scoresText;

        resultList = SaveLoad.Load();
        
        if(resultList!=null){
            foreach(SavedResult sr in resultList){
                scoresText += sr.playerName.ToString() + " - " + sr.result.ToString() + "\n";

            }
            scoreBoardText.text = scoresText;
        }
        else{
            Debug.Log("no scores");
        }
    }

}
