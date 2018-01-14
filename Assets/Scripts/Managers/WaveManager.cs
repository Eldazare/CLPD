using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager {

	private List<int> enemyCounterList = new List<int>{};
	private int totalEnemyCount;
	private AudioSource deathSounder;

	Spawner spawner;
	GameObject[] spawnpointList;
	float spawnModifier;
	float baseSpawnModifier;
	float clearModifier;


	public WaveManager(string loadString, int nOEnemy, float spawnModifier, float clearModifier, float baseSpawnModifier){
		GameObject spawnerO = GameObject.FindGameObjectWithTag ("Spawner");
		spawner = spawnerO.GetComponent<Spawner> ();
		if (nOEnemy != spawner.enemyPrefabList.Length) {
			Debug.LogError ("prefabList is not as long as it should be");
		}
		deathSounder = GameObject.FindGameObjectWithTag ("MANAGER").GetComponents<AudioSource> () [1];
		deathSounder.clip = SoundManager.GetSoundEffect ("enemyDeath");
		string value = DataManager.ReadDataString (loadString);
		string[] valueSplit = value.Split (";" [0]);
		spawnpointList = GameObject.FindGameObjectsWithTag("Spawnpoint");
		totalEnemyCount = 0;
		for (int i = 0; i < valueSplit.Length; i++) {
			int amount = int.Parse (valueSplit [i]);
			enemyCounterList.Add (amount);
			if (i != 0) {
				totalEnemyCount += amount;
			}
		}
		while (nOEnemy > enemyCounterList.Count) {
			enemyCounterList.Add (0);
		}
		this.baseSpawnModifier = baseSpawnModifier;
		this.spawnModifier = spawnModifier;
		this.clearModifier = clearModifier;
		EnemyStatter.SetCurrentWave (this);
	}


	public void EnemyDeathReport(int type){
		enemyCounterList [type] -= 1;
		ScoreManager.enemyKilled (type);
		deathSounder.Stop ();
		deathSounder.Play ();
		//Debug.Log ("Type " + type + " enemy has died.");
	}

	public void EnemyDeathReportOutOfBounds(int type){
		SpawnSingle (type);
	}

	public IEnumerator WaveEndCheck(){
		int currentTotal = 0;
		for (int i = 1;i<enemyCounterList.Count;i++){
			currentTotal += enemyCounterList [i];
		}
		float modifiedTotalCount = (float)totalEnemyCount * (float)clearModifier;
		while(currentTotal > modifiedTotalCount) {
			Debug.Log ("currently " + currentTotal + "| Needed to clear " + modifiedTotalCount);
			yield return new WaitForSeconds(2.0f);
			currentTotal = 0;
			for (int i = 1;i<enemyCounterList.Count;i++){
				currentTotal += enemyCounterList [i];
			}
		}
		Debug.Log ("Wave has passed");
	}

	public IEnumerator WaveSpawnScript(){
		List<int> bufferList = new List<int>{ };
		foreach (int i in enemyCounterList) {
			bufferList.Add (i);
		}
		List<int> positiveIndexList = new List<int>{ };
		int spawnsLeft = 0; //arbitary initialization
		for(int i=1;i<bufferList.Count;i++){
			spawnsLeft += bufferList [i];
			positiveIndexList.Add (i);
		}
		for (int i = 0; i < enemyCounterList [0]; i++) { // spawn mindlesses (type 0)
			int spawnIndex = Random.Range (0, spawnpointList.Length);
			Vector3 spawnPoint = spawnpointList [spawnIndex].transform.position;
			spawner.Spawn (0, spawnPoint);
			yield return new WaitForSeconds (0.1f);
		}
		while (spawnsLeft > 0) { // spawn rest of the types
			int spawnIndex = Random.Range (0, spawnpointList.Length);
			Vector3 spawnPoint = spawnpointList [spawnIndex].transform.position;
			int randomPositiveIndexIndex = Random.Range (0, positiveIndexList.Count);
			int enemyIndex = positiveIndexList [randomPositiveIndexIndex];
			while (bufferList [enemyIndex] == 0) {
				positiveIndexList.RemoveAt (randomPositiveIndexIndex);
				randomPositiveIndexIndex = Random.Range (0, positiveIndexList.Count);
				enemyIndex = positiveIndexList [randomPositiveIndexIndex];
			}
			spawner.Spawn (enemyIndex, spawnPoint);
			bufferList [enemyIndex] -= 1;
			spawnsLeft -= 1;
			yield return new WaitForSeconds (spawnModifier*baseSpawnModifier);
		}
		Debug.Log ("Wave has spawned");
	}

	private void SpawnSingle(int type){
		int spawnIndex = Random.Range (0, spawnpointList.Length);
		Vector3 spawnPoint = spawnpointList [spawnIndex].transform.position;
		spawner.Spawn (type, spawnPoint);
	}
}
