using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SavedSheetLine
{
    public int id;  
    public string lineName;
    public string lineType; //upper section, a straight, pair....
    public int pointsDefault; //if zero, points are calculated from dices
    public int points;   
    public int extraPoints; // ie for multiyatzys
    public bool hasBeenPlayed;  //has the line been played

    public SavedSheetLine(){
        id = 0;
        lineName = "";
        lineType = "";
        pointsDefault = 0;
        points = 0;
        extraPoints = 0;
        hasBeenPlayed = false;

    }

}
