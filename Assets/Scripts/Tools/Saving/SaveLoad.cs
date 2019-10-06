using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*

for saving game data, such as scores

 */
public static class SaveLoad {

	public static List<SavedResult> results = new List<SavedResult>(); //scores for completed games

//saves score from completed solo games. Either this will remain as such or will be expanded to handle all data that needs to be saved
	public static void SaveSoloResults(string name, int score){
		SavedResult tempResult = new SavedResult (){playerName =name, result = score };
 		results.Add(tempResult);
    	BinaryFormatter bf = new BinaryFormatter();
    	FileStream file = File.Create (Application.persistentDataPath + "/savedResultsSolo.gd");
    	bf.Serialize(file, SaveLoad.results);
    	file.Close();

	}
	//loading data from file. rn only loads results from solo games
	public static List<SavedResult> Load() {
    if(File.Exists(Application.persistentDataPath + "/savedResultsSolo.gd")) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/savedResultsSolo.gd", FileMode.Open);
        SaveLoad.results = (List<SavedResult>)bf.Deserialize(file);
        file.Close();

		if(results.Count!=0){	//sends saved results to where ever they are needed (scoreboard mostly), if nothing is saved yet return null
			return results;
		}
		else{return null;}
	}
	return null;
}

}