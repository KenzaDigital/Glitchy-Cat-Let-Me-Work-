using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("MainScene"); 
    }

    public void LoadGame()
    {
        Debug.Log("Charger la sauvegarde..."); 
    }

    public void OpenOptions()
    {
        
        Debug.Log("Options ouvertes");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quitter()
    {
        // Ferme le jeu (ne marche que dans le build final)
        Application.Quit();

        // Pour tester dans l’éditeur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}