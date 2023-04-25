using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
{
    public PlayerState playerState;
    public PlayerUI myUI;
    public static PlayerManager localPlayer;
    public static PlayerManager opponentPlayer;
    public float currentElixir;public int currentHealth;
    bool isLocal;
   
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        playerState = JsonUtility.FromJson<PlayerState>(info.photonView.InstantiationData[0].ToString());
        if (photonView.IsMine)
        {
            isLocal = true;
            localPlayer = this;
            GameManager.instance.myPlayer = this;
            GameManager.instance.players[0] = this;
            GameManager.instance.playerUI[0].myPlayer= this;
            myUI = GameManager.instance.playerUI[0];
        }
        else
        {
            isLocal = false;
            opponentPlayer = this;
            GameManager.instance.players[1] = this;
            GameManager.instance.playerUI[1].myPlayer = this;
            myUI = GameManager.instance.playerUI[1];
        }
        StartCoroutine(CheckJoinStatus(photonView.IsMine));
        StartCoroutine(nameof(ELixerLoader));
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentElixir);
            stream.SendNext(currentHealth);
        }
        else if (stream.IsReading)
        {
            currentElixir = (float)stream.ReceiveNext();
            playerState.ElixirAmount = currentElixir;
            myUI.UpdateUI(playerState);
            currentHealth= (int)stream.ReceiveNext();
            playerState.PlayerHealth = currentHealth;
            myUI.UpdateHealthUI(playerState);
        }
    }
    public IEnumerator CheckJoinStatus(bool IsMine)
    {
        while(GameManager.instance==null)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (IsMine)
        {           
            photonView.RPC("AddplayerRequest",RpcTarget.MasterClient,JsonUtility.ToJson(playerState));
            bool check=true;
            while (check)
            {
                Debug.Log("" + PhotonManager.instance.playerId);
                if (GameManager.instance.gameState.player1 !=null && !string.IsNullOrEmpty(GameManager.instance.gameState.player1.PlayerID) && PhotonManager.instance.playerId==GameManager.instance.gameState.player1.PlayerID)
                {
                    check = false;
                    GameManager.instance.isFirstPlayer=true;
                    continue;
                }
                if (GameManager.instance.gameState.player2 != null && !string.IsNullOrEmpty(GameManager.instance.gameState.player2.PlayerID) && PhotonManager.instance.playerId == GameManager.instance.gameState.player2.PlayerID)
                {
                    check = false;
                    GameManager.instance.isFirstPlayer=false;
                        
                    GameManager.instance.mainCam.transform.eulerAngles = new Vector3(0, 0, 180);
                    
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    bool isElixerLoading;
    public IEnumerator ELixerLoader()
    {
        while (true)
        {
            if (isLocal && GameManager.instance.gameState.CurrentState == 1)
            {
                if (currentElixir <= 9)
                {
                    currentElixir += 1;
                    playerState.ElixirAmount = currentElixir;
                    myUI.UpdateUI(playerState);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    [PunRPC]
    private void RPC_Spawner(string playerId, float x, float y)
    {
        Spawner.instance.Spawn(playerId, new Vector3(x, y, 0));
    }
    [PunRPC]
    public void AddplayerRequest(string playerStatejson)
    {
        GameManager.instance.AddPlayersToGame(playerStatejson);
    }
    public void Update()
    {
        FightBtnClick();
    }
    public void Start()
    {
        if (!PlayerUI.instance.isMine)
        {
            currentHealth = myUI.maxHealth;
            SetMaxHealth(myUI.maxHealth);
        }
        
    }
    private void OnEnable()
    {

    }
    public void SetMaxHealth(int health)

    {
        if (!PlayerUI.instance.isMine)
        {
            myUI.slider.maxValue = health;
            myUI.slider.value = health;
            myUI.fill.color = myUI.gradient.Evaluate(1f);
        }
        //else
        //{
        //    myUI.slider.maxValue = health;
        //    myUI.slider.value = health;
        //    myUI.fill.color = myUI.gradient.Evaluate(1f);
        //}
      
    }
    public void SetHealth(int health)
    {
        if (!PlayerUI.instance.isMine)
        {
            Debug.Log(gameObject.name);
            Debug.LogError(myUI);
            Debug.LogError(myUI.slider.value);
            myUI.slider.value = health;
            myUI.fill.color = myUI.gradient.Evaluate(myUI.slider.normalizedValue);
        }
        //else
        //{
        //    myUI.slider.value = health;
        //    myUI.fill.color = myUI.gradient.Evaluate(myUI.slider.normalizedValue);
        //}
        
    }
    public void FightBtnClick()
    {
        if (!isLocal)
            return;

        if (Input.GetMouseButtonDown(0) && GameManager.instance.gameState.CurrentState == 1)
        {
            Vector2 mousepos = Input.mousePosition;
            Vector2 newpos = Camera.main.ScreenToWorldPoint(mousepos);
            if (mousepos.y <= Screen.height / 2)
            {
                if (currentElixir >= 2)
                {
                    Debug.Log("<color=yellow>Poguthu over OVER...</color>");
                    currentElixir = currentElixir - 2;
                    playerState.ElixirAmount = currentElixir;
                    myUI.UpdateUI(playerState);
                    photonView.RPC("RPC_Spawner", RpcTarget.AllViaServer, PhotonManager.instance.playerId, newpos.x, newpos.y);
                }
            }
        }
    }

}