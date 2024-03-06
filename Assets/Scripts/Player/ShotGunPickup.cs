using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunPickup : MonoBehaviour
{

    Shooting shooting;

    void Start(){
        shooting = GameObject.FindWithTag("Player").GetComponent<Shooting>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            shooting.shotgunPickup();
            Destroy(gameObject);
            FindAnyObjectByType<AudioManager>().Play("reload");
        }
    }
}
