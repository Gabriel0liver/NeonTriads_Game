using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LevelController : MonoBehaviour
{
    public GameObject[] levels;
    private int selectedIndex = 0;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public float animationTime = 0.25f;
    float nextStart;

    public Animator fadeTransition;
    public float transitionTime;
    public float soundTime;

    void Start(){
        nextStart = Time.time;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.SetFloat("time1", 212.45f);
        for (int i = 0; i < levels.Length; i++)
        {
            //get levels[i] component with tag BestTime
            TextMeshProUGUI[] texts = levels[i].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if(text.tag == "BestTime"){
                    float bestTime = PlayerPrefs.GetFloat("time"+i+1);
                    if(bestTime > 0){
                        int minutes = Mathf.FloorToInt(bestTime / 60);
                        int seconds = Mathf.FloorToInt(bestTime % 60);
                        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
                        text.text = "Best time: " + timerString;
                    }else{
                        text.text = "Best time: --:--";
                    }
                }
            }

        }
    }

    private void OnLeft(InputValue input){
        float lastPress = Time.time;
        float timeUnitlStart = nextStart - lastPress;
        if(timeUnitlStart < 0){
            timeUnitlStart = 0;
        }
        StartCoroutine(Animate(timeUnitlStart,-1));
       
        nextStart = timeUnitlStart + lastPress + 0.25f;
    }

     IEnumerator Animate(float timeUnitlStart , int direction){
        FindAnyObjectByType<AudioManager>().Play("ui");
        yield return new WaitForSeconds(timeUnitlStart);
        SelectMenuItem(selectedIndex + direction);
    }

    private void OnRight(InputValue input){
        float lastPress = Time.time;
        float timeUnitlStart = nextStart - lastPress;
        if(timeUnitlStart < 0){
            timeUnitlStart = 0;
        }
        StartCoroutine(Animate(timeUnitlStart,+1));
       
        nextStart = timeUnitlStart + lastPress + 0.25f;
    }

    private void OnSubmit()
    {
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
        SceneManager.LoadScene(selectedIndex+1);
    }

    void SelectMenuItem(int index)
    {
        // Ensure the index stays within valid range
        int prevIndex = selectedIndex;
        if(index >= 0){

        selectedIndex = Mathf.Clamp(index, 0, levels.Length - 1);

        if(selectedIndex > 0 && selectedIndex < levels.Length-1){
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }else if(selectedIndex == 0){
            leftArrow.SetActive(false);
        }else if(selectedIndex == levels.Length-1){
            rightArrow.SetActive(false);
        }
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == selectedIndex)
            {
                if(prevIndex < selectedIndex){
                    levels[i].GetComponent<Animator>().SetTrigger("showRight");
                }else{
                    levels[i].GetComponent<Animator>().SetTrigger("showLeft");
                }
            }else if(i == prevIndex){
                if(prevIndex < selectedIndex){
                    levels[i].GetComponent<Animator>().SetTrigger("hideLeft");
                }else{
                    levels[i].GetComponent<Animator>().SetTrigger("hideRight");
                }
            }
        }
        }
        
    }
}
