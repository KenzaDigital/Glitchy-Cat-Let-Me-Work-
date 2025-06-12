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
        else
        {
            task.ResetTask();
        }
    }

    public void MarkTaskCompletedByName(string taskName)
    {
        string key = taskName.Trim().ToLower();
        if (completedTasks.Contains(key)) return;

        completedTasks.Add(key);
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        foreach (var task in tasks)
        {
            if (task.GetTaskKey() == key)
            {
                task.CompleteTask();
                break;
            }
        }

        // Dès qu'une tâche est complétée, on vérifie si on peut avancer le tutoriel
        CheckAndAdvanceTutorialIfNeeded();
    }

    private void CheckAndAdvanceTutorialIfNeeded()
    {
        if (AreTasksForCurrentStepCompleted())
        {
            GameManager.Instance?.NextDayStep();

            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.AdvanceStep();
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

    public bool AreTasksForCurrentStepCompleted()
    {
        if (GameManager.Instance == null) return false;

        DayPart step = GameManager.Instance.GetCurrentDayPart();

        string[] tasksToCheck;

        switch (step)
        {
            case DayPart.Matin:
                tasksToCheck = new[] { "trierlesmails", "classerlesfichiers" };
                break;
            case DayPart.PauseDejeuner:
                tasksToCheck = new[] { "pausedejeuner" };
                break;
            case DayPart.ApresMidi:
                tasksToCheck = new[] { "fidelistesclients", "meeting" };
                break;
            default:
                return true;
        }

        foreach (string task in tasksToCheck)
        {
            if (!completedTasks.Contains(task))
                return false;
        }

        return true;
    }

    private void LoadCompletedTasks()
    {
        completedTasks.Clear();

        string[] keys = { "trierlesmails", "classerlesfichiers", "pausedejeuner", "fidelistesclients", "meeting" };
        foreach (string key in keys)
        {
            if (PlayerPrefs.GetInt(key, 0) == 1)
            {
                Debug.Log("Tâche déjà complétée au chargement : " + key);
                completedTasks.Add(key);
            }
        }
    }

    public void SyncTasksState()
    {
        foreach (var task in tasks)
        {
            string key = task.GetTaskKey();
            if (completedTasks.Contains(key))
                task.CompleteTask();
            else
                task.ResetTask();
        }
    }

    public void Unsubscribe(TaskItem task)
    {
        if (tasks.Contains(task))
        {
            tasks.Remove(task);
        }
    }
}
