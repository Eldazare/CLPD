using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

	public Text scoreText;

	public void ReceiveMenuManagerInfo(bool victory){
		this.scoreText.text = ScoreManager.GetEndScoreText (victory);
		Debug.Log(ScoreManager.GetEndScoreText(victory));
		AudioSource endSource = GetComponent<AudioSource> ();
		if (victory) {
			endSource.clip = (AudioClip)Resources.Load ("Sounds/Ending/Victory");
		} else {
			endSource.clip = (AudioClip)Resources.Load ("Sounds/Ending/Defeat");
		}
		endSource.Play ();
		StartCoroutine(DatabaseManager.StoreScore(ScoreManager.playerName,ScoreManager.currentScore,ScoreManager.level));
	}

	public void RestartButton(){
		StartCoroutine (ReturnToInitialScene());
	}

	private IEnumerator ReturnToInitialScene(){
		AsyncOperation operation = SceneManager.LoadSceneAsync ("Initial");
		while (!operation.isDone) {
			yield return null;
		}
	}
}
