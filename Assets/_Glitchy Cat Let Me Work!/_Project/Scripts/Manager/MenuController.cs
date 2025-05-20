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
}
