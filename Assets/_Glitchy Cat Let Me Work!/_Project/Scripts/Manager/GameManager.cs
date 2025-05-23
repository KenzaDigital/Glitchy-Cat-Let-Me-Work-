using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainScene":
                audioManager.instance.PlayMusic("MainScene", true);
                break;
            case "DossierCrush":
                audioManager.instance.PlayMusic("DossierCrush", true);
                break;
            case "PauseDej":
                audioManager.instance.PlayMusic("PauseDejeuner", true);
                break;
            case "PongMeeting":
                audioManager.instance.PlayMusic("PongMeeting", true);
                break;
            case "QuiEsce":
                audioManager.instance.PlayMusic("QuiEsce", true);
                break;
            default:
                audioManager.instance.StopMusic();
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
