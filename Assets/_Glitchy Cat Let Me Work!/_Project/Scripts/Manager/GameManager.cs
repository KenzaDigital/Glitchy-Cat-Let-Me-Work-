using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("UI Journée")]
    public GameObject dayPartPanel; // le panneau affichant la partie de la journée
    public TextMeshProUGUI dayPartTextUI; // texte TMP affiché dans le panneau

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            currentDayPart = (DayPart)PlayerPrefs.GetInt("dayPart", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateDayPartUI(); // met à jour l'affichage dès le début
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateDayPartUI(); // réaffiche la bonne info à chaque changement de scène

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
        }

        SaveDayProgress();
        UpdateDayPartUI(); // mettre à jour le texte après changement
    }

    public void SaveDayProgress()
    {
        PlayerPrefs.SetInt("dayPart", (int)currentDayPart);
        PlayerPrefs.Save();
    }

    public void UpdateDayPartUI()
    {
        if (dayPartPanel == null || dayPartTextUI == null)
            return;

        string dayText = "";

        switch (currentDayPart)
        {
            case DayPart.Matin:
                dayText = "Matin";
                break;
            case DayPart.PauseDejeuner:
                dayText = "Pause Déjeuner";
                break;
            case DayPart.ApresMidi:
                dayText = "Après-midi";
                break;
            case DayPart.Termine:
                dayText = "Journée terminée";
                break;
        }

        dayPartPanel.SetActive(true);
        dayPartTextUI.text = dayText;
    }
}
