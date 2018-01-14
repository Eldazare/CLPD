using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

	public int finalWave;
	private float spawnModifier;
	private float baseSpawnRate;
	private float clearModifier;
	public string baseString;

	public int currentWave;
	public WaveManager currentWaveManager;
	private string level;


	public LevelManager(string level){
		baseString = "level_" + level + "_";
		string data = DataManager.ReadDataString (baseString + "header");
		string[] splitData = data.Split (";" [0]);
		finalWave = int.Parse(splitData [1]);
		spawnModifier = float.Parse(splitData [0]);
		clearModifier = DataManager.ReadDataFloat ("level_waveclearmodifier");
		baseSpawnRate = DataManager.ReadDataFloat ("level_basespawnmod");
		currentWave = 1;
		ScoreManager.ReceiveLevel (int.Parse (level));
		this.level = level;
	}

	public IEnumerator WaveLoop (){
		SoundManager.PlayMusic ("battleBGM"+level);
		int numberOfEnemyTypes = int.Parse (DataManager.ReadDataString ("enemy_numberOfEnemies"));
		ScoreManager.Initialize (numberOfEnemyTypes);
		while (currentWave <= finalWave) {
			string waveString = baseString + currentWave;
			currentWaveManager = new WaveManager (waveString, numberOfEnemyTypes, spawnModifier, clearModifier, baseSpawnRate);
			yield return currentWaveManager.WaveSpawnScript ();
			yield return currentWaveManager.WaveEndCheck ();
			currentWave += 1;
		}
	}
}
