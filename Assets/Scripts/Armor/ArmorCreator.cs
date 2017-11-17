using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArmorCreator {

	public static Armor CreateArmor(string name){
		string pathBegin = "armor_" + name + "_";
		Armor armor = new Armor (name);
		armor.armorHP = DataManager.ReadDataFloat (pathBegin + "armorHP");
		armor.movespeedMod = DataManager.ReadDataFloat (pathBegin + "movespeedMod");
		armor.recoverySpd = DataManager.ReadDataFloat (pathBegin + "recoverySpd");
		return armor;
	}
}
