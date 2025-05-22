using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    public string sceneToLoad = "MainScene";

    public void LoadMainScene()
    {
        Debug.Log("Chargement de la scène : " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}