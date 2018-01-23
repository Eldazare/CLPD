using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager {

	private static List<int> scores = new List<int>{1, 2, 3, 3};
	public static int currentScore = 0;
	public static string playerName;
	public static int level;

	public static void Initialize(int numberOfEnemies){
		currentScore = 0;
		if (scores.Count < numberOfEnemies) {
			Debug.LogError ("ScoreManager has too few scores logged in list");
		}
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
	}

	public static void enemyKilled(int enemyType){
		currentScore += scores[enemyType];
	}

	public static string GetEndScoreText(bool victory){
		string end = "PLAYER: "+playerName+"\nSCORE: " + currentScore;
		if (victory) {
			end += "\nLEVEL: CLEARED";
		} else {
			end += "\nLEVEL: FAILED";
		}
		return end;
	}


	public static void ClearBonus(float difficulty, float passedTime){
		float scoreFloat = currentScore * (0.2f + Mathf.Sqrt (difficulty));
		currentScore = Mathf.RoundToInt (scoreFloat);
		float totalTimeMod = difficulty * 60.0f - passedTime;
		if (totalTimeMod > 0) {
			currentScore += Mathf.RoundToInt(totalTimeMod);
		}
	}
}
