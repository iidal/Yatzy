using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the dices, throwing, etc
/// </summary>

public class DiceParent : MonoBehaviour
{
    public static DiceParent instance; 
    public GameObject[] diceObjects;    //dices as game object
    public GameObject dicePrefab;
    Rigidbody[] rbs;
    DiceManager[] diceGMs;  //individual dice managers, same order as in all of the arrays with components for the dices
    public Button[] diceButtons;
    public Transform[] throwPositions = new Transform[2];   // positions on each side where the dices are thrown from (one for each player)

    public Button ThrowButton;
    public TextMeshProUGUI throwsLeftText;  //temporary maybe, shows how many throws left
    int throwsPerRound; //is gotten from game manager
    public int throwsUsed = 0;

    int diceAmount = 5; // dices in game

    public bool dicesThrown = false; //true when dices get thrown, checking the states of the dices

    public Dictionary<int, int> results = new Dictionary<int, int>();   //id of the dice, score
    public int newResults = 0; //for checking if result has come in from all dices (might be a little obsolete)

    

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
        //references
        diceObjects = new GameObject[diceAmount];
        rbs = new Rigidbody[diceAmount];
        diceGMs = new DiceManager[diceAmount];

        //creation and setting up needed things
        for (int i = 0; i < diceAmount; i++)
        {
            GameObject go = Instantiate(dicePrefab, transform.position, Quaternion.identity);
            diceObjects[i] = go;
            rbs[i] = go.GetComponent<Rigidbody>();
            diceGMs[i]= go.GetComponent<DiceManager>();
            diceGMs[i].id = i;
            diceGMs[i].diceButton = diceButtons[i];
            go.transform.parent = transform; //parenting
            go.SetActive(false);    //hiding the dice
        }


    }
    #endregion

    #region Throwing
    public void ThrowDices() {

        //can throw if throws are left to use
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
                //throw
                StartCoroutine("Throw", 1); //parameter 0 or 1, which side to throw from (player 1 or 2)
            }
            else
            {
                //tell player they cant throw when all dices are locked
                GameNotificationManager.instance.ShowNotification("allDicesLockedCantThrow");
            }
        }
        else {//still keeping just in case for bugs
            Debug.Log("obsolete code, will not come here. hopefully");
        }

        
    }
    public IEnumerator Throw(int i) {

        ThrowButton.interactable = false;
        SheetManager.instance.clickBlocker.SetActive(true); //sheet cant be touched when dices are moving

        //adjusting direction where to throw from throw pos. minus flips the direction so throws go to the bowl
        int dir;
        if (i == 0)
            dir = -1;
        else
            dir = 1;


        int index = 0; //index for looping through all the needed arrays referencing to dice components
        //variables for adjusting throw
        Vector3 throwForce;
        Vector3 throwTorque;
        Vector3 throwPos;

        foreach (GameObject go in diceObjects) {

            //ACTUALLY THROWING INSIDE THIS IF
            //only throw the dice if it has not been locked
            if (go.GetComponent<DiceManager>().isLocked == false ) {
                go.SetActive(true);
               
                //where to throw from and moving dice to that position
                throwPos = throwPositions[i].transform.position;
                throwPos.z += (index*1.2f);
                go.transform.position = throwPos;
                
                //add forces
                rbs[index].angularVelocity = Vector3.zero;
                rbs[index].velocity = Vector3.zero;
                yield return new WaitForEndOfFrame(); //just in case, remains from fixing buggy dice behaviour 
                go.transform.rotation = throwPositions[i].rotation;
                throwForce = new Vector3(Random.Range(15,30)*dir,0,0);
                throwTorque = new Vector3(Random.Range(-360, 360), Random.Range(-360,360), Random.Range(-360,360));
                rbs[index].AddForce(throwForce, ForceMode.Impulse);
                rbs[index].AddTorque(throwTorque, ForceMode.Impulse);

                diceButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = ""; //reset the dice's button
                diceGMs[index].result = 0;  //reset the dice's score
            }


            diceButtons[index].interactable = false; //buttons cant be clicked when moving 
            index++;
        }

        yield return new WaitForSeconds(0.25f); // little wait so the dices dont get checked for a score too soon (when dices stop they are checked for a score, this way they are not checked when they are about to be thrown)
        dicesThrown = true;
    
        foreach (DiceManager dm in diceGMs) {
            dm.diceStopped = false;
        }

        SheetManager.instance.ClearSheet(); //clear last rounds calculations

        throwsUsed++;
        // int throwsLeft = throwsPerRound - throwsUsed;
        // throwsLeftText.text = "throws left: " +throwsLeft.ToString();
        SetThrowsLeftText();
        if (throwsUsed == throwsPerRound) {         //if all throws used, round is ending
            RoundEnded();  
            GameManager.instance.roundEnded = true;
        }
        
    }
    public void SetThrowsLeftText(){
        int throwsLeft = throwsPerRound - throwsUsed;
        throwsLeftText.text = "throws left: " +throwsLeft.ToString();
    }

    #endregion

    //results from dices, not the played points
    public void GetResults(int result, int id) {

        //getting the new results, sending them along when results have been gotten from each dice (id might be useless at this point, as well as counting new values, but better safe than sorry) 

        if (results.TryGetValue(id, out int value))
        {
            results[id] = result;
        }
        else
        {
            newResults++;
            results.Add(id, result);
        }


        if (newResults == 5) {  //when results have been gotten from all dices
            List<int> temp = new List<int>();
            foreach (KeyValuePair<int,int> kvp in results) {
                temp.Add(kvp.Value);
            }
            GameManager.instance.GetNumbers(temp);  //send results as list forward
            results.Clear();
            newResults = 0;
            dicesThrown = false;
            foreach (Button b in diceButtons) {
                b.interactable = true;  //dices can be locked/chosen
            }
        }
        
    }

    public void RoundEnded() {  //ie all throws have been used
        ThrowButton.interactable = false;
        
    }
    public void StartNewRound() {
        StartCoroutine("HideDices"); // after round has been played dices are hidden (set active when throwing)
        throwsUsed = 0;
        throwsLeftText.text = "throws left: " + throwsPerRound.ToString();
        ThrowButton.interactable = true; // can throw again
        foreach (DiceManager dm in diceGMs) {
            if (dm.isLocked)    // unlock locked dices 
            {
                dm.ToggleLocked();
            }   
            
        }
        foreach (Button db in diceButtons) {
            db.GetComponentInChildren<TextMeshProUGUI>().text = "";
            db.interactable = false;    //set interactable after throwing and dices have stopped
        }
    }
    IEnumerator HideDices() {


        foreach (GameObject dice in diceObjects)
        {
            //dices are lifted up from the floor so the colliders checking which side is touching the floor gets resetted. if the colliders are not resetted the dice will start to show wrong results
            dice.transform.position = new Vector3(dice.transform.position.x, dice.transform.position.y +0.7f, dice.transform.position.z);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject dice in diceObjects)
        {
            dice.SetActive(false); //this could be done with and animation in the future (shrinking maybe)
        }
    }

    public void OnNewGameStart() {  //what needs to be done when a new game is starting

        
        StartNewRound();
    }

    public SerializableVector3[] SaveDices(string posOrRot){
        //saving the rotations and positions of the the dices at the time of saving, so they can be showed and gotten results from the same way as they were the first time

        SerializableVector3[] tempArray = new SerializableVector3[diceObjects.Length];
        int i =0;
        if (posOrRot == "position")
        {
            
            foreach (GameObject go in diceObjects)
            {
                Vector3 tempVec = new Vector3(go.transform.position.x, go.transform.position.y+1, go.transform.position.z);
                tempArray[i] = tempVec;
                i++;
            }
            return tempArray;
        }
        else if (posOrRot == "rotation")
        {

            foreach (GameObject go in diceObjects)
            {
                tempArray[i] = go.transform.rotation.eulerAngles;
                i++;
            }
            return tempArray;
        }
        else
        {
            Debug.Log("whoops something went wrong with the posOrRot string");
            return null;
        }
    }
    public IEnumerator DicesLoaded(SerializableVector3[] loadedPos, SerializableVector3[] loadedRot){
        //get saved rotations and positions and assign them to dices

        int i = 0;
        foreach(GameObject go in diceObjects){
            go.SetActive(true);
            go.transform.position = loadedPos[i];
            Quaternion tempQ = Quaternion.Euler(loadedRot[i]);
            go.transform.rotation = tempQ;
            i++;
        }
        yield return new WaitForSeconds(0.2f);
        dicesThrown = true;


    }
}
