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
	static Dictionary<string,string> armorData = new Dictionary<string, string>{ };
	static Dictionary<string,string> levelData = new Dictionary<string, string>{ };

	private static List<string> nameList = new List<string>{ };

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
		case "armor":
			return armorData [entryName];
		case "level":
			return levelData [entryName];
		default:
			Debug.LogError ("DataManager was given unsuitable identifier in entryName (" + entryName + ").");
			return null;
		}
	}

	static public List<string> GetNameList(){
		if (!readBool) {
			DownloadTextData ();
		}
		return nameList;
	}

	static public float ReadDataFloat(string entryName){
		string value = ReadDataString (entryName);
		return float.Parse (value);
	}

	static public int ReadDataInt(string entryName){
		string value = ReadDataString (entryName);
		return int.Parse (value);
	}


	static private void DownloadTextData(){
		DownloadSingleFile ("Classes", classData);
		DownloadSingleFile ("Drops", dropData);
		DownloadSingleFile ("Enemies", enemyData);
		DownloadSingleFile ("Player", playerData);
		DownloadSingleFile ("Weapons", weaponData);
		DownloadSingleFile ("Armor", armorData);
		DownloadSingleFile ("Levels", levelData);
		readBool = true;
	}

	static private void DownloadSingleFile(string filename, Dictionary<string,string> dic){
		string path = "TextData/" + filename;
		TextAsset fullData = Resources.Load (path) as TextAsset; 
		string[] data = fullData.text.Split("\r\n".ToCharArray());
		foreach (string line in data) {
			if ((line.Length > 0) && (line[0] != "#"[0])) {
				if (line [0] == "%"[0]) {
					string lineN = line.Substring (1);
					nameList.Add (lineN);
				} else {
					string[] keyValue = line.Split ("="[0]);
					dic.Add (keyValue [0], keyValue [1]);
				}
			}
		}
	}
}
