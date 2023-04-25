using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParrelSync;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    [SerializeField]
    public GameState gameState = new GameState();
    public GameObject gameHUD;
    public bool isFirstPlayer;
    public GameObject mainCam;
    public GameObject playerprefab;
    public PlayerUI[] playerUI;
    public PlayerManager[] players;
    public PlayerManager myPlayer;
    bool isGameStart;
    private void OnDisable()
    {
        base.OnDisable();
        instance=null;
    }

    private void OnEnable()
    {
        base.OnEnable();
        gameState=new GameState();
        instance=this;
        PlayerState ps = new PlayerState();

        ps.PlayerID = PhotonManager.instance.playerId;
        object[] obj = new object[1];
        obj[0] =JsonUtility.ToJson(ps);
        PhotonManager.instance.RPC_CreatePlayer(obj);
    } 
    public void AddPlayersToGame(string PlayerStatejson)
    {
        if (gameState.player1 == null || string.IsNullOrEmpty(gameState.player1.PlayerID))
        {
            PlayerState ps = new PlayerState();
            ps = JsonUtility.FromJson<PlayerState>(PlayerStatejson);
            ps.PlayerHealth = 100;
            ps.ElixirAmount = 0;
            playerUI[0].Player1Elixir.text= ps.ElixirAmount.ToString();
            gameState.player1 = ps;
        }
        else if (gameState.player2 == null || string.IsNullOrEmpty(gameState.player2.PlayerID))
        {
            PlayerState ps = new PlayerState();
            ps = JsonUtility.FromJson<PlayerState>(PlayerStatejson);
            ps.PlayerHealth = 100;
            ps.ElixirAmount = 0;
            playerUI[1].Player1Elixir.text =ps.ElixirAmount.ToString();
            gameState.player2 = ps;
        }

        if(gameState.player1 != null && gameState.player2 != null && (!string.IsNullOrEmpty(gameState.player2.PlayerID) && !string.IsNullOrEmpty(gameState.player1.PlayerID)))
        {
            gameState.CurrentState = 1;
        }

        UpdateGameStateToServer();

    }

    
    public void UpdateGameStateToServer()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["GameState"].ToString() != JsonUtility.ToJson(gameState))
            {
           
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { 
                    {
                        "GameState", JsonUtility.ToJson(gameState) 
                    }
                });
                UpdateNetworkToGame();
            }
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.IsMasterClient)
            return;
    
        if (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] != null)
        {
            gameState = JsonUtility.FromJson<GameState>(PhotonNetwork.CurrentRoom.CustomProperties["GameState"].ToString());
            UpdateNetworkToGame();
        }
    }
    public void UpdateNetworkToGame()
    {
        switch (gameState.CurrentState)
        {
            case 0:
                gameHUD.SetActive(false);
                isGameStart = false;
                break;

            case 1:
                gameHUD.SetActive(true);
                if (isGameStart)
                {
                    for(int i = 0; i < 2; i++)
                    {
                        //StartCoroutine(nameof(PlayerManager.ELixerLoader));
                    }
                    isGameStart = true;
                }
                break;

            case 2:
                isGameStart = false;
                gameHUD.SetActive(false);
                break;
        }
    }
}