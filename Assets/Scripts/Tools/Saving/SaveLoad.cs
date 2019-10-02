using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static List<SavedResult> results = new List<SavedResult>(); //scores for completed games
	//public static List<GameData> gameData = new List <GameData> ();

	public static void SaveSoloResults(string name, int score){
		SavedResult tempResult = new SavedResult (){playerName =name, result = score };
 		results.Add(tempResult);
    	BinaryFormatter bf = new BinaryFormatter();
    	FileStream file = File.Create (Application.persistentDataPath + "/savedResultsSolo.gd");
    	bf.Serialize(file, SaveLoad.results);
    	file.Close();

	}
	public static List<SavedResult> Load() {
    if(File.Exists(Application.persistentDataPath + "/savedResultsSolo.gd")) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/savedResultsSolo.gd", FileMode.Open);
        SaveLoad.results = (List<SavedResult>)bf.Deserialize(file);
        file.Close();

		if(results.Count!=0){
			return results;
		}
		else{return null;}
	}
	return null;
}

}