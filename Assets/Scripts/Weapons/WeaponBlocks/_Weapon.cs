using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Weapon {


	//Stats
	protected PlayerBody owner;
	public GameObject weaponShot;
	public string type;
	public string name;
	public float rof; // in seconds
	public float damage;
	public float spread;
	public float ammoMax;
	public float ammo;
	public float reload; // in seconds
	public float swapspeed;
	public float bulletspeed;
	public float bulletdistance;
	// Shotspeed?

	protected bool rateOfFireBool = true; // true = shooting allowed


	//shot
	virtual
	public IEnumerator Shoot (){
		if (rateOfFireBool && (this.ammo > 0)) {
			rateOfFireBool = false;
			ammo -= 1;
			owner.Shoot (weaponShot, this);
			yield return new WaitForSeconds (rof);
			rateOfFireBool = true;
		}
	}

	virtual
	public void ReloadWeapon(){
		this.ammo = this.ammoMax;
	}

	virtual
	public void ReceiveOwner(PlayerBody owner){
		this.owner = owner;
	}

	abstract
	public float GetWeaponReloadMod (Class playerClass);
}
