using System;
using UnityEngine;
using UnityEngine.UI;

public class ProductivitySlider : MonoBehaviour
{
    [SerializeField] private Slider slider; // Prefab for the slider
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ProductivityManager.Instance.OnUpdateUI += UpdateSlider;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        slider.value = (float)ProductivityManager.Instance.GetCurrentProductivity() / (float)ProductivityManager.Instance.maxProductivity;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (ProductivityManager.Instance != null)
        {
            ProductivityManager.Instance.OnUpdateUI -= UpdateSlider;
        }
    }
}
