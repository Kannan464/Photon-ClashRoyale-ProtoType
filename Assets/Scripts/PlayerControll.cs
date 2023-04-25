using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public static PlayerControll instance;
    private Rigidbody2D rb;
    private float playerSpeed;
    private Transform player,targetTransform;
    public GameObject Player2;
    public GameObject Player1;
    public void Awake()
    {
        instance= this;
        
        //this.transform.SetParent(GameObject.Find("SpawnPoint").GetComponent<Transform>(), false);  
    }
    public void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
        playerSpeed = 0.8f;

    }

    private void OnEnable()
    {
        GameObject player2val = GameObject.Find("Player2");
        Player2 = player2val;
        Player1 = GameObject.Find("Player1");
//       AssignTarget();
    }

    //void Assigntarget()
    //{
    //    if (tag == "spawn1")
    //    {
    //        targettransform = player2.transform;
    //    }
    //    else if (tag == "spawn2")
    //    {
    //        targettransform = player2.transform;
    //    }
    //}
    public void Update()
    {
        //if (targetTransform)
        //    transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, playerSpeed * Time.deltaTime);
        //else
        //    AssignTarget();
        //return;


        if (tag == "Spawn1")
        {
            Vector3 target = Player2.transform.position;
            if (GameObject.FindGameObjectsWithTag("Spawn2").Length != 0)
            {
                target = GameObject.FindGameObjectsWithTag("Spawn2")[0].transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, target , playerSpeed * Time.deltaTime);

        }
        else
        {
            Vector3 target = Player1.transform.position;
            if (GameObject.FindGameObjectsWithTag("Spawn1").Length != 0)
            {
                target = GameObject.FindGameObjectsWithTag("Spawn1")[0].transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);

        }

        //if (tag == "Spawn1" && tag == "Spawn2")
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.playerprefab.transform.position, playerSpeed * Time.deltaTime);
        //    //Destroy(GameManager.instance.playerprefab);
        //}
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player2") && this.tag == "Spawn1")
        {
           // Debug.Log("Destroy by :::: player2 tower");
            Destroy(this.gameObject);
            if (GameManager.instance.gameState.player1.PlayerID == PhotonManager.instance.playerId)
            GameManager.instance.playerUI[1].TakeDamage(10);
            else
                GameManager.instance.playerUI[0].TakeDamage(10);
        }
        else if (collision.CompareTag("Player1") && this.tag == "Spawn2")
        {
           // Debug.Log("Destroy by :::: player1 tower");
            Destroy(this.gameObject);
            if (GameManager.instance.gameState.player2.PlayerID == PhotonManager.instance.playerId)
                GameManager.instance.playerUI[1].TakeDamage(10);
            else
                GameManager.instance.playerUI[0].TakeDamage(10);
        }
        
        else if(this.tag.Contains("Spawn") && collision.tag.Contains("Spawn"))
        {

            if (this.tag == collision.tag )
            {
                return;
            }
            
            Destroy(this.gameObject);
          //  Debug.Log("Destroy by :::: bullet");
           
        }
    }
}