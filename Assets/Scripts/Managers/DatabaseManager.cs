using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public static class DatabaseManager {

	private static string scriptPath = "http://users.metropolia.fi/~sakarioh/Scripts/clpdScores.php";

	public static IEnumerator StoreScore(string name, int score, int level){
		WWWForm form = new WWWForm ();
		form.AddField ("name", name);
		form.AddField ("score", score);
		form.AddField ("level", level);
		WWW download = new WWW (scriptPath, form);
		yield return download;
		if (download.error != null) {
			Debug.LogError (download.error);
		} else {
			Debug.Log (download.text);
			Debug.Log ("StoreScore success!");
		}
	}
}
