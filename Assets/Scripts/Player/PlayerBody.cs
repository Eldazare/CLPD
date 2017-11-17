using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour {


	private Rigidbody2D playerRigidBody;

	//--Attributes--
	private Class playerClass;
	private _Weapon weapon1;
	private _Weapon weapon2;
	private Armor armor;
	private IConsumable consumable;
	private IPickup pickup;

	//Body constants
	private float healthMax;
	private float movespeedBase;
	private float movespeedReloadBase;

	//RuntimeVariables
	private int currentWeapon;
	private bool reloading;
	public float healthCurrent;
	public float money;


	//Counters
	public float consumableCDLeft; // in damage/kills
	public float reloadLeft; // in time

	//movespeed mods
	public float movespeedModPickup = 1.0f;
	public float movespeedModReload = 1.0f;
	public float movespeedModArmor = 1.0f;


	private void Initialize(Class playerClass){
		this.playerClass = playerClass;
		this.playerRigidBody = GetComponent<Rigidbody2D> ();
		currentWeapon = 1; 
		reloading = false;
		healthMax = DataManager.ReadDataFloat ("player_health_max");
		healthCurrent = healthMax;
		money = 0; //??
		movespeedBase = DataManager.ReadDataFloat ("player_movespeed_base");
		movespeedReloadBase = DataManager.ReadDataFloat ("player_movespeed_reloadMod");
	}

	void FixedUpdate(){
		float movespeedMod = movespeedBase * playerClass.spdBase * movespeedModPickup * movespeedModReload * movespeedModArmor;
		Vector2 direction = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		playerRigidBody.velocity = direction * movespeedMod;
	}

	public void ShootWeapon(){
		StartCoroutine (GetCurrentWeapon ().Shoot (this));
	}

	public IEnumerator Reload(){
		this.movespeedModReload = movespeedReloadBase;
		reloading = true;
		yield return new WaitForSeconds (GetCurrentWeapon ().reload);
		reloading = false;
		this.movespeedModReload = 1.0f;
	}

	public void ChangeWeapon(int weapon){
		this.currentWeapon = weapon;
	}

	public void PickupItem(IPickup pickup){
		if (pickup != null){
			this.pickup = pickup;
			this.movespeedModPickup = 1- (pickup.GetPickupMovespeedMod () * playerClass.spdPickup);
		} else {
			this.movespeedModPickup = 1;
		}
	}

	public void DonArmor(Armor armor){
		if (armor != null) {
			this.armor = armor;
			this.movespeedModArmor = 1 - (armor.movespeedMod * playerClass.spdArmor);
		} else {
			this.movespeedModArmor = 1;
		}
	}


	private _Weapon GetCurrentWeapon(){
		switch (currentWeapon) {
		case 1:
			return weapon1;
		case 2:
			return weapon2;
		default: 
			Debug.Log ("Current weapon number not found in switch");
			return null;
		}
	}
}
