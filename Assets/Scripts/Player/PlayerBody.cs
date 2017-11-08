using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour {

	//--Attributes--
	public Class playerClass;
	public _Weapon weapon1;
	public _Weapon weapon2;
	//consumable thing
	//armor

	//pickup

	public void ShootWeapon(){
		
	}


	//RuntimeVariables
	public int currentWeapon;
	public float healthCurrent;
	public float movespeedMod; //updated every frame?

	public float money;
}
