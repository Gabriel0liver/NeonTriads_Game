using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private bool dead = false;
    private bool paused = false;
    private bool victory = false;

    public GameObject deathUI;
    public GameObject pauseUI;
    public GameObject levelCompleteUI;
    public GameObject blackScreen;

    public Animator fadeTransition;
    public float transitionTime;

    private float gameTime;
    private int enemiesLeft;
    public TextMeshProUGUI timerText;

    void Start(){
        dead = false;
        gameTime = 0f;
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        UpdateTimerText();

        if (enemiesLeft == 0)
        {
            levelComplete();
        }
    }

    public void enemyDeath(){
        Debug.Log(enemiesLeft);
        enemiesLeft--;
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }

    public void death(){
        dead = true;
        deathUI.SetActive(true);

    }

    void OnRestart(){
        if(dead){
            StartCoroutine(restartLevel());
        }
    }

    IEnumerator restartLevel(){
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void levelComplete(){
        victory = true;
        levelCompleteUI.SetActive(true);
        if(PlayerPrefs.GetFloat("time"+SceneManager.GetActiveScene().buildIndex) > gameTime || PlayerPrefs.GetFloat("time"+SceneManager.GetActiveScene().buildIndex) == 0){
            PlayerPrefs.SetFloat("time"+SceneManager.GetActiveScene().buildIndex, gameTime);
        }
    }

    void OnPause(){
        Debug.Log("Pause");
        if(!dead){
            if(paused){
                Resume();
            }else{
                Pause();
            }
        }
    }

    void OnEnter(){
        if(victory){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    void OnExit(){
        if(paused || victory){
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1f;
        }
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        paused = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 0f;

    }

    void Resume(){
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        paused = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
