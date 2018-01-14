using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IBullet {

	// Should have Trigger Circle Collider

	private float damage;
	private float radius;
	bool onTriggered = false;

	public void Initialize(float damage, float travelledDistance){
		this.damage = damage;
		this.radius = travelledDistance * 0.35f;
		this.GetComponent<CircleCollider2D> ().radius = this.radius;
		StartCoroutine (LateDestroy ());
	}


	public float GetDamage(){
		return this.damage;
	}

	public bool DestroyThis(){
		return false;
	}

	void OnTriggerStay2D(Collider2D other){
		GameObject otherGO = other.gameObject;
		if (otherGO.CompareTag ("Enemy")) {
			otherGO.GetComponent<EnemyMono> ().TakeDamage (this.damage);
		}
		onTriggered = true;
	}

	private IEnumerator LateDestroy(){
		while (!onTriggered) {
			yield return null;
		}
		Destroy (this.gameObject.GetComponent<Collider2D>());
		yield return new WaitForSeconds (0.3f);
		Destroy (this.gameObject);
	}


}
