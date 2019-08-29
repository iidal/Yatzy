using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class SheetManager : MonoBehaviour
{
    public static SheetManager instance;
    public TextMeshProUGUI[] upperLines;
    public int[] currentDices = new int [5];
    public ToggleGroup lineToggles;
    public Button playButton;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }
    private void Start()
    {
        playButton.interactable = false;
    }

    public void PlayRound() {
        Toggle temp = lineToggles.ActiveToggles().FirstOrDefault();
        if (temp == null) {
            Debug.Log("null");
        }
        else
        Debug.Log(temp.GetComponentInChildren<TextMeshProUGUI>().text);
      
    }

    //If a line is selected play button can be pressed
    public void CheckPlayButton() {
        if (lineToggles.AnyTogglesOn() == true)
            playButton.interactable = true;
        else
            playButton.interactable = false;

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
            amount = 0;
        }
    }
}
