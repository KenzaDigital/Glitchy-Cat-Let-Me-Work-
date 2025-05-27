using UnityEngine;
using UnityEngine.SceneManagement;

public enum DayPart
{
    Matin,
    PauseDejeuner,
    ApresMidi,
    Termine
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DayPart currentDayPart = DayPart.Matin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Chargement de la progression sauvegardée (si nécessaire)
            currentDayPart = (DayPart)PlayerPrefs.GetInt("dayPart", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    public DayPart GetCurrentDayPart()
    {
        return currentDayPart;
    }

    public void NextDayStep()
    {
        switch (currentDayPart)
        {
            case DayPart.Matin:
                currentDayPart = DayPart.PauseDejeuner;
                Debug.Log("➡️ Passage à la Pause Déjeuner !");
                break;

            case DayPart.PauseDejeuner:
                currentDayPart = DayPart.ApresMidi;
                Debug.Log("➡️ Passage à l’Après-midi !");
                break;

            case DayPart.ApresMidi:
                currentDayPart = DayPart.Termine;
                Debug.Log("✅ Journée terminée !");
                break;

            default:
                break;
        }

        SaveDayProgress();
    }

    public void SaveDayProgress()
    {
        PlayerPrefs.SetInt("dayPart", (int)currentDayPart);
        PlayerPrefs.Save();
    }
}