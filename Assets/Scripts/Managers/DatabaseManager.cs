using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DatabaseManager {

	private static string storeScriptPath = "http://users.metropolia.fi/~sakarioh/Scripts/clpdScores.php";
	private static string getScriptPath = "http://users.metropolia.fi/~sakarioh/Scripts/clpdGetScores.php";

	private static string rawScores = null;

	public static IEnumerator StoreScore(string name, int score, int level){
		WWWForm form = new WWWForm ();
		form.AddField ("name", name);
		form.AddField ("score", score);
		form.AddField ("level", level);
		WWW download = new WWW (storeScriptPath, form);
		yield return download;
		if (download.error != null) {
			Debug.LogError (download.error);
		} else {
			Debug.Log (download.text);
			Debug.Log ("StoreScore success!");
		}
	}

	public static IEnumerator LoadScores(){
		WWW download = new WWW (getScriptPath);
		yield return download;
		if (download.error != null) {
			Debug.LogError (download.error);
			rawScores = "ERROR LOADING SCORES!";
		} else {
			if (download.text != "") {
				rawScores = download.text;
			} else {
				rawScores = "No data on database. Why not go into game and change that? (Empty name doesn't register on game end, so make sure to not leave the name slot empty on first menu)";
			}
			Debug.Log (download.text);
			Debug.Log ("Get scores Success!");
		}

	}

	public static string GetScores(){
		return rawScores;
	}
}
