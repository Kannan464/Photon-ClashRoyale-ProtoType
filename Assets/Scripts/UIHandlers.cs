using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandlers : MonoBehaviour
{
    public static UIHandlers instance;
    public GameObject playerui;
    public GameObject gameOver;
    public GameObject Winner;
    public GameObject mainMenu;
    public GameObject inGame;
    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
       // mainMenu.onClick.AddListener(OnclickMainMenu);
    }

    public void OnclickMainMenu()
    {
        mainMenu.SetActive(true);
        inGame.SetActive(false);
        PhotonNetwork.LeaveRoom();  
    }
  
}
