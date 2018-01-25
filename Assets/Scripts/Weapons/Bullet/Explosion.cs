using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IBullet {

	// Should have Trigger Circle Collider

	private PhotonView ownerView;
	private float damage;
	private float radius;

	public void Initialize(float damage, float bulletSpeed, PhotonView ownerView){
		this.ownerView = ownerView;
		this.damage = damage;
		this.radius = 100.0f / bulletSpeed;
		this.transform.localScale = new Vector3 (radius, radius, 1);
		StartCoroutine (LateDestroy ());
	}


	public float GetDamage(){
		return this.damage;
	}

	public PhotonView GetOwnerView(){
		return this.ownerView;
	}

	public bool DestroyThis(){
		return false;
	}

	void OnTriggerStay2D(Collider2D other){
		GameObject otherGO = other.gameObject;
		if (otherGO.CompareTag ("Enemy")) {
			otherGO.GetComponent<EnemyMono> ().TakeDamageExternal (damage);
		}
		Destroy (this.gameObject.GetComponent<Collider2D>());
	}

	private IEnumerator LateDestroy(){
		yield return new WaitForSeconds (0.2f);
		Destroy (this.gameObject);
	}


}
