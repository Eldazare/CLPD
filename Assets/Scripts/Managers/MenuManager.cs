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
	private string lvlname = null;

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

	private PhotonView photonView;

	private int currentWpnNr;
	private LevelManager currentLvl;

	private bool battleSceneActive = false;
	private float battleTimer = 0;

	//MasterClientData
	private int levelLoadsReceived = 0;

	// TODO: How to play .txt

	// TODO: Feature documentation .txt

	// TODO: Distinct own and other player bullet colors

	// TODO: Invunerability on hit?

	// TODO: Move to text files: Movement class attributes && Explosion radius stuff

	// TODO: UI: Later maybe consumable CD indicator if consumables get implemented.

	void Awake(){
		Screen.SetResolution (1024, 768, false, 60);
		DontDestroyOnLoad (this.transform.gameObject);
		SoundManager.Initialize (this.gameObject);
		List<string> nameData = DataManager.GetNameList ();
		foreach (string s in nameData) {
			CreateEquipmentButton (s);
		}
		WeaponCreator.bulletPrefab = bulletPrefab;
		SoundManager.PlayMusic ("menuTheme1");
		photonView = gameObject.GetComponent<PhotonView> ();

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
		lvlname = name;
		//ScoreManager.level = int.Parse(name);
	}

	public GameObject SpawnPlayer(){
		GameObject playerGO = PhotonNetwork.Instantiate (playerPrefab.name, new Vector3 (0, 0, 0), Quaternion.identity, 0) as GameObject;
		playerGO.GetComponent<PlayerBody> ().CallDonEverything(className,gun1Name,gun2Name,armorName,consumableName);
		return playerGO;
	}
		
	public void GameStart(){
		// Called from start button in final menu
		if (PhotonNetwork.room != null) {
			PhotonNetwork.room.IsOpen = false;
			PhotonNetwork.room.IsVisible = false;
			photonView.RPC ("GameStartRPC", PhotonTargets.All, lvlname);
		}
	}

	[PunRPC]
	public void GameStartRPC(string lvlName){
		Debug.Log ("GameStart RPC was called.");
		if(PhotonNetwork.isMasterClient){
			Debug.Log ("IS MASTER!");
		}
		StartCoroutine(GameWait(lvlName));
	}

	// IMPORTANT to wait for scene load
	private IEnumerator GameWait(string lvlName){
		this.lvlname = lvlName;
		Debug.Log ("Gamewait was called");
		currentLvl = new LevelManager (lvlName);
		ScoreManager.Initialize ();
		ScoreManager.ReceivePlayerName (playerName);
		AsyncOperation operation = SceneManager.LoadSceneAsync ("GameLayout0", LoadSceneMode.Single);
		while (!operation.isDone) {
			yield return null;
		}
		photonView.RPC ("ReportGameLoad",PhotonTargets.MasterClient, null);
		if (PhotonNetwork.isMasterClient) {
			while (levelLoadsReceived < PhotonNetwork.room.PlayerCount) {
				yield return null;
			}
			photonView.RPC ("AfterGameLoad", PhotonTargets.All, null);
		}
	}

	[PunRPC]
	private void AfterGameLoad(){
		SoundManager.PlayMusic ("battleBGM" + lvlname);
		GameObject playerGO = SpawnPlayer ();
		GameObject.FindGameObjectWithTag ("UIManager").GetComponent<PlayerUIManager> ().Initialize (playerGO.GetComponent<PlayerBody> (), currentLvl);
		battleSceneActive = true;
		if (PhotonNetwork.isMasterClient) {
			StartCoroutine (MasterGameWaitEnd ());
		}
	}

	private IEnumerator MasterGameWaitEnd(){
		yield return currentLvl.WaveLoop ();
		StartCoroutine (GameEnd (true));
	}

	[PunRPC]
	private void ReportGameLoad(){
		levelLoadsReceived += 1;
	}


	[PunRPC]
	public void PrematureGameEnd(bool victory){
		StartCoroutine (GameEnd (victory));
	}
		

	private IEnumerator GameEnd(bool victory){
		battleSceneActive = false;
		if (victory) {
			ScoreManager.ClearBonus(currentLvl.difficulty, battleTimer);
		}
		photonView.RPC ("PrematureGameEnd", PhotonTargets.Others, victory);

		AsyncOperation operation = SceneManager.LoadSceneAsync ("EndScene");
		while (!operation.isDone) {
			yield return null;
		}
		GameObject.FindGameObjectWithTag("EndManager").GetComponent<EndManager>().ReceiveMenuManagerInfo(victory);
		GameObject.FindGameObjectWithTag ("PunManager").GetComponent<PUNManager> ().LeaveRoom ();
		Destroy(GameObject.FindGameObjectWithTag("PunManager"));
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
