using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject[] menuButtons;
    private int selectedIndex = 0;

    public Animator fadeTransition;
    public float transitionTime;
    public float soundTime;


    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnNavigate(InputValue input){
        selectedIndex -= Mathf.RoundToInt(input.Get<Vector2>().y);
        SelectMenuItem(selectedIndex);
        
    }

    private void OnSubmit(){
        FindAnyObjectByType<AudioManager>().Play("enter");
        StartCoroutine(SubmitWithDelay());
    }

    IEnumerator SubmitWithDelay()
    {
        yield return new WaitForSeconds(soundTime);
        StartCoroutine(submit());
    }


    IEnumerator submit(){
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        menuButtons[selectedIndex].GetComponent<mmButton>().Execute();
    }    

    void SelectMenuItem(int index)
    {
        // Ensure the index stays within valid range
        selectedIndex = Mathf.Clamp(index, 0, menuButtons.Length - 1);
        // Handle highlighting or selecting the menu item
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (i == selectedIndex)
            {
                menuButtons[i].GetComponent<mmButton>().Select();
                FindAnyObjectByType<AudioManager>().Play("ui");
            }
            else
            {
                
                menuButtons[i].GetComponent<mmButton>().DeSelect();  
            }
        }
    }

    // Additional logic for handling the selection, for example, triggering an action when pressing Enter or Space.
    // You can implement this based on your specific needs.

}
