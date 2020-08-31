using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionsManager : MonoBehaviour
{
    string[] infoSnippets;
    int snippetCount = 0;
    int nextSlideIndex = 0;
    int currentSlideIndex = 0;
    public TextMeshProUGUI infoBoxText;

    void Start()
    {
        LoadTextAsset();

    }


    public void ChangeSlide(string dir)
    { //loop through characters, in both directions
        if (dir == "left")
        {
            nextSlideIndex = currentSlideIndex - 1;
            if (nextSlideIndex < 0)
            {
                nextSlideIndex = snippetCount - 1;
            }


        }
        else if (dir == "right")
        {
            nextSlideIndex = currentSlideIndex + 1;
            if (nextSlideIndex > snippetCount - 1)
            {
                nextSlideIndex = 0;
            }
        }
        else
        {
            Debug.Log("button dir bug");    //buttons direction string is wrong
        }

        currentSlideIndex = nextSlideIndex;
        infoBoxText.text = infoSnippets[currentSlideIndex];    //show the new char

    }


    void LoadTextAsset()
    {
        Debug.Log("hellooo");
        TextAsset textAsset = Resources.Load<TextAsset>("YatzyHowToPlay");
        infoSnippets = textAsset.text.Split('/');
        snippetCount = infoSnippets.Length;
        infoBoxText.text = infoSnippets[0];
        currentSlideIndex = 0;
    }
}
