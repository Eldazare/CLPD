using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMono : MonoBehaviour {

	// Contains body data
	public int enemyType; // save to prefab
	private EnemyData selfData;
	private WaveManager curWave;
	private float currentHP;
	private Rigidbody2D enemyRig;
	private AudioSource enemyAud;
	private AudioClip enemyAudClip;
	private SuperMove pattern;
	private PhotonView photonView;

	private bool IsKillable;
	private bool BonusAvailable;

	void Awake (){
		IsKillable = true;
		BonusAvailable = true;
		photonView = GetComponent<PhotonView> ();
		selfData = EnemyStatter.GetEnemyStats (enemyType);
		currentHP = selfData.maxHP;
		curWave = EnemyStatter.GetCurrentWave ();
		enemyRig = this.GetComponent<Rigidbody2D> ();
		enemyAud = this.GetComponent<AudioSource> ();
		enemyAudClip = SoundManager.GetSoundEffect ("enemyHit");
		enemyAud.clip = enemyAudClip;
		pattern = EnemyMovement.GetPattern(selfData.pattern);
	}

	void Start(){
		if (photonView.isMine) {
			pattern.MovStart (enemyRig);
		}
	}

	void FixedUpdate(){
		if (photonView.isMine) {
			pattern.Movement (enemyRig, selfData.speed);
		}
	}

	public EnemyData GetEnemyData(){
		return selfData;
	}

	public void TakeDamageExternal(float amount){
		object[] objList = new object[] { amount };
		photonView.RPC ("TakeDamage", PhotonTargets.All, objList);
		if (currentHP <= 0) {
			if (BonusAvailable) {
				BonusAvailable = false;
				ScoreManager.EnemyKilled (enemyType);
			}
		}
	}

	// Enemies doesn't always know who hit them
	[PunRPC]
	private void TakeDamage(float amount){
		currentHP -= amount;
		enemyAud.Stop ();
		enemyAud.Play ();
		//Debug.Log ("Took " + amount + " damage, has " + currentHP + " left.");
		if (currentHP <= 0 && IsKillable) {
			IsKillable = false;
			if (PhotonNetwork.isMasterClient) {
				curWave.EnemyDeathReport (enemyType);
				PhotonNetwork.Destroy (this.gameObject);
			} else {
				GameObject.FindGameObjectWithTag ("PunManager").GetComponent<PUNManager> ().UpdateScoresOnly (this.enemyType);
			}
		}
	}

	private void OutOfBounds(){
		if (PhotonNetwork.isMasterClient) {
			curWave.EnemyDeathReportOutOfBounds (enemyType);
			PhotonNetwork.Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("InnerWall")){
			Physics2D.IgnoreCollision (this.GetComponent<Collider2D>(), other.collider);
		}
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<PlayerBody>().TakeDamage(this.selfData.damage);
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.CompareTag ("Bullet")) {
			if (this.currentHP > 0) {
				IBullet bull = other.GetComponent<IBullet> ();
				if (bull.GetOwnerView().isMine) {
					//object[] objList = new object[]{ bull.GetDamage() };
					//photonView.RPC ("TakeDamage", PhotonTargets.All, objList);
					TakeDamageExternal (bull.GetDamage ());
				}
				if (bull.DestroyThis ()) {
					Destroy (other.gameObject);
				}
			}
		} else if (other.CompareTag ("OuterWall")) {
			OutOfBounds ();
		}
	}
}
