using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet {

	// Body for all weapon-based bullets

	public GameObject explosion;
	public bool useExplosion = false;


	private float damage;
	private float bulletSpeed;
	private float aliveDistance;
	Rigidbody2D bulletRig;

	private Vector3 startPos;

	void Awake(){
		this.bulletRig = this.GetComponent<Rigidbody2D> ();
	}

	public void Initialize (_Weapon source, float rotation){
		this.damage = source.damage;
		this.bulletSpeed = source.bulletspeed;
		this.aliveDistance = source.bulletdistance;
		startPos = this.transform.position;
		float spread = Random.Range (-source.spread, source.spread);
		float newZRot = rotation+spread;
		this.transform.rotation = Quaternion.Euler (0, 0, newZRot);
		bulletRig.velocity = bulletSpeed * transform.up;
	}

	void Update(){
		Vector2 movedDis = this.transform.position - startPos;
		//Debug.Log (movedDis.magnitude);
		if (movedDis.magnitude > aliveDistance){
			if (useExplosion) {
				GameObject explosionGO = Instantiate (explosion, this.transform.position, Quaternion.identity) as GameObject;
				explosionGO.GetComponent<Explosion> ().Initialize (this.damage, movedDis.magnitude);
			}
			Destroy (this.gameObject);
		}
	}

	public float GetDamage(){
		float theDamage = this.damage;
		this.damage = 0;
		return theDamage;
	}

	public bool DestroyThis(){
		return true;
	}
}
