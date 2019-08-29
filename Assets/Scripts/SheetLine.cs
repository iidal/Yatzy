using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/SheetLine", order = 1)]
public class SheetLine : ScriptableObject
{
    public string lineName;
    public int points;
    public int[] possibleLines = new int[5];
}
