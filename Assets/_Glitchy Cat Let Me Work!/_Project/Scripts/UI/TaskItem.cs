using UnityEngine;
using UnityEngine.UIElements;

public class TaskItem : MonoBehaviour
{
    public UnityEngine.UI.Toggle toggle;
    public Label taskLabel;

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
            if (taskLabel != null)
                taskLabel.text = $"<s>{taskText}</s>"; // barré avec balise <s>
            ProductivityManager.Instance?.AddProductivity(productivityGain);
        }
        else if (!isOn && alreadyCompleted)
        {
            alreadyCompleted = false;
            if (taskLabel != null)
                taskLabel.text = taskText;
            ProductivityManager.Instance?.RemoveProductivity(productivityGain);
        }
    }

    public void CompleteTask()
    {
        alreadyCompleted = true;

        if (taskLabel != null)
            taskLabel.text = $"<s>{taskText}</s>";
    }

    public bool IsCompleted()
    {
        return alreadyCompleted;
    }
}
