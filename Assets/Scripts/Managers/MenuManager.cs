using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public delegate void AddListenerDeleg (string name);

	public GameObject bulletPrefab;
	public GameObject buttonPrefab;
	public GameObject playerPrefab;

	private string playerName;
	private string gun1Name = null;
	private string gun2Name = null;
	private string consumableName = null;
	private string armorName = null;
	private string className = null;

	public GameObject levelCenterPanel;
	public GameObject equipmentMenu;
	public GameObject assaultMenu;
	public GameObject glMenu;
	public GameObject lmgMenu;
	public GameObject pistolMenu;
	public GameObject shotgunMenu;
	public GameObject smgMenu;
	public GameObject sniperMenu;
	public GameObject consumableMenu;
	public GameObject armorMenu;
	public GameObject classMenu;
	public Text scoreText;


	private int currentWpnNr;
	private LevelManager currentLvl;

	private bool battleSceneActive = false;
	private float battleTimer = 0;


	// TODO: Joining game (button etc.) multiplayer

	// TODO: How to play .txt

	// TODO: Feature documentation .txt

	// TODO: Invunerability on hit?

	// TODO: Play game to generate highscore

	// TODO: Move to text files: Movement class attributes && ScoreManager data && Explosion radius stuff

	// TODO: UI: Later maybe consumable CD indicator if consumables get implemented.

	void Awake(){
		DontDestroyOnLoad (this.transform.gameObject);
		SoundManager.Initialize (this.gameObject);
		List<string> nameData = DataManager.GetNameList ();
		foreach (string s in nameData) {
			CreateEquipmentButton (s);
		}
		WeaponCreator.bulletPrefab = bulletPrefab;
		SoundManager.PlayMusic ("menuTheme1");

		//DatabaseTest
		//StartCoroutine(DatabaseManager.StoreScore("Derp",1,10));
	}

	void Update(){
		if (battleSceneActive) {
			battleTimer += Time.deltaTime;
		}
	}

	public void GetScoresFrmDTBS(){
		StartCoroutine (WaitScoresFrmDTBS ());
	}

	private IEnumerator WaitScoresFrmDTBS(){
		yield return DatabaseManager.LoadScores ();
		scoreText.text = DatabaseManager.GetScores ();
	}

	public void GunChoice(int nr){
		currentWpnNr = nr;
	}

	public void ReceivePlayerName(string name){
		Debug.Log ("Received name " + name);
		this.playerName = name;
		//ScoreManager.playerName = name;
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
		//ScoreManager.level = int.Parse(name);
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
		battleSceneActive = true;
		yield return currentLvl.WaveLoop ();
		StartCoroutine(GameEnd(true));
	}

	public void PrematureGameEnd(bool victory){
		StartCoroutine (GameEnd (victory));
	}

	private IEnumerator GameEnd(bool victory){
		battleSceneActive = false;
		if (victory) {
			ScoreManager.ClearBonus(currentLvl.difficulty, battleTimer);
		}
		AsyncOperation operation = SceneManager.LoadSceneAsync ("EndScene");
		while (!operation.isDone) {
			yield return null;
		}
		GameObject.FindGameObjectWithTag("EndManager").GetComponent<EndManager>().ReceiveMenuManagerInfo(victory);
		Destroy (this.gameObject);
		Debug.Log ("GameEnded");
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
			currentMenu = glMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "assault":
			currentMenu = assaultMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "smg":
			currentMenu = smgMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "shotgun":
			currentMenu = shotgunMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "lmg":
			currentMenu = lmgMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "sniper":
			currentMenu = sniperMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "pistol":
			currentMenu = pistolMenu;
			listenerMethod = this.ReceiveGun;
			break;
		case "consumable":
			currentMenu = consumableMenu;
			listenerMethod = this.ReceiveConsumable;
			break;
		case "class":
			currentMenu = classMenu;
			listenerMethod = this.ReceiveClass;
			break;
		case "armor":
			currentMenu = armorMenu;
			listenerMethod = this.ReceiveArmor;
			break;
		case "level":
			currentMenu = levelCenterPanel;
			listenerMethod = this.ReceiveLevel;
			addEquipReturn = false;
			buttonText = dictionaryName[0]+"-"+dictionaryName[1];
			SoundManager.AddLevelMusic(dictionaryName);
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
			btn.onClick.AddListener (delegate{equipmentMenu.SetActive (true);});
		}
		Debug.Log ("Created button " + buttonText);
	}
}
