using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    int throwsPerRound = 3;
    int throwsUsed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeThrow() {
        if (throwsUsed < throwsPerRound) {
            DiceParent.instance.ThrowDices();

            throwsUsed++;
        }
        else {
            Debug.Log("three throws");
            throwsUsed = 0;
        }
    }
}
