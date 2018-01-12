using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassCreator {
	//TODO: create checks in case of failure (modData not found, mod data false etc.)
	public static Class CreateClass(string name){
		Debug.Log ("Creating class named " + name);
		Class tempClass = new Class (name);
		string beginning = "class_" + name + "_";
		string modData = DataManager.ReadDataString (beginning + "mod");
		string[] parsedModData = modData.Split (";".ToCharArray());
		foreach (string data in parsedModData) {
			string[] mod = data.Split (",".ToCharArray ());
			tempClass.ModifyMod (mod[0],float.Parse(mod[1]));
		}
		return tempClass;
	}
}