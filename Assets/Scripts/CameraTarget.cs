using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraTarget : MonoBehaviour
{   
    [SerializeField] Camera cam;
    [SerializeField] Transform player;
    [SerializeField] float threshold = 15;

    bool followPlayer = true;

    private void OnShift(InputValue inputValue){
        Debug.Log(inputValue);
        followPlayer = !inputValue.isPressed;
    }

    // Update is called once per frame
    void Update()
    {

        if(false){//followPlayer
            this.transform.position = player.position;
        }else{
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = (player.position + mousePos) / 2;

            targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

            this.transform.position = targetPos;
        } 
    }
}
