using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	//enemy prefabs			// DISCLAIMER, if you add more, remember to check first row of constructor and int value at Enemy.txt
	public GameObject e0pf;
	public GameObject e1pf;
	public GameObject e2pf;
	public GameObject e3pf; 
	/*
	public GameObject e4pf; 
	public GameObject e5pf; 
	*/

	public GameObject[] enemyPrefabList;

	void Awake(){
		enemyPrefabList = new GameObject[] { e0pf, e1pf, e2pf , e3pf/*, e4pf, e5pf*/ };
	}

	public void Spawn(int enemyIndex, Vector3 spawnPoint){
		Instantiate (enemyPrefabList [enemyIndex], spawnPoint, Quaternion.identity);
	}


	//Move to static class? Used by pauseMenu button.
	public void EndSceneNow(){
		GameObject.FindGameObjectWithTag ("MANAGER").GetComponent<MenuManager> ().PrematureGameEnd (false);
	}
}
