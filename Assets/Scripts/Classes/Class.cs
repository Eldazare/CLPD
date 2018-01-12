using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class {

	public string name;

	//spdBase, spdArmor, spdPickup, armor, rldGl, rldAssault, rldSmg, rldShotgun, rldLmg, rldSniper, rldPistol
	public float spdBase; // can be anything
	public float spdArmor; // lower is better, from 1 to 0
	public float spdPickup;
	public float dmgTaken; // lower is better, from 1 to 0
	public float rldGl;
	public float rldAssault;
	public float rldSmg;
	public float rldShotgun;
	public float rldLmg;
	public float rldSniper;
	public float rldPistol;

	public Class (string name){
		this.name = name;
		spdBase = 		1.0f;
		spdArmor = 		1.0f;
		spdPickup = 	1.0f;
		dmgTaken = 		1.0f;
		rldGl = 		1.0f;
		rldAssault = 	1.0f;
		rldSmg = 		1.0f;
		rldShotgun = 	1.0f;
		rldLmg = 		1.0f;
		rldSniper = 	1.0f;
		rldPistol = 	1.0f;
	}


	public void ModifyMod(string modType, float value){
		switch (modType) {
		case "spdBase":
			this.spdBase = value;
			break;
		case "spdArmor":
			this.spdArmor = value;
			break;
		case "spdPickup":
			this.spdPickup = value;
			break;
		case "dmgTaken":
			this.dmgTaken = value;
			break;
		case "rldGl":
			this.rldGl = value;
			break;
		case "rldAssault":
			this.rldAssault = value;
			break;
		case "rldSmg":
			this.rldSmg = value;
			break;
		case "rldShotgun":
			this.rldShotgun = value;
			break;
		case "rldLmg":
			this.rldLmg = value;
			break;
		case "rldSniper":
			this.rldSniper = value;
			break;
		case "rldPistol":
			this.rldPistol = value;
			break;
		default: 
			Debug.Log ("no mod name in switch");
			break;
		}
	}
}
