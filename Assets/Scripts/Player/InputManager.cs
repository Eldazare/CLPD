using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	// REMEMBER TO CHECK FOR !RELOADING (bool) in PlayerBody
	PlayerBody playerBody;
	GameObject pauseMenu;
	IPickup potentialPickup = null;
	GameObject pickupObject = null;

	//Keycodes for shorter code down the line
	KeyCode Left = KeyCode.A;
	KeyCode Right = KeyCode.D;
	KeyCode Up = KeyCode.W;
	KeyCode Down = KeyCode.S;

	void Start(){
		playerBody = this.GetComponent<PlayerBody> ();
		pauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu");
		pauseMenu.SetActive (false);
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.CompareTag ("Pickup")) {
			potentialPickup = other.GetComponent<IPickup> ();
			pickupObject = other.gameObject;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag ("Pickup")) {
			potentialPickup = null;
		}
	}

	void Update(){
		if (!playerBody.reloading || paused) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				playerBody.SwapWeapon ();
			} else if (Input.GetKeyDown (KeyCode.E)) {
				if (potentialPickup != null) {
					playerBody.PickupItem (potentialPickup);
					Destroy (pickupObject);
				}
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				StartCoroutine (playerBody.Reload ());
			} else if (Input.GetMouseButton (1)) {
				playerBody.UseConsumable ();
			} else if (Input.GetMouseButton (0)) {
				if (!playerBody.ShootWeapon ()) {
					playerBody.Reload (); // AUTORELOAD
				}
			}
		} 
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseGame ();
		}
	}

	public Vector2 GetMovementInput(){
		Vector2 dir = new Vector2 (0, 0);
		if (IsDown (Left) && IsDown (Right)) {
			
		} else if (IsDown (Left)) {
			dir.x = -1;
		} else if (IsDown (Right)) {
			dir.x = 1;
		}
		if (IsDown (Up) && IsDown (Down)) {

		} else if (IsDown (Down)) {
			dir.y = -1;
		} else if (IsDown (Up)) {
			dir.y = 1;
		}
		if (dir.magnitude != 0) {
			dir.Normalize ();
		}
		return dir;
	}

	private bool IsDown(KeyCode button){
		if (Input.GetKey (button)) {
			return true;
		} else {
			return false;
		}
	}
		
	private bool paused = false;
	private void PauseGame(){
		if (!paused) {
			Time.timeScale = 0;
			paused = true;
			pauseMenu.SetActive (true);
			SoundManager.bgmSource.Pause ();
		} else {
			Time.timeScale = 1;
			paused = false;
			pauseMenu.SetActive (false);
			SoundManager.bgmSource.UnPause ();
		}
	}
}
