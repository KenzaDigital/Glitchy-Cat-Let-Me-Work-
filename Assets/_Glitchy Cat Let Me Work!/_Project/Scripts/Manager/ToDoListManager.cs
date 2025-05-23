using UnityEngine;
using System.Collections.Generic;

public class ToDoListManager : MonoBehaviour
{
    public static ToDoListManager Instance;

    [Header("Liste des tâches")]
    public List<TaskItem> tasks = new List<TaskItem>();

    // Stocke les noms des tâches complétées (en minuscules)
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
        string key = task.taskText.Trim().ToLower();

        // Évite les doublons
        if (!tasks.Contains(task))
            tasks.Add(task);

        // Si la tâche a déjà été complétée, on la met à jour
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
            return; // Déjà complété

        completedTasks.Add(key);

        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        foreach (var task in tasks)
        {
            if (task.taskText.Trim().ToLower() == key)
            {
                task.CompleteTask();
                return;
            }
        }
    }

    private void LoadCompletedTasks()
    {
        completedTasks.Clear();

        // on ne dépend plus de la présence des objets dans la scène ici
        foreach (string key in PlayerPrefsKeys())
        {
            if (PlayerPrefs.GetInt(key, 0) == 1)
                completedTasks.Add(key);
        }
    }

    // ✅ MÉTHODE MANQUANTE : à appeler après avoir complété une tâche
    public void SaveCompletedTasks()
    {
        foreach (string task in completedTasks)
        {
            PlayerPrefs.SetInt(task, 1);
        }

        PlayerPrefs.Save();
    }

    // Méthode utilitaire pour récupérer les clés connues
    private IEnumerable<string> PlayerPrefsKeys()
    {
        yield return "tridemail";
        yield return "fichiercrush";
        yield return "rapport;";
        yield return "pause dejeuner";
    }

    // ✅ Pour re-synchroniser la UI avec les données sauvegardées
    public void SyncTasksState()
    {
        foreach (var task in tasks)
        {
            string key = task.taskText.Trim().ToLower();
            if (completedTasks.Contains(key))
            {
                task.CompleteTask();
            }
        }
    }

}
