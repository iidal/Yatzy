using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SheetManager : MonoBehaviour
{
    public static SheetManager instance;
    public TextMeshProUGUI[] upperLines;
    public int[] currentDices = new int [5];


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    public void CalculateLines(int[]line) {
        

        Debug.Log("calculate lines");

        currentDices = line;
        CheckUpperLines();
   
    }
    void CheckUpperLines() {
        int count = 1;
        foreach (TextMeshProUGUI t in upperLines) {
            int amount = 0;
            foreach (int i in currentDices) {
                if (i == count){
                    amount += i;
                }
            }
            t.text = amount.ToString(); 
            count++;
            
        }
    }
}
