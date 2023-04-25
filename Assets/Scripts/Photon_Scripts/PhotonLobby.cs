using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{

    public static PhotonLobby lobbyInstance;

    public GameObject battleButton;

    public GameObject cancelBtn;
    public GameObject OfflineBtn;




    private void Awake()
    {
        lobbyInstance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connects to Master Photon Server.
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("<color=aqua>Player has connected to the Photon master server</color>");
        PhotonNetwork.AutomaticallySyncScene = true;
        OfflineBtn.SetActive(false);
        battleButton.SetActive(true); // Player is now connected to servers, enables battle button to allow join a game.
    }
    public void OnBattleButtonClick()
    {
        Debug.Log("<color=yellow>Battle Button was click</color>");
        battleButton.SetActive(false);
        cancelBtn.SetActive(true);
        PhotonNetwork.JoinRandomRoom();// trying to join a random room.
    }
    public override void OnJoinRandomFailed(short returnCode, string message) // when randomroom fails this function will be called.
    {
        Debug.LogError("<color=red>Tried to join a random room but failed , There is no available room</color>");
        CreateRoom();
    }
    void CreateRoom() // trying to create a room that does not exist.
    {
        Debug.Log("<color=yellow>Trying to create a new room</color>");
        int randomRoomRange = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiPlayerSetting.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomRange, roomOps); // trying to create a room with the specified values.

        // Custom Room Properties

        Debug.Log("<color=yellow>Custom Room Properties Working...</color>");

        ExitGames.Client.Photon.Hashtable CusRoomProperties = new ExitGames.Client.Photon.Hashtable();
        CusRoomProperties.Add("GameState", JsonUtility.ToJson(new GameState()));
        roomOps.CustomRoomProperties= CusRoomProperties;
    }
     public override void OnCreateRoomFailed(short returnCode, string message)// when create room fails this function will be called.
    {
        Debug.LogError("<color=red>Tried to create a new room but failed , there must already be a room with the same name</color>");
        CreateRoom(); // retrying to create a new room with a diff name.
    }
    
    public void OnCancelBtnClick()
    {
        Debug.Log("<color=#FFFFFF>Cancel Button was click</color>");
        cancelBtn.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}