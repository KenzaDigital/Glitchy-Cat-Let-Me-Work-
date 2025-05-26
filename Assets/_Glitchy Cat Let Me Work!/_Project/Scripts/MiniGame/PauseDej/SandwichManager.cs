using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SandwichManager : MonoBehaviour
{
    public static SandwichManager Instance;

    public List<string> expectedOrder = new List<string> { "Pain", "Beurre", "Jambon", "Pain" };
    private List<string> placedOrder = new List<string>();

    public GameObject sandwichDonePanel;
    //public Animator sandwichDoneAnimator;
    public TextMeshProUGUI feedbackText;

    private List<DraggableObject> registeredIngredients = new List<DraggableObject>();

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {

        MiniGameManager.Instance.SetCurrentMiniGame(MiniGameType.PauseDejeuner);

    }

    public void RegisterIngredientObject(DraggableObject obj)
    {
        if (!registeredIngredients.Contains(obj))
        {
            registeredIngredients.Add(obj);
        }
    }

    public void RegisterIngredient(string name, DraggableObject obj)
    {

        Debug.Log($"Ingredient placé : '{name}' (attendu : '{expectedOrder[placedOrder.Count]}')");

        placedOrder.Add(name);
        int index = placedOrder.Count - 1;

        if (index >= expectedOrder.Count || placedOrder[index] != expectedOrder[index])
        {
            Debug.LogWarning($"Erreur ordre : placé '{placedOrder[index]}', attendu '{expectedOrder[index]}'");
            feedbackText.text = "🥴 Mauvais ordre ! Recommence...";
            ResetSandwich();
            return;
        }

        feedbackText.text = $"{placedOrder.Count}/{expectedOrder.Count} ingrédients placés";

        // Animation de disparition avec DOTween
        obj.PopAndDisappear();

        if (placedOrder.Count == expectedOrder.Count)
        {
            feedbackText.text = "Sandwich prêt !";
            ShowSandwichPanel();
        }
    }

    void ShowSandwichPanel()
    {
        Debug.Log("Activation du panel sandwich !");
        sandwichDonePanel.SetActive(true);
       // sandwichDoneAnimator.Play("PopupIn", 0, 0f);
    }

    public void ResetSandwich()
    {
        placedOrder.Clear();
        Debug.Log("Reset sandwich, placedOrder cleared");

        foreach (var obj in registeredIngredients)
        {
            obj.gameObject.SetActive(true);
            obj.transform.localScale = Vector3.one;
            obj.ResetPosition();
        }
    }

    public void Restart()
    {
        sandwichDonePanel.SetActive(false);
        ResetSandwich();
    }

  



}
