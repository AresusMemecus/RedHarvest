using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]public GameObject menuPanel;
    public void OnClickMenuButton(){
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickPlayButton(){
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickExitButton(){
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("CoreScene");
    }

    public void OnClickSettingsMenuButton(){

    }   

    public void OnClickExitSettingsMenuButton(){

    }   
}
