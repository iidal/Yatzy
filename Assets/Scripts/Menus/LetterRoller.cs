using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LetterRoller : MonoBehaviour
{
    string[] allChars;
    public int currentCharIndex = 0;
    int nextCharindex;
    int charCount;  //how many chars in array
    public TextMeshProUGUI letterBox;

    // Start is called before the first frame update
    void Start()
    {
        letterBox.text = allChars[0];
        currentCharIndex = 0;
    }

    // Update is called once per frame

    public void ChangeChar(string dir){
        if(dir == "up"){
            nextCharindex = currentCharIndex -1;
            if(nextCharindex < 0){
                nextCharindex = charCount-1;
            }

            
        }
        else if(dir == "down"){
            nextCharindex = currentCharIndex + 1;
            if(nextCharindex > charCount-1){
                nextCharindex = 0;
            }
        }
        else{
            Debug.Log("button dir bug");
        }
        
        currentCharIndex = nextCharindex;
            letterBox.text = allChars[currentCharIndex];

    }

    public void GetChars(string[] chars, int length){
        allChars = chars;
        charCount = length;

    }

    
}
