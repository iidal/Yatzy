using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
/*
for saving players result from solo game, icomparable so these can be sorted

 */

public class SavedResult: IComparable<SavedResult>
{
    public string playerName;
    public int result;

//public string playerName {get;set};
//public int result {get;set;}
    public SavedResult()
    {
       playerName = "";
         result = 0;

    }
    public int CompareTo(SavedResult other)
    {
        return result.CompareTo(other.result);
    }
}
