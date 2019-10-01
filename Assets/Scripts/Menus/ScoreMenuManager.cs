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
    public TextMeshProUGUI[] textBlocks;

    void Awake()
    {
        LoadScoreBoard();
    }

    void LoadScoreBoard(){


        resultList = SaveLoad.Load();
        

        

        //resultList = SortResults(resultList);

        int i = 0;
        if(resultList!=null){
            //resultList.Sort();
            resultList.Sort((x, y) => y.result.CompareTo(x.result));
            int resultsCount = resultList.Count;
            Debug.Log(resultsCount);
            foreach(TextMeshProUGUI t in textBlocks){
                if(i < resultsCount){
                    t.text = (i+1).ToString() + ". " +resultList[i].playerName.ToString() + " - " + resultList[i].result.ToString() + "\n";
                }
                else{
                    t.text = (i+1).ToString() + ".";
                }

                i++;
            }
    
        }
        else{
            Debug.Log("no scores");
        }
    }
    List <SavedResult> SortResults(List<SavedResult> res){
        List<SavedResult> temp = new List<SavedResult> ();

        // foreach(SavedResult sr in res){
        //     if(temp == null){   //adding the firts one
        //         temp.Add(sr);
        //     }
        //     else
        //     {
        //         for(int i = 0; i<res.Count; i++){
                   
        //         }
        //     }

        // }

        return temp;
    }
}
