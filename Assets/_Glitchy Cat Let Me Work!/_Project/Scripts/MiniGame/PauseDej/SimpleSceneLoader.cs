using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    public string sceneToLoad = "MainScene";

    public void LoadMainScene()
    {
        Debug.Log("Chargement de la sc�ne : " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}