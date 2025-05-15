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

        if (toggle != null)
            toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn && !alreadyCompleted)
        {
            alreadyCompleted = true;
            if (taskLabel != null)
                taskLabel.text = $"<s>{taskText}</s>";
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

        if (toggle != null)
            toggle.isOn = true;

        if (taskLabel != null)
            taskLabel.text = $"<s>{taskText}</s>";
    }

    public bool IsCompleted()
    {
        return alreadyCompleted;
    }
}
