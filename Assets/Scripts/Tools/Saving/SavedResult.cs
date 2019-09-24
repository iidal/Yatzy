using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class SavedResult
{
    public string playerName;
    public int result;

    public SavedResult(){
        playerName = "";
        result = 0;

    }
}
