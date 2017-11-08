using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponCreator {
	// Weapon types: gl assault smg shotgun lmg sniper pistol


	static public _Weapon CreateWeapon(string name){
		string pathBegin = "weapon_" + name + "_";
		string weaponType = DataManager.ReadDataString (pathBegin + "type");
		_Weapon wep;
		switch (weaponType) {
		case "gl":
			wep = new WeaponGrenadeLauncher ();
			break;
		case "assault":
			wep = new WeaponAssault ();
			break;
		case "smg":
			wep = new WeaponSMG ();
			break;
		case "shotgun":
			wep = new WeaponShotgun ();
			break;
		case "lmg":
			wep = new WeaponLMG ();
			break;
		case "sniper":
			wep = new WeaponSniper ();
			break;
		case "pistol":
			wep = new WeaponPistol ();
			break;
		default:
			return null;
		}
		wep.type = weaponType;
		wep.rof = DataManager.ReadDataFloat (pathBegin + "rof");
		wep.damage = DataManager.ReadDataFloat (pathBegin + "damage");
		wep.spread = DataManager.ReadDataFloat (pathBegin + "spread");
		wep.ammo = DataManager.ReadDataFloat(pathBegin + "ammo");
		wep.reload = DataManager.ReadDataFloat(pathBegin + "reload");
		wep.swapspeed = DataManager.ReadDataFloat(pathBegin + "swapspeed");
		return wep;
	}
}
