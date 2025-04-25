using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]public GameObject menuPanel, settingsPanel;
    
    public void OnClickStartButton(){
        SceneManager.LoadScene("FirstLevel", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void OnClickSettingsButton(){
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OnClickExitButton(){
        Application.Quit();
    }

    public void OnClickExitSettingsMenuButton(){
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }   
}
