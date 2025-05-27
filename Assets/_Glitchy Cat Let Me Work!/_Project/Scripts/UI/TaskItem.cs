using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskItem : MonoBehaviour
{
    public Toggle toggle;
    public TextMeshProUGUI taskLabel;

    public string taskText;
    public int productivityGain = 10;

    public bool alreadyCompleted = false;

    void Start()
    {
        if (toggle != null)
            toggle.interactable = false;

        ToDoListManager.Instance?.RegisterTask(this);
    }

    private void OnDestroy()
    {
        ToDoListManager.Instance?.Unsubscrible(this);
    }

    public void CompleteTask()
    {
        if (alreadyCompleted)
            return;

        alreadyCompleted = true;

        if (toggle != null)
        {
            toggle.isOn = true;
            toggle.interactable = false;
        }

        if (taskLabel != null)
        {
            Debug.Log($"Avant modif: {taskLabel.text}");
            taskLabel.text = $"<s>{taskText}</s>";
            taskLabel.ForceMeshUpdate();
            Debug.Log($"Après modif: {taskLabel.text}");
        }

        ProductivityManager.Instance?.AddProductivity(productivityGain);

        audioManager.instance.PlaySFX("Achievement");
    }

    public void ResetTask()
    {
        alreadyCompleted = false;

        if (toggle != null)
        {
            toggle.isOn = false;
            toggle.interactable = false;
        }

        if (taskLabel != null)
        {
            taskLabel.text = taskText;
        }
    }

    public bool IsCompleted()
    {
        return alreadyCompleted;
    }

    public string GetTaskKey()
    {
        return taskText.Trim().ToLower();
    }
}