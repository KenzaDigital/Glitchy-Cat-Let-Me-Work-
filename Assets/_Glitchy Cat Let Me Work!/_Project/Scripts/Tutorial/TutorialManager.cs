using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public TutorialHighlighter[] steps;  // ordre du tutoriel
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        HighlightCurrent();
    }

    public void AdvanceStep()
    {
        Debug.Log(">>> AdvanceStep appelé — étape actuelle : " + currentIndex);

        if (currentIndex < steps.Length)
        {
            steps[currentIndex].DeactivateHighlight();
            currentIndex++;
            Debug.Log("→ Étape suivante : " + currentIndex);

            if (currentIndex < steps.Length)
            {
                HighlightCurrent();
            }
            else
            {
                Debug.Log("🎉 Tutoriel terminé !");
            }
        }
    }

    void HighlightCurrent()
    {
        if (currentIndex < steps.Length)
        {
            Debug.Log("Tutoriel → Highlight de l'étape " + currentIndex + " : " + steps[currentIndex].gameObject.name);
            steps[currentIndex].ActivateHighlight();
        }
        else
        {
            Debug.Log("Tutoriel terminé !");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espace pressé, avancée de l'étape du tutoriel");
            AdvanceStep();
        }
    }
}
