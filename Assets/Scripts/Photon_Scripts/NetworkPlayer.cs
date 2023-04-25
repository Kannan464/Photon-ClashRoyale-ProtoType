using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviourPun
{
    new public GameObject camera;


    private void Start()
    {
        if (photonView.IsMine)
        {
            Destroy(camera);
        }
    }
}
