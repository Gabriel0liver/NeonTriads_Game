using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolPickup : MonoBehaviour
{

    Shooting shooting;

    void Start(){
        shooting = GameObject.FindWithTag("Player").GetComponent<Shooting>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            shooting.pistolPickup();
            Destroy(gameObject);
            FindAnyObjectByType<AudioManager>().Play("reload");
        }
    }
}
