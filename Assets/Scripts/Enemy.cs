using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool patroler = false;
    public bool clockwise = true;
    public Color deathColor = Color.grey;
    public float shotDelay = 0.5f;
    public float shotGunDelay = 1.2f;
    public float machineGunDelay = 0.1f;
    public float bulletSpeed = 20f;
    public float combatCooldown = 5f;
    public Gun gun = Gun.Pistol;
    public GameController gameController;
    
    GameObject player;
    public Transform firePoint;
    public GameObject bulletPrefab;
    
    private bool alive = true;
    private float lastFireTime;
    private float lastFireTimeMG;
    private int machineGunShotsMax;
    private int machineGunShots = 0;
    private Vector3 lastRotation;
    private float lastTimeSinceCombat;
    private bool patrol;
    private Rigidbody2D rb;


    private SpriteRenderer sprRender;

    public LayerMask layer;

    public enum Gun
    {
        Pistol,
        MachineGun,
        ShotGun
    }

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sprRender = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        lastFireTime = Time.time;
        lastFireTimeMG = Time.time;
        machineGunShotsMax = UnityEngine.Random.Range(3, 8);
        lastRotation = transform.eulerAngles;
        patrol = patroler;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.tag == "Bullet")
        {   
            Vector2 velocity = collisionInfo.relativeVelocity;
            Destroy(collisionInfo.gameObject);
            if(alive){
                Die();
            }
            rb.AddForce(velocity*5);
        }
    }

    void Die()
    {
        sprRender.color = deathColor;
        alive = false;
        FindAnyObjectByType<AudioManager>().Play("hit1");
        FindAnyObjectByType<AudioManager>().Play("hit2");
        gameController.enemyDeath();
    }

    void Update(){
        if(alive){
            if(patrol){
                movement();
            }
            scan();
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
            if(hitPlayer.collider.gameObject.tag == "Player" && distPlayer < 15f){
                if(patrol){
                    lastRotation = transform.eulerAngles;
                }
                patrol = false;
                lastTimeSinceCombat = Time.time;

                Vector3 playerPos = player.transform.position;
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((playerPos.y - transform.position.y), (playerPos.x - transform.position.x)) * Mathf.Rad2Deg - 90);
                shoot();
            }
        }
        float timeSinceCombat = Time.time - lastTimeSinceCombat;
        if(timeSinceCombat >= combatCooldown){
            transform.eulerAngles = lastRotation;
            if(patroler){
                patrol = true;
            }
        }
    }

    void shoot(){
        switch(gun){
            case Gun.Pistol:
                {
                    float timeSinceLastFire = Time.time - lastFireTime;
                    if(timeSinceLastFire >= shotDelay){
                        shootPistol();
                        lastFireTime = Time.time;
                    }  
                }
            break;
            case Gun.MachineGun:
                {
                    float timeSinceLastFire = Time.time - lastFireTime;
                    if(timeSinceLastFire >= shotDelay){
                        shootMachineGun();
                    }  
                }
            break;
            case Gun.ShotGun:
                {
                    float timeSinceLastFire = Time.time - lastFireTime;
                    if(timeSinceLastFire >= shotGunDelay){
                        shootShotGun();
                        lastFireTime = Time.time;
                    }  
                }
            break;
        }
    }

    void shootPistol(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = bulletSpeed * transform.up;
        //FindAnyObjectByType<AudioManager>().Play("shooting1");
    }

    void shootMachineGun(){
        if(machineGunShots < machineGunShotsMax){
            float timeSinceLastFire = Time.time - lastFireTimeMG;
                    if(timeSinceLastFire >= machineGunDelay){
                        shootPistol();

                        machineGunShots ++;
                        lastFireTimeMG = Time.time;
                    }  
        }else{
            machineGunShots = 0;
            machineGunShotsMax = UnityEngine.Random.Range(3, 8);
            lastFireTime = Time.time;
        }
    }

    void shootShotGun(){
        GameObject bullet1 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        GameObject bullet2 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 1));
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
        GameObject bullet3 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 2));
        Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
        GameObject bullet4 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -1));
        Rigidbody2D rb4 = bullet4.GetComponent<Rigidbody2D>();
        GameObject bullet5 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -2));
        Rigidbody2D rb5 = bullet5.GetComponent<Rigidbody2D>();

        
        rb1.velocity = bulletSpeed * transform.up;
        rb2.velocity = bulletSpeed * (Quaternion.Euler(0, 0, 1)*transform.up);
        rb3.velocity = bulletSpeed * (Quaternion.Euler(0, 0, 2)*transform.up);
        rb4.velocity = bulletSpeed * (Quaternion.Euler(0, 0, -1)*transform.up);
        rb5.velocity = bulletSpeed * (Quaternion.Euler(0, 0, -2)*transform.up);
    }

    public bool isAlive(){
        return alive;
    }
}
