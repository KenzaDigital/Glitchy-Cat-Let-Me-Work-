using System;
using UnityEngine;
using UnityEngine.UI;

public class ProductivityManager : MonoBehaviour
{
    public static ProductivityManager Instance;

    [Header ("productivity Settings")]
    [Range (0,100)]
    public int currentProductivity = 50;
    public int maxProductivity = 100;

    public Action OnUpdateUI;


    private void Awake()
    {
        // Singleton pattern to ensure only one instance of this class exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddProductivity(int amount)
    {
        currentProductivity =Mathf.Clamp(currentProductivity + amount,0,maxProductivity);

        OnUpdateUI?.Invoke();
    }

    public void RemoveProductivity(int amount)
    {
        currentProductivity = Mathf.Clamp(currentProductivity -  amount, 0, maxProductivity);

        OnUpdateUI?.Invoke();
    }

    public int GetCurrentProductivity()
    {
        return currentProductivity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
