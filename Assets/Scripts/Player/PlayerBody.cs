using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour {

	Plane plane = new Plane(new Vector3(0,0,-1),new Vector3(0,0,0)); // plane for raycasting player rotation
	private Rigidbody2D playerRigidBody;
	private AudioSource playerAudioSource0; // for gun sounds
	private AudioSource playerAudioSource1; // for onHitEffects
	public GameObject emptyChildShoot;
	private InputManager inputManager;

	//--Attributes--
	private Class playerClass;
	private _Weapon weapon1;
	private _Weapon weapon2;
	public Armor armor;
	private IConsumable consumable;
	private IPickup pickup;

	//Body constants
	public float healthMax;
	private float movespeedBase;
	private float movespeedReloadBase;
	private float reloadSpeedStandingMod;

	//RuntimeVariables
	private int currentWeapon;
	public bool reloading;
	public float healthCurrent;
	public float money;


	//Counters
	public float consumableCDLeft; // in damage/kills
	public float consumableCDMax;
	public float reloadLeft; // in time
	public float reloadMax;

	//movespeed mods
	public float movespeedModPickup = 1.0f;
	public float movespeedModReload = 1.0f;
	public float movespeedModArmor = 1.0f;


	public void Initialize(Class playerClass){
		this.playerClass = playerClass;
		this.playerRigidBody = GetComponent<Rigidbody2D> ();
		currentWeapon = 1; 
		reloading = false;
		healthMax = DataManager.ReadDataFloat ("player_health_max");
		healthCurrent = healthMax;
		money = 0; //??
		movespeedBase = DataManager.ReadDataFloat ("player_movespeed_base");
		movespeedReloadBase = DataManager.ReadDataFloat ("player_movespeed_reloadMod");
		reloadSpeedStandingMod = DataManager.ReadDataFloat ("player_reload_standingMod");
		this.inputManager = this.GetComponent<InputManager> ();
		this.playerAudioSource0 = this.GetComponents<AudioSource> () [0];
		this.playerAudioSource1 = this.GetComponents<AudioSource> () [1];
		this.playerAudioSource1.clip = SoundManager.GetSoundEffect ("playerOnHit");
		Debug.Log ("Initialize complete");
	}

	void FixedUpdate(){
		MovePlayer ();
		RotatePlayer ();
		if (armor != null) {
			armor.recoveryCurrent -= Time.fixedDeltaTime;
		}
	}

	private void MovePlayer(){
		float movespeedMod = movespeedBase * playerClass.spdBase * movespeedModPickup * movespeedModReload * movespeedModArmor;
		Vector2 direction = inputManager.GetMovementInput();
		playerRigidBody.velocity = direction * movespeedMod;
	}

	private void RotatePlayer (){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float rayDistance;
		Vector3 point;
		if (plane.Raycast (ray, out rayDistance)){
			point = ray.GetPoint (rayDistance);
			float AngleRad = Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x);
			float AngleDeg = (180 / Mathf.PI) * AngleRad;
			this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg-90);
		}
	}

	public bool IsMoving(){
		float veloci = playerRigidBody.velocity.magnitude;
		if (veloci != 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool ShootWeapon(){
		_Weapon curWep = GetCurrentWeapon ();
		if (curWep != null) {
			if (curWep.ammo > 0) {
				StartCoroutine (curWep.Shoot ());
				return true;
			} else {
				return false;
			}
		} else {
			Debug.Log ("No weapon");
			return true;
		}
	}

	public IEnumerator Reload(){
		_Weapon curWep = GetCurrentWeapon ();
		if (curWep != null) {
			reloading = true;
			if (this.IsMoving ()) {
				this.movespeedModReload = movespeedReloadBase;
				yield return ReloadWait (GetCurrentWeapon ().reload);
			} else {
				this.movespeedModReload = 0.0f;
				yield return ReloadWait (GetCurrentWeapon ().reload * reloadSpeedStandingMod);
			}
			GetCurrentWeapon ().ReloadWeapon ();
			reloading = false;
			this.movespeedModReload = 1.0f;
		}
	}

	private IEnumerator ReloadWait(float time){
		reloadMax = time;
		reloadLeft = time;
		while (reloadLeft > 0) {
			yield return null;
			reloadLeft -= Time.deltaTime;
		}
	}

	public void SwapWeapon(){
		switch (currentWeapon) {
		case 1:
			currentWeapon = 2;
			break;
		case 2:
			currentWeapon = 1;
			break;
		default: 
			Debug.Log ("Current weapon number not found in switch");
			break;
		}
	}

	public void PickupItem(IPickup pickup){
		if (pickup != null){
			this.pickup = pickup;
			this.movespeedModPickup = 1 - (pickup.GetPickupMovespeedMod () * playerClass.spdPickup);
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

	public void DonWeapon(_Weapon weapon, int slot){
		weapon.ReceiveOwner (this);
		switch (slot) {
		case 1:
			weapon1 = weapon;
			break;
		case 2: 
			weapon2 = weapon;
			break;
		default:
			return;
		}
	}

	public void DonConsumable(IConsumable consumable){
		this.consumable = consumable;
	}

	public void UseConsumable(){
		if (consumable != null && consumableCDLeft <= 0) {
			// use consumable
		}
	}


	public _Weapon GetCurrentWeapon(){
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

	// TODO: Optimize, there is copypaste
	public void TakeDamage(float amount){
		Debug.Log ("Took damage");
		playerAudioSource1.Stop ();
		playerAudioSource1.Play ();
		if (armor != null) {
			if (armor.armorHPCurrent > 0) {
				armor.armorHPCurrent -= amount;
				armor.recoveryCurrent = armor.recoverySpd;
			} else {
				this.healthCurrent -= amount;
				if (this.healthCurrent <= 0) {
					GameObject.FindGameObjectWithTag ("MANAGER").GetComponent<MenuManager> ().PrematureGameEnd (false);
				}
			}
		} else {
			this.healthCurrent -= amount;
			if (this.healthCurrent <= 0) {
				GameObject.FindGameObjectWithTag ("MANAGER").GetComponent<MenuManager> ().PrematureGameEnd (false);
			}
		}
	}

	private void DeactivateBody(){
		
	}

	public void Shoot(GameObject bulletPrefab, _Weapon source, AudioClip gunClip){
		GameObject bullet = Instantiate (bulletPrefab, emptyChildShoot.transform.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<Bullet> ().Initialize (source, this.transform.rotation.eulerAngles.z);
		playerAudioSource0.Stop ();
		playerAudioSource0.clip = gunClip;
		playerAudioSource0.Play ();
	}

	void OnCollisionEnter2D (Collision2D other){
		if (other.gameObject.CompareTag ("Player")) {
			Physics2D.IgnoreCollision (this.GetComponent<Collider2D> (), other.collider);
		}
	}
}
