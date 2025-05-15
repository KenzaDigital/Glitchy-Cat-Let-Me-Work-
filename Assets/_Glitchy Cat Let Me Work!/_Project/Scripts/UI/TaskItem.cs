using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskItem : MonoBehaviour
{
    public Toggle toggle;
    public TextMeshProUGUI taskLabel;

    public string taskText;
    public int productivityGain = 10;

    private bool alreadyCompleted = false;

    void Start()
    {
        if (taskLabel != null)
            taskLabel.text = taskText;

        // On désactive le toggle pour que le joueur ne puisse pas l'utiliser
        if (toggle != null)
        {
            toggle.interactable = false; // Bloque l'interaction utilisateur
            // Pas besoin d'écouter le changement utilisateur puisque toggle est non interactif
        }
    }

    /// <summary>
    /// Complète la tâche via le script uniquement
    /// </summary>
    public void CompleteTask()
    {
        if (alreadyCompleted)
            return;

        alreadyCompleted = true;

        if (toggle != null)
        {
            toggle.isOn = true;
            toggle.interactable = false; // s'assure que c'est non interactif
        }

        if (taskLabel != null)
            taskLabel.text = $"<s>{taskText}</s>";

        ProductivityManager.Instance?.AddProductivity(productivityGain);
    }

    public bool IsCompleted()
    {
        return alreadyCompleted;
    }
}
