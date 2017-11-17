using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShotgun : _Weapon {

	override
	public float GetWeaponReloadMod (Class playerClass){
		return playerClass.rldShotgun;
	}
}
