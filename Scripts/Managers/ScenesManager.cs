using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) Debug.LogWarning("More than 1 instance of SceneManager");
        Instance = this;
    }

    public enum Scene
    {
        MainMenu,
        Tutorial
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
    public void LoadNewGame()
    {
        SceneManager.LoadScene(Scene.Tutorial.ToString());
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }
}
