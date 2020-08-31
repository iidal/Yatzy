using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// each side of dice has its own dicecollider
/// only one has (should have) sideonfloor = true, that side is on the floor and checked in DiceManager
/// </summary>

public class DiceCollider : MonoBehaviour
{

    public bool sideOnFloor = false;    

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Floor") {
            sideOnFloor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Floor") {
            sideOnFloor = false;
        }
    }
}
