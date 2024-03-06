using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    Shooting shooting;
    public TextMeshProUGUI ammoText;

    void Start(){
        shooting = GameObject.FindWithTag("Player").GetComponent<Shooting>();
    }

    void Update()
    {
        ammoText.text = shooting.getAmmoCount();
    }
}
