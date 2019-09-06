using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceParent : MonoBehaviour
{
    public static DiceParent instance; 
    public GameObject[] diceObjects;
    public GameObject dicePrefab;

    Rigidbody[] rbs;
    DiceManager[] diceGMs;
    public Button[] diceButtons;
    public Transform[] throwPositions = new Transform[2];

    public Button ThrowButton;
    public TextMeshProUGUI throwsLeftText;
    int throwsPerRound;
    int throwsUsed = 0;

    public bool dicesThrown = false;


    public Dictionary<int, int> results = new Dictionary<int, int>();
    public int newResults = 0; //new results from not locked dices

    int diceAmount = 5;

    #region Game start
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
        throwsPerRound = GameManager.instance.throwsPerRound;
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
    #endregion

    #region Throwing
    public void ThrowDices() {

        if (throwsUsed < throwsPerRound)
        {
            //check if all dices are locked, if yes, cant throw
            bool allLocked = true;
            foreach (DiceManager dm in diceGMs)
            {
                if (!dm.isLocked)
                {
                    allLocked = false;
                }
            }
            if (!allLocked)
            {
                StartCoroutine("Throw", 1);
            }
            else
            {
               // ThrowButton.interactable = true;
                Debug.Log("cant throw");
                GameNotificationManager.instance.ShowNotification("allDicesLockedCantThrow");
            }
        }
        else {
            Debug.Log("obsolete code, will not come here. hopefully");
        }

        
    }
    public IEnumerator Throw(int i) {

        ThrowButton.interactable = false;
        SheetManager.instance.clickBlocker.SetActive(true);
        //adjusting direction where to throw from throw pos
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
            //only throw if dice has not been locked
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

                diceButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = "";
                go.GetComponent<DiceManager>().result = 0;
            }


            diceButtons[index].interactable = false; //cant be clicked when moving 
            index++;
        }

        yield return new WaitForSeconds(0.25f);
        dicesThrown = true;
    
        foreach (DiceManager dm in diceGMs) {
            dm.diceStopped = false;
        }
        SheetManager.instance.ClearSheet();
        throwsUsed++;
        int throwsLeft = throwsPerRound - throwsUsed;
        throwsLeftText.text = "throws left: " +throwsLeft.ToString();
        if (throwsUsed == throwsPerRound) {
            ThrowButton.interactable = false;
        }
        
    }
    #endregion

    //results from dices, not the played points
    public void GetResults(int result, int id) {
        newResults++;
    
        if (results.TryGetValue(id, out int value))
        {
            results[id] = result;
        }
        else
        {
            results.Add(id, result);
        }


        if (newResults == 5) {
            List<int> temp = new List<int>();
            foreach (KeyValuePair<int,int> kvp in results) {
                temp.Add(kvp.Value);
            }
            GameManager.instance.GetNumbers(temp);
            results.Clear();
            newResults = 0;
            dicesThrown = false;
            foreach (Button b in diceButtons) {
                b.interactable = true;
            }
        }
        
    }

    public void RoundEnded() {
        ThrowButton.interactable = false;
        
    }
    public void StartNewRound() {
        StartCoroutine("HideDices");
        throwsUsed = 0;
        throwsLeftText.text = "throws left: " + throwsPerRound.ToString();
        ThrowButton.interactable = true;
        foreach (DiceManager dm in diceGMs) {
            if (dm.isLocked)
            {
                dm.ToggleLocked();
            }
            
        }
        foreach (Button db in diceButtons) {
            db.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
    IEnumerator HideDices() {


        foreach (GameObject dice in diceObjects)
        {

            dice.transform.position = new Vector3(dice.transform.position.x, dice.transform.position.y +0.5f, dice.transform.position.z);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject dice in diceObjects)
        {
            dice.SetActive(false); //this could be done with and animation in the future (shrinking maybe)
        }

        yield return null;
    }

    public void OnNewGameStart() {

        
        StartNewRound();
       // HideDices();
    }
}
