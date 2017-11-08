using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassCreator : MonoBehaviour {
	//Modifiers
	//Movespeed: Base, armor penalty mod, pickup penalty mod
	//Bonus armor?
	//Reload speed by type


	public Class CreateClass(string name){
		string beginning = "class_" + name + "_";
		DataManager.ReadDataString (beginning + "mod");
	}
}


public class ClassModifier{
	public enum modifierType {spdBase, spdArmor, spdPickup, armor, rldGl, rldAssault, rldSmg, rldShotgun, rldLmg, rldSniper, rldPistol};
	public modifierType mod;
	public float value;

}