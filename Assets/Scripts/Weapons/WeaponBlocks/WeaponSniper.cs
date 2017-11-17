using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSniper : _Weapon {

	public WeaponSniper(){
		//weapon shot
	}

	override
	public IEnumerator Shoot(PlayerBody playerBody){
		yield return new WaitForSeconds (1.0f);
	}

	override
	public float GetWeaponReloadMod (Class playerClass){
		return playerClass.rldSniper;
	}
}
