using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStatter {

	private static List<EnemyData> dataList;
	private static bool IsReady = false;
	private static WaveManager currentWave = null;

	public static EnemyData GetEnemyStats(int enemyType){
		if (!IsReady) {
			ReadEnemyStatsFromFiles ();
			IsReady = true;
		}
		return dataList [enemyType];
	}

	public static WaveManager GetCurrentWave(){
		return currentWave;
	}

	public static void SetCurrentWave(WaveManager curWave){
		currentWave = curWave;
	}

	private static void ReadEnemyStatsFromFiles(){
		int numberOfTypes = DataManager.ReadDataInt ("enemy_numberOfEnemies");
		dataList = new List<EnemyData>{ };
		for (int i = 0; i < numberOfTypes; i++) {
			string dataEntryName = "enemy_" + i + "_";
			EnemyData newData = new EnemyData ();
			newData.name = DataManager.ReadDataString (dataEntryName + "name");
			newData.maxHP = DataManager.ReadDataFloat (dataEntryName + "health");
			newData.damage = DataManager.ReadDataFloat (dataEntryName + "damage");
			newData.speed = DataManager.ReadDataFloat (dataEntryName + "speed");
			newData.pattern = DataManager.ReadDataInt (dataEntryName + "pattern");
			dataList.Add (newData);
		}
	}
}
