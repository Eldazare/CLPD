using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public static class DatabaseManager {

	private static string host = "mysql.metropolia.fi";
	private static string dbname = "sakarioh";
	private static string user = "sakarioh";
	private static string passwrd = "metrodata333";

	private static string ConStr(){
		string constr = "SERVER=" + host +";DATABASE=" + dbname + ";UID=" + user + ";PASSWORD=" + passwrd + ";Pooling=true";
		return constr;
	}

	private static void Query(string query){
		try{
			MySqlConnection con = new MySqlConnection(ConStr());
			con.Open();
			MySqlCommand cmd = new MySqlCommand(query, con);
			MySqlDataReader reader = cmd.ExecuteReader();
			reader.Close();
			con.Close();
			con.Dispose();
		}
		catch(Exception ex){
			Debug.LogError(ex.ToString());
		}
	}

	public static void StoreScore(string name, int score, int level){
		Query ("INSERT INTO Scores VALUES (" + name + "," + score + "," + level + ");");
	}

	public static void Test(){
		Query("INSERT INTO Scores VALUES (derp,1,10)");
	}

}
