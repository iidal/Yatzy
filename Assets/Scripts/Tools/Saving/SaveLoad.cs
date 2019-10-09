using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*

for saving game data, such as scores

 */
public static class SaveLoad
{
    
    public static List<SavedResult> results = new List<SavedResult>(); //scores for completed games

    //saving game state
    public static List<SavedSheetLine> savedLines = new List<SavedSheetLine>();
    public static SavedStateOther gameState; //throws used, position and rotation of dices
    

    #region saving and loading saved solo scores
    public static void SaveSoloResults(string name, int score)
    {
        SavedResult tempResult = new SavedResult() { playerName = name, result = score };
        results.Add(tempResult);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedResultsSolo.gd");
        bf.Serialize(file, SaveLoad.results);
        file.Close();

    }
    //loading data from file. rn only loads results from solo games
    public static List<SavedResult> LoadSoloScores()
    {
        if (File.Exists(Application.persistentDataPath + "/savedResultsSolo.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedResultsSolo.gd", FileMode.Open);
            SaveLoad.results = (List<SavedResult>)bf.Deserialize(file);
            file.Close();

            if (results.Count != 0)
            {   //sends saved results to where ever they are needed (scoreboard mostly), if nothing is saved yet return null
                return results;
            }
            else { return null; }
        }
        return null;
    }
    #endregion
    #region saving and loading the state of not completed solo game

    public static void SaveGameState(List<SavedSheetLine> lines, int throws, bool collected, SerializableVector3[] positions, SerializableVector3[] rotations)
    {
        savedLines.Clear();
        savedLines = lines;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameStateSheet.gd");
        bf.Serialize(file, SaveLoad.savedLines);
        file.Close();
		Debug.Log("sheet saved");

        SavedStateOther stateTemp = new SavedStateOther(){throwsUsed = throws, dicesCollected = collected, dicePositions = positions, diceRotations = rotations};
        gameState = stateTemp;
        bf = new BinaryFormatter();
        file = File.Create(Application.persistentDataPath + "/gameStateOther.gd");
        bf.Serialize(file, SaveLoad.gameState);
        file.Close();
		Debug.Log("state Saved");
    }
	public static List<SavedSheetLine> LoadGameStateSheet(){

		if (File.Exists(Application.persistentDataPath + "/gameStateSheet.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameStateSheet.gd", FileMode.Open);
            SaveLoad.savedLines = (List<SavedSheetLine>)bf.Deserialize(file);
            file.Close();
			
            return savedLines;
            
           
        }
        return null;
	}
    public static SavedStateOther LoadGameStateOther(){
        if (File.Exists(Application.persistentDataPath + "/gameStateOther.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameStateOther.gd", FileMode.Open);
            SaveLoad.gameState = (SavedStateOther)bf.Deserialize(file);
            file.Close();
			
            return gameState;
            
           
        }
        return null;
    }

    #endregion
    public static void DeleteFile(string toDelete)
    {
        if(toDelete=="gameState"){
            File.Delete(Application.persistentDataPath + "/gameStateSheet.gd");
            File.Delete(Application.persistentDataPath + "/gameStateOther.gd");
        }
    }
}