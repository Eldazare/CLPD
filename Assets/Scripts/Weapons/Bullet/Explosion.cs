using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IBullet {

	// Should have Trigger Circle Collider

	private float damage;
	private float radius;

	public void Initialize(float damage, float travelledDistance){
		this.damage = damage;
		this.radius = travelledDistance * 0.2f;
		this.GetComponent<CircleCollider2D> ().radius = this.radius;
		Destroy (this, 0.3f);
	}


	public float GetDamage(){
		return this.damage;
	}
}
