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

    public void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
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
        Debug.Log("🔍 Tentative de marquer la tâche comme complétée : " + key);

        if (completedTasks.Contains(key))
        {
            Debug.Log("⚠️ Tâche déjà marquée comme complétée : " + key);
            return;
        }

        completedTasks.Add(key);
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        Debug.Log("✅ Tâche marquée comme complétée et sauvegardée dans PlayerPrefs : " + key);

        foreach (var task in tasks)
        {
            Debug.Log("🔁 Comparaison avec la tâche : " + task.GetTaskKey());
            if (task.GetTaskKey() == key)
            {
                Debug.Log("✅ Correspondance trouvée, tâche complétée dans la UI : " + key);
                task.CompleteTask();
                return;
            }
        }

        Debug.Log("❌ Aucune tâche trouvée avec la clé : " + key);
    }


    private void LoadCompletedTasks()
    {
        completedTasks.Clear();

        foreach (string key in PlayerPrefsKeys())
        {
            if (PlayerPrefs.GetInt(key, 0) == 1)
            {
                Debug.Log("Tâche chargée comme complétée : " + key);
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
        yield return "trierlesmails";
        yield return "classerlesfichiers";
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
        Debug.Log($"🔄 Vérification des tâches pour l'étape : {step}");

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
                Debug.Log("⚠️ Étape inconnue ou terminée");
                return true;
        }

        foreach (string task in tasksToCheck)
        {
            bool isCompleted = IsTaskCompleted(task);
            Debug.Log($"➡️ Tâche '{task}' complétée ? {isCompleted}");
            if (!isCompleted)
                return false;
        }

        Debug.Log("✅ Toutes les tâches sont complétées pour cette étape.");
        return true;
    }

    public bool IsTaskCompleted(string taskName)
    {
        string key = taskName.Trim().ToLower();
        return completedTasks.Contains(key);
    }
}