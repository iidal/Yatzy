using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (other.tag == "Floor")
        {
            sideOnFloor = false;
        }
    }
}
