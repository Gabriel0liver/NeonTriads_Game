using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject bulletShotGunPrefab;
    
    Player playerScript;

    public float bulletSpeed = 20f;
    public float shotDelay = 0.1f;
    public float shotgunDelay = 1.5f;
   
    private int ammo = 15;
    private int magCapacity = 15;
    private bool firing = false;
    private bool released = true;
    private float lastFireTime;
    private Gun gun = Gun.Pistol;

    public enum Gun
    {
        Pistol,
        MachineGun,
        ShotGun
    }


    void Start(){
        playerScript = GetComponent<Player>();
    }

    void Update()
    {

        if(!firing){
            released = true;
        }

        if(ammo > 0 && playerScript.isAlive()){
            switch(gun){
            case Gun.Pistol:
                if (firing && released)
                {
                    float timeSinceLastFire = Time.time - lastFireTime;
                    if(timeSinceLastFire >= shotDelay){
                        Shoot();
                        CinemachineShake.Instance.ShakeCamera(4f,.15f);
                        lastFireTime = Time.time;
                        released = false;
                    }  
                }
            break;
            case Gun.MachineGun:
                if (firing)
                {
                    float timeSinceLastFire = Time.time - lastFireTime;
                    if(timeSinceLastFire >= shotDelay){
                        Shoot();
                        CinemachineShake.Instance.ShakeCamera(5f,.15f);
                        lastFireTime = Time.time;
                    }  
                }
            break;
            case Gun.ShotGun:
            if (firing && released)
                {
                    float timeSinceLastFire = Time.time - lastFireTime;
                    if(timeSinceLastFire >= shotgunDelay){
                        ShootSpread();
                        CinemachineShake.Instance.ShakeCamera(7f,.15f);
                        lastFireTime = Time.time;
                        released = false;
                    }  
                }
            break;
        }
        }

    }

    private void OnFire(InputValue inputValue){
        firing = inputValue.isPressed;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = bulletSpeed * transform.up;
        ammo --;

        //FindAnyObjectByType<AudioManager>().Play("shooting1");
    }

    void ShootSpread()
    {   
        GameObject bullet1 = Instantiate(bulletShotGunPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        GameObject bullet2 = Instantiate(bulletShotGunPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 1));
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
        GameObject bullet3 = Instantiate(bulletShotGunPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 2));
        Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
        GameObject bullet4 = Instantiate(bulletShotGunPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -1));
        Rigidbody2D rb4 = bullet4.GetComponent<Rigidbody2D>();
        GameObject bullet5 = Instantiate(bulletShotGunPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -2));
        Rigidbody2D rb5 = bullet5.GetComponent<Rigidbody2D>();

        
        rb1.velocity = bulletSpeed * transform.up;
        rb2.velocity = bulletSpeed * (Quaternion.Euler(0, 0, 2)*transform.up);
        rb3.velocity = bulletSpeed * (Quaternion.Euler(0, 0, 4)*transform.up);
        rb4.velocity = bulletSpeed * (Quaternion.Euler(0, 0, -2)*transform.up);
        rb5.velocity = bulletSpeed * (Quaternion.Euler(0, 0, -4)*transform.up);

        FindAnyObjectByType<AudioManager>().Play("shotgun2");

        ammo --;
    }

    public void pistolPickup(){
        gun = Gun.Pistol;
        ammo = 15;
        magCapacity = 15;
    }

    public void machinegunPickup(){
        gun = Gun.MachineGun;
        ammo = 30;
        magCapacity = 30;
    }

    public void shotgunPickup(){
        gun = Gun.ShotGun;
        ammo = 10;
        magCapacity = 10;
    }

    public String getAmmoCount(){
        return ammo.ToString() + "/" + magCapacity.ToString();
    }
}
