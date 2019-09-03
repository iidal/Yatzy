using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{
    Rigidbody rb;
    Renderer rend;

    public int id;
    public int result;

    public bool isLocked = false; //is the dice locked from throwing
    public bool diceStopped = false;

    public DiceCollider[] sides = new DiceCollider[6];

    public Material diceMat;
    public Material lockedMat;

    public Button diceButton;
    TextMeshProUGUI textTemp;

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
        {

            //if (DiceParent.instance.allDicesStopped)
            //{
            //    diceButton.interactable = true;
            //}
            if (!diceStopped && DiceParent.instance.dicesThrown == true)
            {
                //if dices start landing with an edge on floor(not on side) check that atleast one rotation axis is zero
                //Debug.Log("dicestopped");
                diceStopped = true;
                
                GetResult();


            }
        }
        else {
            //diceStopped = false;
            //diceButton.interactable = false;
        }


    }
    void GetResult() {
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
                        //NudgeDice();
                        break;
                }
                break;
            }
        }
        textTemp.text = result.ToString();

       // diceButton.interactable = true;
        DiceParent.instance.GetResults(result, id);
    }

    //private void NudgeDice() {
    //    Vector3 nudge = new Vector3(1,0,0);
    //    rb.AddForce(nudge, ForceMode.Impulse);
    //}

    //When clicking on dice
    private void OnMouseDown()
    {
        if (diceButton.interactable) {
            ToggleLocked();
        }
    }

    //Toggle dices lock state
    public void ToggleLocked() {
        if (isLocked)
            isLocked = false;
        else
            isLocked = true;


        if (isLocked)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rend.material = lockedMat;
            diceButton.GetComponent<Image>().color = Color.yellow;

        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rend.material = diceMat;
            diceButton.GetComponent<Image>().color = Color.white;
        }
    }

    
}
