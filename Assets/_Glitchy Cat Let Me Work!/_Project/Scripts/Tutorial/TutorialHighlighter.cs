using UnityEngine;
using UnityEngine.UI;

public class TutorialHighlighter : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isActive = false;
    private Image imageComponent;

    // Optionnel : couleur normale et couleur de surbrillance
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    void Start()
    {

        originalScale = transform.localScale;
        imageComponent = GetComponent<Image>();

        // Remet la couleur normale au démarrage
        if (imageComponent != null)
            imageComponent.color = normalColor;
    }

    void Update()
    {
        if (isActive)
        {
            // Animation de pulsation (scale entre 0.95 et 1.05)
            float scale = 1f + Mathf.Sin(Time.time * 4f) * 0.05f;
            transform.localScale = originalScale * scale;

            // Debug moins répétitif : afficher une fois par seconde max
            if (Time.frameCount % 60 == 0)
                Debug.Log($"Highlight actif sur {gameObject.name}");
        }
    }

    public void ActivateHighlight()
    {
        isActive = true;

        if (imageComponent != null)
            imageComponent.color = highlightColor;

        Debug.Log($"ActivateHighlight appelé sur {gameObject.name}");
    }

    public void DeactivateHighlight()
    {
        isActive = false;
        transform.localScale = originalScale;

        if (imageComponent != null)
            imageComponent.color = normalColor;

        Debug.Log($"DeactivateHighlight appelé sur {gameObject.name}");
    }
}
