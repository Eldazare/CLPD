using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public delegate void AddListenerDeleg (string name);

	public GameObject uiManagerPrefab;
	public GameObject bulletPrefab;
	public GameObject buttonPrefab;
	public GameObject playerPrefab;

	private string playerName;
	private string gun1Name = null;
	private string gun2Name = null;
	private string consumableName = null;
	private string armorName = null;
	private string className = null;

	public GameObject LevelCenterPanel;
	public GameObject EquipmentMenu;
	public GameObject AssaultMenu;
	public GameObject GlMenu;
	public GameObject LMGMenu;
	public GameObject PistolMenu;
	public GameObject ShotgunMenu;
	public GameObject SMGMenu;
	public GameObject SniperMenu;
	public GameObject ConsumableMenu;
	public GameObject ArmorMenu;
	public GameObject ClassMenu;


	private int currentWpnNr;
	private LevelManager currentLvl;


	// TODO: Joining game (button etc.) multiplayer

	// TODO: Invunerability

	// TODO: Database

	// TODO: MORE ENEMY movement patterns

	// TODO: UI: Later maybe consumable CD indicator if consumables get implemented.

	void Awake(){
		DontDestroyOnLoad (this.transform.gameObject);
		List<string> nameData = DataManager.GetNameList ();
		foreach (string s in nameData) {
			CreateEquipmentButton (s);
		}
		WeaponCreator.bulletPrefab = bulletPrefab;
		DatabaseManager.Test ();
	}

	public void GunChoice(int nr){
		currentWpnNr = nr;
	}

	public void ReceivePlayerName(string name){
		this.playerName = name;
	}

	public void ReceiveGun(string name){
		switch (currentWpnNr) {
		case 1:
			gun1Name = name;
			break;
		case 2:
			gun2Name = name;
			break;
		default:
			Debug.Log ("MenuManager gun number was incorrect (probably null");
			break;
		}
	}

	public void ReceiveConsumable(string name){
		consumableName = name;
	}

	public void ReceiveArmor(string name){
		armorName = name;
	}

	public void ReceiveClass(string name){
		className = name;
	}

	//name is doubledigit
	public void ReceiveLevel(string name){
		currentLvl = new LevelManager (name);
	}

	public GameObject SpawnPlayer(){
		GameObject playerGO = Instantiate (playerPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		PlayerBody player = playerGO.GetComponent<PlayerBody> ();
		if (className != null) {
			player.Initialize (ClassCreator.CreateClass (className));
		} else {
			player.Initialize (ClassCreator.CreateClass ("default"));
		}
		if (gun1Name != null) {
			player.DonWeapon (WeaponCreator.CreateWeapon (gun1Name), 1);
		}
		if (gun2Name != null) {
			player.DonWeapon (WeaponCreator.CreateWeapon (gun2Name), 2);
		}
		if (armorName != null) {
			player.DonArmor (ArmorCreator.CreateArmor (armorName));
		}
		if (consumableName != null) {
			player.DonConsumable (WeaponCreator.CreateConsumable (consumableName));
		}
		return playerGO;
	}

	public void CreateLobby(){
		// TODO: Multiplayer stuff
	}

	public void GameStart(){
		// Called from start button in final menu
		StartCoroutine(GameWait());
	}

	// IMPORTANT to wait for scene load
	private IEnumerator GameWait(){
		ScoreManager.ReceivePlayerName (playerName);
		AsyncOperation operation = SceneManager.LoadSceneAsync ("GameLayout0", LoadSceneMode.Single);
		while (!operation.isDone) {
			yield return null;
		}
		GameObject playerGO = SpawnPlayer ();
		GameObject.FindGameObjectWithTag ("UIManager").GetComponent<PlayerUIManager> ().Initialize (playerGO.GetComponent<PlayerBody> (), currentLvl);
		yield return currentLvl.WaveLoop ();
		//GameEnd
		StartCoroutine(GameEnd(true));
	}

	public IEnumerator GameEnd(bool victory){
		AsyncOperation operation = SceneManager.LoadSceneAsync ("EndScene");
		while (!operation.isDone) {
			yield return null;
		}
		GameObject.FindGameObjectWithTag("EndManager").GetComponent<EndManager>().ReceiveMenuManagerInfo(victory);
		Destroy (this.gameObject);
	}

	private bool CheckForTakeoff(){
		if (gun1Name != null) {
			return true;
		} else {
			return false;
		}
	}


	// names are supposedly all %-lines
	private void CreateEquipmentButton(string name){
		Debug.Log ("button creation name " + name);
		GameObject currentMenu;
		AddListenerDeleg listenerMethod;
		bool addEquipReturn = true;
		string dictionaryName;
		string[] splitName = name.Split ("_"[0]);
		dictionaryName = splitName [1];
		string buttonText = dictionaryName;
		#region menuSwitch
		switch (splitName [0]) {
		case "gl":
			currentMenu = GlMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "assault":
			currentMenu = AssaultMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "smg":
			currentMenu = SMGMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "shotgun":
			currentMenu = ShotgunMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "lmg":
			currentMenu = LMGMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "sniper":
			currentMenu = SniperMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "pistol":
			currentMenu = PistolMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "consumable":
			currentMenu = ConsumableMenu;
			listenerMethod = this.ReceiveConsumable;
			break;
		case "class":
			currentMenu = ClassMenu;
			listenerMethod = this.ReceiveClass;
			break;
		case "armor":
			currentMenu = ArmorMenu;
			listenerMethod = this.ReceiveArmor;
			break;
		case "level":
			currentMenu = LevelCenterPanel;
			listenerMethod = this.ReceiveLevel;
			addEquipReturn = false;
			buttonText = dictionaryName[0]+"-"+dictionaryName[1];
			break;
		default:
			Debug.Log ("No name found while creating buttons ("+name+")");
			return;
		}
		#endregion

		GameObject newButton = Instantiate(buttonPrefab, currentMenu.transform);
		Text butText = newButton.GetComponentInChildren<Text> ();
		butText.text = buttonText;
		Button btn = newButton.GetComponent<Button> ();
		btn.onClick.AddListener (delegate{currentMenu.SetActive(false);});
		btn.onClick.AddListener (delegate{listenerMethod(dictionaryName);});
		if (addEquipReturn) {
			btn.onClick.AddListener (delegate{EquipmentMenu.SetActive (true);});
		}
		Debug.Log ("Created button " + buttonText);
	}
}
