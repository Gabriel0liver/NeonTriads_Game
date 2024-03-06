using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class mmButton : MonoBehaviour
{
    
    public enum ButtonType {
        play,
        options,
        exit
    }

    public ButtonType type;

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI shadowText;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        switch(type){
            case ButtonType.play:
                Select();
            break;
            default :
                DeSelect();
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(){
        anim.SetBool("selected",true);
        Color color;
        if(ColorUtility.TryParseHtmlString("#32C5D1", out color)){
            mainText.color =  color;
        }
    }

    public void DeSelect(){
        anim.SetBool("selected",false);
        Color color;
        if(ColorUtility.TryParseHtmlString("#BCBCBC", out color)){
            mainText.color =  color;
        }
    }

    public void Execute(){
        switch(type){
            case ButtonType.play:
                SceneManager.LoadScene("LevelSelection");
            break;
            case ButtonType.options:
                SceneManager.LoadScene("Options");
            break;
            case ButtonType.exit:
                Application.Quit();
            break;
        }
    }
}
