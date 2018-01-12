using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

	public Text scoreText;

	public void ReceiveMenuManagerInfo(bool victory){
		this.scoreText.text = ScoreManager.GetEndScoreText (victory);
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
