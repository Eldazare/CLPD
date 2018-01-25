using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUNManager : Photon.PunBehaviour {

	public Text playerOnRoomText;
	public GameObject startGameButton;
	public GameObject roomJoinedWaitingPanel;
	public GameObject roomListPanel;
	public GameObject roomListRightPanel;
	public GameObject roomButtonPrefab;

	public byte maxPlayersPerRoom = 4;
	public string gameVersion = "1";

	private List<GameObject> roomButtonList;
	private string creationRoomName;
	private bool clientStatus = false;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
		PhotonNetwork.autoJoinLobby = true;
		Connect ();
	}

	// EXTERNAL TODOLIST HERE:
	// TODO: 

	public void Connect() {
		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.connected) {

		}
		else {
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
	}

	public void GetRooms(){
		if (roomButtonList != null) {
			foreach (GameObject go in roomButtonList) {
				Destroy (go);
			}
		}
		if (PhotonNetwork.connected) {
			roomButtonList = new List<GameObject>{ };
			foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
				string roomName = room.Name;
				GameObject newButton = Instantiate(roomButtonPrefab, roomListRightPanel.transform);
				Text butText = newButton.GetComponentInChildren<Text> ();
				butText.text = roomName;
				Button btn = newButton.GetComponent<Button> ();
				btn.onClick.AddListener (delegate{JoinRoom(roomName);});
				btn.onClick.AddListener (delegate{roomListPanel.SetActive (false);});
				btn.onClick.AddListener (delegate{SetClientStatus(true);});
				roomButtonList.Add(newButton);
			}
		} else {
			Debug.Log ("NOT CONNECTED, CAN'T GET ROOMS");
		}
	}

	public void SetClientStatus(bool clientStatus){
		this.clientStatus = clientStatus;
	}

	public void JoinRoom(string roomName){
		if (PhotonNetwork.JoinRoom (roomName)) {
		} else {
			// TODO: what do when room is full / not open
			roomListPanel.SetActive(true);
		}
	}

	public void LeaveRoom(){
		if (PhotonNetwork.room != null) {
			PhotonNetwork.LeaveRoom ();
		}
	}

	public void ReceiveRoomName(string roomName) {
		this.creationRoomName = roomName;
	}


	public void CreateRoom() {
		if (this.creationRoomName != null){
			PhotonNetwork.CreateRoom(creationRoomName, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null);
		} else {
			Debug.Log ("Room name is empty, can't create room");
		}
	}

	public override void OnConnectedToMaster() {
		Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
	}


	public override void OnDisconnectedFromPhoton() {
		Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");        
	}

	public override void OnJoinedRoom() {
		Debug.Log("Joined room");
		if (clientStatus) {
			roomJoinedWaitingPanel.SetActive (true);
		}
		if (PhotonNetwork.isMasterClient) {
			startGameButton.SetActive (true);
		}
		photonView.RPC ("APlayerJoined", PhotonTargets.MasterClient, null);
	}

	public void LeaveARoom(){
		if (PhotonNetwork.room != null) {
			PhotonNetwork.LeaveRoom ();
			photonView.RPC ("APlayerJoined", PhotonTargets.MasterClient, null);
		} else {
			
		}
	}

	//only call on masterClient
	// Only updates a piece of text
	[PunRPC]
	private void APlayerJoined(){
		playerOnRoomText.text = PhotonNetwork.playerList.Length + " player(s) in the room. \n(Including you)\nMaximum number allowed = 4";
	}





	public void UpdateScoresOnly(int enemyType){
		ScoreManager.EnemyKilled (enemyType);
	}
}
