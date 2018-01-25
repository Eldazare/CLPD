using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShotgun : _Weapon {

	override
	public float GetWeaponReloadMod (Class playerClass){
		return playerClass.rldShotgun;
	}

	override
	public IEnumerator Shoot(){
		if (rateOfFireBool && this.ammo > 0) {
			rateOfFireBool = false;
			this.ammo -= 1;
			for (int i = 0; i < 8; i++) {
				CallShoot ();
			}
			yield return new WaitForSeconds (rof);
			rateOfFireBool = true;
		}
	}
}
