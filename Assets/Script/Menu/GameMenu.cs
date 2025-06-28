using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] public GameObject menuPanel;

    public PlayerController playerController;
    public ActionTrigger actionTrigger;

    public void OnClickMenuButton()
    {
        menuPanel.SetActive(true);
        playerController.enabled = false;
        actionTrigger.enabled = false;
    }

    public void OnClickPlayButton()
    {
        menuPanel.SetActive(false);
        playerController.enabled = true;
        actionTrigger.enabled = true;
    }

    public void OnClickExitButton()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("CoreScene");
    }

    public void OnClickSettingsMenuButton()
    {

    }

    public void OnClickExitSettingsMenuButton()
    {

    }
}
