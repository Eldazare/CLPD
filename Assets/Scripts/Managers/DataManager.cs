using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager  {
	//Text files: Classes.txt, Drops.txt, Enemies.txt, Player.txt, Weapons.txt
	//Identifiers: class | drop | enemy | player | weapon
	static private bool readBool = false;

	static Dictionary<string,string> classData = new Dictionary<string, string>{ };
	static Dictionary<string,string> dropData = new Dictionary<string, string>{ };
	static Dictionary<string,string> enemyData = new Dictionary<string, string>{ };
	static Dictionary<string,string> playerData = new Dictionary<string, string>{ };
	static Dictionary<string,string> weaponData = new Dictionary<string, string>{ };
	// could also do <string,weaponStats> dictionary

	static public string ReadDataString(string entryName){
		if (!readBool) {
			DownloadTextData ();
		}
		string identifier = entryName.Split ("_".ToCharArray())[0];
		switch (identifier) {
		case "class":
			return classData [entryName];
		case "drop":
			return dropData [entryName];
		case "enemy":
			return enemyData [entryName];
		case "player":
			return playerData [entryName];
		case "weapon":
			return weaponData [entryName];
		default:
			Debug.LogError ("DataManager was given unsuitable identifier in entryName (" + entryName + ").");
			return null;
		}
	}

	static public float ReadDataFloat(string entryName){
		string value = ReadDataString (entryName);
		return float.Parse (value);
	}


	static private void DownloadTextData(){
		DownloadSingleFile ("Classes.txt", weaponData);
		DownloadSingleFile ("Drops.txt", weaponData);
		DownloadSingleFile ("Enemies.txt", weaponData);
		DownloadSingleFile ("Player.txt", weaponData);
		DownloadSingleFile ("Weapons.txt", weaponData);

	}

	static private void DownloadSingleFile(string filename, Dictionary<string,string> dic){
		string path = "Assets/Managers/TextData/" + filename;
		StreamReader reader = new StreamReader(path); 
		string[] data = reader.ReadToEnd ().Split("\n".ToCharArray());
		Debug.Log (data [0]);
		foreach (string line in data) {
			if (line [0] != ""[0] && line[0] != "#"[0]) {
				string[] keyValue = line.Split ("=".ToCharArray());
				dic.Add (keyValue [0], keyValue [1]);
			}
		}
	}
}
