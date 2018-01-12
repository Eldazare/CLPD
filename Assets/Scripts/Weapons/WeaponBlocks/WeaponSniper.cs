using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSniper : _Weapon {

	// normal weapon shooting is not used for this weapon

	override
	public IEnumerator Shoot(){
		if (!owner.IsMoving ()) {
			if (rateOfFireBool && this.ammo > 0) {
				rateOfFireBool = false;
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 mouse2DPos = new Vector2 (mousePos.x, mousePos.y);
				RaycastHit2D hit = Physics2D.Raycast (mouse2DPos, Vector2.zero, 0f);
				if (hit) {
					if (hit.transform.CompareTag ("Enemy")) {
						EnemyMono hitEnemy = hit.transform.GetComponent<EnemyMono> ();
						hitEnemy.TakeDamage (this.damage);
					}
				}
				yield return new WaitForSeconds (rof);
				rateOfFireBool = true;
			}
		}
	}

	override
	public float GetWeaponReloadMod (Class playerClass){
		return playerClass.rldSniper;
	}
}
