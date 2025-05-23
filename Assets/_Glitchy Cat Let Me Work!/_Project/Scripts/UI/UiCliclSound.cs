using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIClickSound : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        audioManager.instance.PlaySFX("Clic");
    }
}
