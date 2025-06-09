using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowMenu : MonoBehaviour
{
    [SerializeField] public GameObject[] menuPanel;
        public GameObject Charachter;


    public void onButtonClick()
    {
        // Закрываем все панели
        foreach (GameObject panel in menuPanel)
        {
            panel.SetActive(false);
        }

        // Включаем управление персонажем
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();
        playerController.isFrozen = false;
        characterController.enabled = true;
    }

}