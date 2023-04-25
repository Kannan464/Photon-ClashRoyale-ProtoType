using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiPlayerSetting : MonoBehaviour
{
    public static MultiPlayerSetting multiplayerSettings;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;
    PlayerUI _target;

    public void Awake()
    {
        if (MultiPlayerSetting.multiplayerSettings == null)
        {
            MultiPlayerSetting.multiplayerSettings = this;
        }
        else
        {
            if (MultiPlayerSetting.multiplayerSettings != null)
                
                Destroy(this.gameObject);
            }
     DontDestroyOnLoad(this.gameObject);
    }
}