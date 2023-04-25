using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.Playables;
using Photon.Realtime;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    public PlayerManager myPlayer;
    public Slider elixerLoading;
    public float startTime;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public int maxHealth = 100;
    public int currentHealth, currentElixir;
    public Transform parent;
    public Button FightBtn;
    public GameObject playerprefab;
    public TMP_Text Player1Elixir;
    public bool isMine;

    public void Awake()
    {
        instance = this;
        startTime = int.Parse(startTime.ToString());

        this.transform.SetParent(GameObject.Find("PlayerUI").GetComponent<Transform>(), false);
        //   ElixirValue();
    }
    void Update()
    {
        //if (!isMine)
        //    startTime = PlayerManager.opponentPlayer.transform.position.x;
        //elixerLoading.value = startTime;
        //Player1Elixir.text = startTime.ToString();
        //if (this == null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
    }
    private void Start()
    {
        StartCoroutine(ELixerLoader());
        //InvokeRepeating("ELixerLoader", 0f, 1f);
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
    }
   IEnumerator ELixerLoader()
    {
        while(true)
        {
            if (isMine)
            {
                if (startTime <= 9)
                {
                    //startTime += 1;
                    //elixerLoading.value = startTime;
                    //currentElixir = (int)elixerLoading.value;
                    //PlayerManager.localPlayer.transform.position = new Vector3(elixerLoading.value, 0, 0);

                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void UpdateUI(PlayerState state)
    {
        elixerLoading.value = state.ElixirAmount;
        Player1Elixir.text = ((int)state.ElixirAmount).ToString(); ;
    }
    public void UpdateHealthUI(PlayerState state)
    {
        slider.value = state.PlayerHealth;
        slider.value = currentHealth;
    }
    public void SetMaxHealth(int health)
    {
       // myPlayer.SetMaxHealth(health);
    }
    public void SetHealth(int health)
    {
     myPlayer.SetHealth(health);   
    }
    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        SetHealth(currentHealth);
        if (GameManager.instance.playerUI[0].currentHealth == 0)
        {
            Debug.Log("<color=yellow>GAME OVER...</color>");
            UIHandlers.instance.gameOver.SetActive(true);
            UIHandlers.instance.playerui.SetActive(false);
        }
        if (GameManager.instance.playerUI[1].currentHealth == 0)
        {
            UIHandlers.instance.Winner.SetActive(true);
            UIHandlers.instance.playerui.SetActive(false);
        }
     // myPlayer.myUI.UpdateHealthUI(PlayerState state);
        //else
        //{
        //    if (GameManager.instance.player1UI.maxHealth >= 0)
        //    {
        //        UIHandlers.instance.Winner.SetActive(true);
        //    }
        //}
    }
}