using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LaunchDossierCrush()
    {
        GameManager.Instance.LoadScene("DossierCrush");
    }


    public void LaunchPong()
    {
        GameManager.Instance.LoadScene("PongMeeting");
    }

    public void LaunchPauseDejeuner()
    {
        GameManager.Instance.LoadScene("PauseDej");
    }

    public void LaunchQuiEsce()
    {
        GameManager.Instance.LoadScene("QuiEsce");


    }


    }
