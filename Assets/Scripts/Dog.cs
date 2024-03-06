using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public bool patroler = false;
    public bool clockwise = true;
    public Color deathColor = Color.grey;
    public GameController gameController;
    
    AIPath chaseScript;
    GameObject player;
    
    private bool alive = true;
    private bool patrol;
    private bool chase;
    private Rigidbody2D rb;

    private SpriteRenderer sprRender;

    public LayerMask layer;


    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sprRender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        patrol = patroler;
        chase = false;
        GetComponent<AIDestinationSetter>().target = player.transform;
        chaseScript = GetComponent<AIPath>();
        chaseScript.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.tag == "Bullet")
        {
            Vector2 velocity = collisionInfo.relativeVelocity;
            if(alive){
                Die();
            }
            Destroy(collisionInfo.gameObject); 
            rb.AddForce(velocity*20);
        }
    }

    void Die()
    {
        sprRender.color = deathColor;
        alive = false;
        FindAnyObjectByType<AudioManager>().Play("hit1");
        FindAnyObjectByType<AudioManager>().Play("hit2");
        chaseOff();
        gameController.enemyDeath();
    }

    void Update(){
        if(alive){
            if(patrol){
                movement();
            }
            if(!chase){
                scan();
            }
        }
    }

    void movement(){
        Vector3 fw = transform.TransformDirection(Vector3.up);
        RaycastHit2D hitWall = Physics2D.Raycast(new Vector2(this.transform.position.x,this.transform.position.y),new Vector2(fw.x,fw.y),2.0f,layer);

        Debug.DrawRay(new Vector2(this.transform.position.x,this.transform.position.y),new Vector2(fw.x,fw.y),Color.red);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * 2.0f;

        if(hitWall.collider != null){
            if(hitWall.collider.gameObject.tag == "Wall"){ 
                if(clockwise){
                    transform.Rotate(0,0,-90);
                }else{
                    transform.Rotate(0,0,-90);
                }
            }
        }
    }

    void scan(){
        float distPlayer = Vector3.Distance(player.transform.position,this.transform.position);
        Vector3 dirPlayer = player.transform.position - transform.position;
        RaycastHit2D hitPlayer = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), new Vector2(dirPlayer.x,dirPlayer.y), distPlayer, layer);
        
        if(hitPlayer.collider != null){
            if(hitPlayer.collider.gameObject.tag == "Player" && distPlayer < 30f){
                patrol = false;
                chaseOn();
            }
        }
        
    }

    void chaseOn(){
        chaseScript.enabled = true;
    }

    void chaseOff(){
        chaseScript.enabled = false;
    }

    public bool isAlive(){
        return alive;
    }
}
