using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor {

	public string name;
	public float armorHP;
	public float recoverySpd; // in milliseconds?
	public float movespeedMod; // from 0 to 1


	public Armor(string name){
		this.name = name;
	}
}
