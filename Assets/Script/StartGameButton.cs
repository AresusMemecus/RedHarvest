using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton
{
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        } 
    }

}
