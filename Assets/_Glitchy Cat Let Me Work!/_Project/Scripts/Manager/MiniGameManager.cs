using UnityEngine;

public enum MiniGameType
{
    None,
    TriDeMail,
    FichierCrush,
    PongMeeting,
    Dejeuner,
}

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    public MiniGameType currentMiniGame = MiniGameType.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentMiniGame(MiniGameType miniGame)
    {
        if (currentMiniGame != miniGame)
        {
            currentMiniGame = miniGame;
            Debug.Log($"Mini-jeu défini : {miniGame}");
        }
    }

    public MiniGameType GetCurrentMiniGame()
    {
        return currentMiniGame;
    }

    public void ClearCurrentMiniGame()
    {
        Debug.Log("Mini-jeu courant réinitialisé.");
        currentMiniGame = MiniGameType.None;
    }
}
