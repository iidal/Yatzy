using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

//////handles one dice

public class DiceManager : MonoBehaviour
{
    Rigidbody rb;
    Renderer rend;

    public int id;
    public int result;

    public bool isLocked = false; //is the dice locked from throwing
    public bool diceStopped = false;

    public DiceCollider[] sides = new DiceCollider[6];  // DiceCollider checks if the side is touching floor

    public Material diceMat;    // normal material for dice
    public Material lockedMat;  //material when dice is locked (copy of the normal material but with a different color)

    public Button diceButton;   //button from where the dice can be locked as well
    TextMeshProUGUI textTemp;   // shows the result, might be changed to a sprite that looks like the dices material

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        textTemp = diceButton.GetComponentInChildren<TextMeshProUGUI>();
        diceButton.onClick.AddListener(ToggleLocked);
        diceButton.interactable= false;
    }

    private void Update()
    {

        if (rb.velocity.x == 0 && rb.velocity.y == 0 && rb.velocity.z == 0)
        {//when dice has stopped the result can be checked

            if (!diceStopped && DiceParent.instance.dicesThrown == true)
            {
                //dicestopped and dicesthrown makes sure GetResult is not called multiple times

                diceStopped = true;

                GetResult();

                //if dices start landing with an edge on floor(not on side) check that atleast one rotation axis is zero
            }
        }



    }
     void GetResult() {
        //get result from the dicecollider, only one has (should have) sideonfloor == true
        //sideonfloors opposite side is the result
        foreach (DiceCollider dc in sides) {
            if (dc.sideOnFloor) {
                string diceSide = dc.name;
                switch (diceSide) {
                    case "side1":
                        result = 6;
                        break;
                    case "side2":
                        result = 5;
                        break;
                    case "side3":
                        result = 4;
                        break;
                    case "side4":
                        result = 3;
                        break;
                    case "side5":
                        result = 2;
                        break;
                    case "side6":
                        result = 1;
                        break;
                    default:
                        Debug.Log("something is wrong with the dice");
                        break;
                }
                break;
            }
        }

        if(result == 0){
            Debug.Log("fuk u dice");
            Vector3 force = new Vector3(5,5,0);
            rb.AddForce(force, ForceMode.Impulse);
            diceStopped = false;
            return;
        }


        textTemp.text = result.ToString();  // show result on button
        DiceParent.instance.GetResults(result, id); //send result forward
       
    }

    //When clicking on dice
    private void OnMouseDown()
    {
        Debug.Log("dice clicked");

        if (diceButton.interactable) {
            ToggleLocked();
        }
    }

    //Toggle dice's lock state
    public void ToggleLocked() {
        if (isLocked)
            isLocked = false;
        else
            isLocked = true;


        if (isLocked)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;    //´cant be moved
            rend.material = lockedMat;  //change material
            diceButton.GetComponent<Image>().color = Color.yellow;  //change buttons color 

        }
        else
        {
            //reset
            rb.constraints = RigidbodyConstraints.None;
            rend.material = diceMat;
            diceButton.GetComponent<Image>().color = Color.white;
        }
    }

    
}
