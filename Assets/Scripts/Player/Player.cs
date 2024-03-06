using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool alive;

    public GameController controller;
    public Color deathColor = Color.grey;

    private SpriteRenderer sprRender;

    void Start(){
        alive = true;
        sprRender = GetComponent<SpriteRenderer>();
        controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo){
        if(collisionInfo.collider.tag == "Bullet"){
            die();
        }
        else if(collisionInfo.collider.tag == "Enemy"){
            if(collisionInfo.collider.GetComponent<Enemy>()){
                if(collisionInfo.collider.GetComponent<Enemy>().isAlive()){
                    die();
                }
            }else if(collisionInfo.collider.GetComponent<Dog>()){
                if(collisionInfo.collider.GetComponent<Dog>().isAlive()){
                    die();
                }
            }
        }
    }

    void die(){
        if(alive){
            alive = false;
            sprRender.color = deathColor;
            FindAnyObjectByType<AudioManager>().Play("death");
            controller.death();
        }
    }

    public bool isAlive(){
        return alive;
    }
}
