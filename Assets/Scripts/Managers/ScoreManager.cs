using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager {

	private static List<int> scores;
	public static int currentScore = 0;
	public static string playerName;
	public static int level;
	private static string levelStr;

	public static void Initialize(){
		currentScore = 0;
		int numberOfEnemies = DataManager.ReadDataInt ("enemy_numberOfEnemies");
		string score = DataManager.ReadDataString ("enemy_pointvalues");
		string[] scoreArr = score.Split (";"[0]);
		scores = new List<int> (){ };
		foreach (string stri in scoreArr){
			scores.Add (int.Parse (stri));
		}
		if (scores.Count < numberOfEnemies) {
			Debug.LogError ("ScoreManager has too few scores logged in list");
		}
		Debug.Log ("ScoreManagerInitialized");
	}

	public static void ReceivePlayerName(string name){
		if (name != null) {
			playerName = name;
		} else {
			playerName = "";
		}
	}

	public static void ReceiveLevel(int currentLevel){
		level = currentLevel;
		levelStr = currentLevel.ToString()[0] + "-" + currentLevel.ToString()[1];
	}

	public static void EnemyKilled(int enemyType){
		currentScore += scores[enemyType];
		Debug.Log ("Score added"+scores[enemyType]);
	}

	public static string GetEndScoreText(bool victory){
		string end = "PLAYER: "+playerName+"\nSCORE: " + currentScore + "\nLEVEL: " + levelStr;
		if (victory) {
			end += "\nSTATUS: CLEARED";
		} else {
			end += "\nSTATUS: FAILED";
		}
		return end;
	}


	public static void ClearBonus(float difficulty, float passedTime){
		Debug.Log ("SCORE INITIAL:" + currentScore);
		float scoreFloat = currentScore * (0.2f + Mathf.Sqrt (difficulty));
		Debug.Log ("SCORE FLOAT:" + scoreFloat);
		currentScore = Mathf.RoundToInt (scoreFloat);
		float totalTimeMod = difficulty * 60.0f - passedTime;
		Debug.Log ("SCORE TOTALTIMEMOD:" + totalTimeMod);
		if (totalTimeMod > 0) {
			currentScore += Mathf.RoundToInt(totalTimeMod);
		}
	}
}
