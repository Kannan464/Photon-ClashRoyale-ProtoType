using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public void Awake()
    {
        instance = this;
    }

    public void Spawn(string playerId, Vector3 pos)
    {
        if (playerId == GameManager.instance.gameState.player1.PlayerID)
        {
            // pos=GameManager.instance.player1Spawn.transform.position;
            GameObject go = Instantiate(GameManager.instance.playerprefab, pos, new Quaternion());
            go.tag = "Spawn1";

        }
        else
        {
            //  pos=GameManager.instance.player2Spawn.transform.position;
            GameObject go = Instantiate(GameManager.instance.playerprefab, pos, new Quaternion());
            go.tag = "Spawn2";
        }
    }

}
