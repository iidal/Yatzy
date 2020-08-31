using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LetterRoller : MonoBehaviour
{
    string[] allChars; // all possible chars, gotten from parent object/insertscoreandname.cs
    public int currentCharIndex = 0;    //what index from array is being chosen currently
    int nextCharindex;  //previous or next char
    int charCount;  //how many chars in array
    public TextMeshProUGUI letterBox;

    // Start is called before the first frame update
    void Start()
    {
        letterBox.text = allChars[0];   //shiow first char in array
        currentCharIndex = 0; //current is first index
    }


    public void ChangeChar(string dir){ //loop through characters, in both directions
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
            Debug.Log("button dir bug");    //buttons direction string is wrong
        }
        
        currentCharIndex = nextCharindex;
            letterBox.text = allChars[currentCharIndex];    //show the new char

    }

    public void GetChars(string[] chars, int length){   //get the chars from parent
        allChars = chars;
        charCount = length;

    }

    
}
