using UnityEngine;

public class TutorialHighlighter : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isActive = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isActive)
        {
            float scale = 1f + Mathf.Sin(Time.time * 4f) * 0.05f;
            transform.localScale = originalScale * scale;
        }
    }

    public void ActivateHighlight()
    {
        isActive = true;
    }

    public void DeactivateHighlight()
    {
        isActive = false;
        transform.localScale = originalScale;
    }
}
