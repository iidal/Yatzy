using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SavedStateOther
{
    public int throwsUsed;
    public bool dicesCollected; //if true, positions and rotations are not needed
    public SerializableVector3[] dicePositions;
    public SerializableVector3[] diceRotations;

    public SavedStateOther(){
        throwsUsed = 0;
        dicesCollected = false;
        dicePositions = new SerializableVector3[4];
        diceRotations = new SerializableVector3[4];
    }   
}
