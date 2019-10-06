using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
for showing top 10 scores
 */
public class ScoreMenuManager : MonoBehaviour
{
    
   // string scoresText;
    List<SavedResult> resultList = new List<SavedResult>(); //loaded results go here
    public TextMeshProUGUI[] textBlocks;

    void Awake()
    {
        LoadScoreBoard();
    }

    void LoadScoreBoard(){


        resultList = SaveLoad.Load();
        

        //goes through the saved results (if there are any). sorts and puts them into a presentable form

        int i = 0;
        if(resultList!=null){
        
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
}
