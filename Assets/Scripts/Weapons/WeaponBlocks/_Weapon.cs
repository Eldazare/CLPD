using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Weapon {

	//Stats
	public GameObject weaponShot;
	public string type;
	public float rof; // in seconds
	public float damage;
	public float spread;
	public float ammo;
	public float reload; // in seconds
	public float swapspeed;

	protected bool RateOfFireBool = true; // true = shooting allowed


	//shot
	virtual
	public IEnumerator Shoot (PlayerBody playerBody){
		if (RateOfFireBool) {
			RateOfFireBool = false;
			//TODO do stuff here
			yield return new WaitForSeconds (rof);
			RateOfFireBool = true;
		}
	}

	abstract
	public float GetWeaponReloadMod (Class playerClass);

	public IEnumerator RofCounter(){ // used for implementation in child class shoot methods
		RateOfFireBool = false;
		yield return new WaitForSeconds (rof);
		RateOfFireBool = true;
	}
}
