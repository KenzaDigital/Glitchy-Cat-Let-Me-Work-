using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialHighlighter[] steps;  // ordre du tutoriel
    private int currentIndex = 0;

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
}
