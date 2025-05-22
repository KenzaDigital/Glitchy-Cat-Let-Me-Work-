using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    public Color flashColor = Color.yellow; // Couleur flash
    public float flashDuration = 0.2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient"))
        {
            var drag = other.GetComponent<DraggableObject>();
            if (drag != null) drag.SetOverBoard(true);

            FlashEffect(); // effet visuel rapide
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient"))
        {
            var drag = other.GetComponent<DraggableObject>();
            if (drag != null) drag.SetOverBoard(false);
        }
    }

    void FlashEffect()
    {
        CancelInvoke();
        sr.color = flashColor;
        Invoke(nameof(ResetColor), flashDuration);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }
}
