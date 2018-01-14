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

	void Awake (){
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
		pattern.MovStart (enemyRig);
	}

	void FixedUpdate(){
		pattern.Movement (enemyRig, selfData.speed);
	}

	public EnemyData GetEnemyData(){
		return selfData;
	}

	// Enemies doesn't always know who hit them
	public void TakeDamage(float amount){
		currentHP -= amount;
		enemyAud.Stop (); enemyAud.Play ();
		//Debug.Log ("Took " + amount + " damage, has " + currentHP + " left.");
		if (currentHP <= 0) {
			curWave.EnemyDeathReport (enemyType);
			Destroy (this.gameObject);
		}
	}

	private void OutOfBounds(){
		curWave.EnemyDeathReportOutOfBounds (enemyType);
		Destroy (this.gameObject);
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
				float damage = bull.GetDamage ();
				this.TakeDamage (damage);
				if (bull.DestroyThis()){
					Destroy (other.gameObject);
				}
			}
		} else if (other.CompareTag ("OuterWall")) {
			OutOfBounds ();
		}
	}
}
