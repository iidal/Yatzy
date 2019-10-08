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

    public static List<SavedSheetLine> savedLines = new List<SavedSheetLine>();

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

    public static void SaveGameState(List<SavedSheetLine> lines)
    {
        savedLines.Clear();
        savedLines = lines;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameState.gd");
        bf.Serialize(file, SaveLoad.savedLines);
        file.Close();
		Debug.Log("sheet saved");
    }
	public static List<SavedSheetLine> LoadGameState(){

		if (File.Exists(Application.persistentDataPath + "/gameState.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameState.gd", FileMode.Open);
            SaveLoad.savedLines = (List<SavedSheetLine>)bf.Deserialize(file);
            file.Close();
			
            return savedLines;
            
           
        }
        return null;
	}

    #endregion
    public static void DeleteFile(string fileName)
    {
        try{
        File.Delete(Application.persistentDataPath + "/"+fileName + ".gd");
        }
        catch{
            Debug.Log("deleting a file failed");
        }
    }
}