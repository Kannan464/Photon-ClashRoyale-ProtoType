using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    Player[] photonPlayer;
    public Transform[] spawnPoints;
    public GameObject LobbyCam;
    PhotonView photonView;
    public static GameSetup instance;

    private void Awake()
    {
        instance= this;
    }
}