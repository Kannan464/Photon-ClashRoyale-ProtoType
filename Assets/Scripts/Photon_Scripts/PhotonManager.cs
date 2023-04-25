using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using System;


public class PhotonManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonManager instance;
    private PhotonView PV;
    public bool isGameLoaded;
    public int currentScene;
    //Player Info
    Player[] photonPlayer;
    public int playerInRoom;
    public int myNumberInRoom;
    public int playerInGame;
    //Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayer;
    private float atMaxPlayer;
    private float timeToStart;

    public string playerId;
    private void Awake()
    {
     
        playerId= UnityEngine.SystemInfo.deviceUniqueIdentifier + ParrelSync.ClonesManager.GetArgument();
        //set up singleton
        if (PhotonManager.instance == null)
        {
            PhotonManager.instance = this;
        }
        else
        {
            if (PhotonManager.instance != null)
            {
                Destroy(PhotonManager.instance.gameObject);
                PhotonManager.instance = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
     
    }


    public override void OnEnable()
    {
        //subcribe to function
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }  
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    void Start()
    {
        //set private variable
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayer = startingTime;
        atMaxPlayer = 2;
        timeToStart = startingTime;
        //if (GameManager.instance.playerprefab != null)
        //{
        //    GameObject _uiGo = Instantiate(GameManager.instance.playerprefab, canvasParent) as GameObject;
        //    _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        //}
        //else
        //{
        //    Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        //}
    }
   
    void Update()
    {
        //for delay start only,count down to start
        if (MultiPlayerSetting.multiplayerSettings.delayStart)
        {
            if (playerInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayer -= Time.deltaTime;
                    lessThanMaxPlayer = atMaxPlayer;
                    timeToStart = atMaxPlayer;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayer -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayer;
                }
               // Debug.LogWarning("Display time to start to the players" + timeToStart);
                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }
    public override void OnJoinedRoom()
    {
        //set player data when we join the room
        base.OnJoinedRoom();
        Debug.Log("<color=FF0028>We are in a Room</color>");
        photonPlayer = PhotonNetwork.PlayerList;
        playerInRoom = photonPlayer.Length;
        myNumberInRoom = playerInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        //for delay start only
        if (MultiPlayerSetting.multiplayerSettings.delayStart)
        {
            Debug.Log("<color=red>Display player in room out of max players possiable(" + playerInRoom + "." + MultiPlayerSetting.multiplayerSettings.maxPlayers + "</color>)");
            if (playerInRoom > 1)
            {
                readyToCount = true;
            }
            if (playerInRoom == MultiPlayerSetting.multiplayerSettings.maxPlayers)
            {
                UIHandlers.instance.mainMenu.SetActive(false);
                UIHandlers.instance.inGame.SetActive(true);
                readyToCount = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            //photonPlayer = PhotonNetwork.PlayerList;
            //for (int i = 0; i < photonPlayer.Length; i++)
            //{
            //    photonView.RPC("RPCStartGame", photonPlayer[i], GameSetup.instance.spawnPoints[i].position, GameSetup.instance.spawnPoints[i].rotation);
            //}
        }
        else
        {
            StartGame();
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("<color=#FF0028>A new player has joined the room</color>");
        photonPlayer = PhotonNetwork.PlayerList;
        playerInRoom++;
        if (MultiPlayerSetting.multiplayerSettings.delayStart)
        {
            Debug.Log("<color=red>Display player in room out of max player possible (" + playerInRoom + ":" + MultiPlayerSetting.multiplayerSettings.maxPlayers + "</color>)");
            if (playerInRoom > 1)
            {
                readyToCount = true;
            }
            if (playerInRoom == MultiPlayerSetting.multiplayerSettings.maxPlayers)
            {
                UIHandlers.instance.mainMenu.SetActive(false);
                UIHandlers.instance.inGame.SetActive(true);
                readyToCount = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
    void StartGame()
    {
        
         isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (MultiPlayerSetting.multiplayerSettings.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiPlayerSetting.multiplayerSettings.multiplayerScene);
    }

    //[PunRPC]
    //void RPCStartGame(Vector3 spawnPos, Quaternion spawnRot)
    //{
    //    GameSetup.instance.LobbyCam.SetActive(false);
    //    PhotonNetwork.Instantiate("PhotonNetworkPlayer", spawnPos, spawnRot, 0);
    //}
void RestartTimer()
    {
        //restarts the time for when players leave the room(DelayStart)
        lessThanMaxPlayer = startingTime;
        timeToStart = startingTime;
        atMaxPlayer = 2;
        readyToCount = false;
        readyToStart = false;
    }
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if (currentScene == MultiPlayerSetting.multiplayerSettings.multiplayerScene)
        {
            isGameLoaded = true;
          //  //for delay start game
          //  if (MultiPlayerSetting.multiplayerSettings.delayStart)
          //      {
          //          PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
          //          PV.RPC("RPCStartGame", RpcTarget.MasterClient);
          //      }
          ////  for non delay start game
          //  else
          //          {
          //              RPC_CreatePlayer();
          //          }
        }
    }
    //public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    //{
    //    SetElixirText();
    //}
    //void SetElixirText()
    //{
    //    if (PhotonNetwork.CurrentRoom.CustomProperties["Player1Elixir"] != null)
    //    {
    //        PlayerUI.instance.Player1Elixir.text = PhotonNetwork.CurrentRoom.CustomProperties["Player1Elixir"].ToString();
    //    }
    //    if (PhotonNetwork.CurrentRoom.CustomProperties["Player2Elixir"] != null)
    //    {
    //        PlayerUI.instance.Player2Elixir.text = PhotonNetwork.CurrentRoom.CustomProperties["Player2Elixir"].ToString();
            
    //    }
    //}

    //[PunRPC]
    //private void RPC_LoadedGameScene()
    //{
    //    playerInGame++;
    //    if (playerInGame == PhotonNetwork.PlayerList.Length)
    //    {
    //        PV.RPC("RPC_CreatePlayer", RpcTarget.MasterClient);
    //        //GameObject _uiGo = Instantiate(MultiPlayerSetting.multiplayerSettings.PlayerUiPrefab,canvasParent) as GameObject;
    //        //_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    //    }
    //}
    [PunRPC]
    public void RPC_CreatePlayer(object[] obj)
    {
        //   PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerUI"), transform.position, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity, 0,obj);
    }  
}