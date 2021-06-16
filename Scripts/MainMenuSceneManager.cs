using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject howToObject;

    public void PressStartButton()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("GameScene");
        async.allowSceneActivation = true;
    }

    public void PressHowToButton()
    {
        if(howToObject != null)
        {
            howToObject.SetActive(!howToObject.activeInHierarchy);
        }
    }

    public void PressExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
