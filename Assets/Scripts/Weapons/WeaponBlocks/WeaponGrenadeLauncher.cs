using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenadeLauncher : _Weapon {

	override
	public float GetWeaponReloadMod (Class playerClass){
		return playerClass.rldGl;
	}
}
