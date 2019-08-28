using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

  
    public int[] numbers = new int[5];
    public TextMeshProUGUI numberLine;

    private void Awake()
    {
        if (instance != null)
           Destroy(this);
        else
           instance = this;
    }


    void Start()
    {
        
    }


    


    public void GetNumbers(List<int> results) {

        for (int i = 0; i<results.Count; i++) {
            numbers[i] = results[i];
        }
        string s = "-";
        foreach (int i in numbers) {
            s += i.ToString()+ "-";
        }
        numberLine.text = s;
        SheetManager.instance.CalculateLines(numbers);
    }


}
