using UnityEngine;

public enum MiniGameType
{
    
    None,
    TriDeMail,
    FichierCrush,
    AuRapport,
 
    
   
    // Add more mini-game types as needed
}
public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    public MiniGameType currentMiniGame;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetCurrentMiniGame(MiniGameType miniGame)
    {
        currentMiniGame = miniGame;
    }
}