using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class TaskItem : MonoBehaviour
{
    [Header("UI Elements")]
    public UnityEngine.UI.Toggle toggle;
    public Label taskLabel;

    [Header("Tâche")]
    public string taskText;
    public int productivityGain = 10;

    private bool alreadyCompleted = false;

    void Start()
    {
        if (taskLabel != null)
            taskLabel.text = taskText;

        if (toggle != null)
            toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn && !alreadyCompleted)
        {
            alreadyCompleted = true;

            taskLabel.text = $"<s>{taskText}</s>";
            

           
            ProductivityManager.Instance?.AddProductivity(productivityGain);
        }
        else if (!isOn && alreadyCompleted)
        {
            alreadyCompleted = false;

            
            taskLabel.text = taskText;
           
           
            ProductivityManager.Instance?.RemoveProductivity(productivityGain);
        }
    }
}