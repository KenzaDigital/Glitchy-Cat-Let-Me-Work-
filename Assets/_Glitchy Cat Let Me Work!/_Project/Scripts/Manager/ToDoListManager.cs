using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

        // Marquer comme complétée si déjà dans la liste
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
        // Toutes les clés en minuscules, sans espace ni accents
        yield return "tridemail";
        yield return "fichiercrush";
        yield return "rapport";
        yield return "pausedejeuner";
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
}
