using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponCreator {
	// Weapon types: gl assault smg shotgun lmg sniper pistol
	public static GameObject bulletPrefab;

	public static _Weapon CreateWeapon(string name){
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
			Debug.Log ("No weapon name found in switch");
			return null;
		}
		wep.type = weaponType;
		wep.name = name;
		wep.rof = DataManager.ReadDataFloat (pathBegin + "rof");
		wep.damage = DataManager.ReadDataFloat (pathBegin + "damage");
		wep.spread = DataManager.ReadDataFloat (pathBegin + "spread");
		wep.ammoMax = DataManager.ReadDataFloat(pathBegin + "ammo");
		wep.ReloadWeapon ();	
		wep.reload = DataManager.ReadDataFloat(pathBegin + "reload");
		wep.swapspeed = DataManager.ReadDataFloat(pathBegin + "swapspeed");
		wep.bulletspeed = DataManager.ReadDataFloat (pathBegin + "bulletspeed");
		wep.bulletdistance = DataManager.ReadDataFloat (pathBegin + "bulletdistance");
		wep.shottype = DataManager.ReadDataInt (pathBegin + "shotType");
		return wep;
	}

	public static IConsumable CreateConsumable(string name){
		switch (name) {
		case "ammo":
			return new ConsumableAmmo ();
		case "flashbang":
			return new ConsumableFlashbang ();
		case "javelin":
			return new ConsumableJavelin ();
		case "medkit":
			return new ConsumableMedkit ();
		case "rocketLauncher":
			return new ConsumableRocketLauncher ();
		case "stims":
			return new ConsumableStims ();
		default:
			Debug.LogError ("Unsuitable consumable name found, cross-reference Player.txt or someshit");
			return null;
		}
	}
}
