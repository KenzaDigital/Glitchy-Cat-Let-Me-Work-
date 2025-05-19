using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void LaunchDossierCrush()
    {
        GameManager.Instance.LoadScene("DossierCrush");
    }
}
