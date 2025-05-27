using UnityEngine;
using System.Collections.Generic;

public class ToDoListManager : MonoBehaviour
{
    public static ToDoListManager Instance;

    [Header("Liste des tâches")]
    public List<TaskItem> tasks = new List<TaskItem>();

    private HashSet<string> completedTasks = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCompletedTasks();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterTask(TaskItem task)
    {
        if (!tasks.Contains(task))
            tasks.Add(task);

        string key = task.GetTaskKey();
        if (completedTasks.Contains(key))
        {
            task.CompleteTask();
        }
    }

    public void Unsubscrible(TaskItem task)
    {
        if (tasks.Contains(task))
            tasks.Remove(task);
    }

    public void MarkTaskCompletedByName(string taskName)
    {
        string key = taskName.Trim().ToLower();

        if (completedTasks.Contains(key))
            return;

        completedTasks.Add(key);
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        foreach (var task in tasks)
        {
            if (task.GetTaskKey() == key)
            {
                task.CompleteTask();
                return;
            }
        }
    }

    private void LoadCompletedTasks()
    {
        completedTasks.Clear();

        foreach (string key in PlayerPrefsKeys())
        {
            if (PlayerPrefs.GetInt(key, 0) == 1)
            {
                completedTasks.Add(key);
            }
        }
    }

    public void SaveCompletedTasks()
    {
        foreach (string task in completedTasks)
        {
            PlayerPrefs.SetInt(task, 1);
        }
        PlayerPrefs.Save();
    }

    private IEnumerable<string> PlayerPrefsKeys()
    {
        yield return "tridemail";
        yield return "fichiercrush";
        yield return "meeting";
        yield return "pausedejeuner";
        yield return "fidelistesclients";
    }

    public void SyncTasksState()
    {
        foreach (var task in tasks)
        {
            string key = task.GetTaskKey();
            if (completedTasks.Contains(key))
            {
                task.CompleteTask();
            }
            else
            {
                task.ResetTask();
            }
        }
    }

    public bool AreTasksForCurrentStepCompleted()
    {
        if (GameManager.Instance == null) return false;

        DayPart step = GameManager.Instance.GetCurrentDayPart();
        string[] tasksToCheck;

        switch (step)
        {
            case DayPart.Matin:
                tasksToCheck = new[] { "tridemail", "fichiercrush" };
                break;

            case DayPart.PauseDejeuner:
                tasksToCheck = new[] { "pausedejeuner" };
                break;

            case DayPart.ApresMidi:
                tasksToCheck = new[] { "fidelistesclients", "rapport" };
                break;

            default:
                return true;
        }

        foreach (string task in tasksToCheck)
        {
            if (!IsTaskCompleted(task))
                return false;
        }

        return true;
    }

    public bool IsTaskCompleted(string taskName)
    {
        string key = taskName.Trim().ToLower();
        return completedTasks.Contains(key);
    }
}