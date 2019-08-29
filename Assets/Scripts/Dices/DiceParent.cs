﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceParent : MonoBehaviour
{
    public static DiceParent instance; 
    public GameObject[] diceObjects;
    public GameObject dicePrefab;

    Rigidbody[] rbs;
    DiceManager[] diceGMs;
    public Button[] diceButtons;
    public Transform[] throwPositions = new Transform[2];

    public bool dicesThrown = false;
    public Dictionary<int, int> results = new Dictionary<int, int>();
    //List<int> results = new List<int>();
    public int newResults = 0; //new results from not locked dices

    int diceAmount = 5;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }
    private void Start()
    {
        CreateDices();
    }
    void CreateDices()
    {
        diceObjects = new GameObject[diceAmount];
        rbs = new Rigidbody[diceAmount];
        diceGMs = new DiceManager[diceAmount];

        for (int i = 0; i < diceAmount; i++)
        {
            GameObject go = Instantiate(dicePrefab, transform.position, Quaternion.identity);
            diceObjects[i] = go;
            rbs[i] = go.GetComponent<Rigidbody>();
            diceGMs[i]= go.GetComponent<DiceManager>();
            diceGMs[i].id = i;
            diceGMs[i].diceButton = diceButtons[i];
            go.transform.parent = transform;
            go.SetActive(false);
        }


    }

    public void ThrowDices() {
        StartCoroutine("Throw", 1);
    }
    public IEnumerator Throw(int i) {
        int dir;
        if (i == 0)
            dir = -1;
        else
            dir = 1;


        int index = 0;
        Vector3 throwForce;
        Vector3 throwTorque;
        Vector3 throwPos;

        foreach (GameObject go in diceObjects) {
            if (go.GetComponent<DiceManager>().isLocked == false ) {
                go.SetActive(true);
               
                throwPos = throwPositions[i].transform.position;
                throwPos.z += (index*1.2f);
                go.transform.position = throwPos;
                
                
                rbs[index].angularVelocity = Vector3.zero;
                rbs[index].velocity = Vector3.zero;
                yield return new WaitForEndOfFrame();
                go.transform.rotation = throwPositions[i].rotation;
                throwForce = new Vector3(Random.Range(15,30)*dir,0,0);

                throwTorque = new Vector3(Random.Range(-360, 360), Random.Range(-360,360), Random.Range(-360,360));

                
                rbs[index].AddForce(throwForce, ForceMode.Impulse);
               rbs[index].AddTorque(throwTorque, ForceMode.Impulse);
               


                yield return new WaitForSeconds(0.05f);
            }
            index++;
        }
        dicesThrown = true;

        foreach (DiceManager dm in diceGMs) {
            dm.diceStopped = false;
        }
        
    }

    public void GetResults(int result, int id) {
        int value;
        newResults++;
        if (results.TryGetValue(id, out value))
        {
            results[id] = result;
        }
        else {
            results.Add(id,result);
        }


        if (newResults == 5) {
            List<int> temp = new List<int>();
            foreach (KeyValuePair<int,int> kvp in results) {
                temp.Add(kvp.Value);
            }
            GameManager.instance.GetNumbers(temp);
            results.Clear();
            newResults = 0;
        }
        
    }

}