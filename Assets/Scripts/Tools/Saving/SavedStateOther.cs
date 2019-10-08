using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SavedStateOther
{
    public int throwsUsed;
    public SerializableVector3[] dicePositions;
    public SerializableVector3[] diceRotations;

    public SavedStateOther(){
        throwsUsed = 0;
        dicePositions = new SerializableVector3[4];
        diceRotations = new SerializableVector3[4];
    }   
}
