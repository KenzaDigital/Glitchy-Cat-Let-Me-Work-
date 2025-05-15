using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VirusChatManager : MonoBehaviour
{
    [Header("Interruption UI")]
    public GameObject blackoutPanel;
    public GameObject popupPanel;
    public GameObject[] popupWindows; // Les popups individuelles (enfants de popupPanel)

    public Button proButton;
    public Button spamButton;

    [Header("Timings")]
    public float timeBetweenInterruptions = 15f;
    public float sabotageDuration = 5f;

    [Header("Popups Settings")]
    public int maxPopupsAtOnceStart = 1;  // Au début 1 popup max
    public int maxPopupsAtOnceMax = 3;    // Max atteint après progression
    public float popupDifficultyIncreaseInterval = 60f; // toutes les X secondes on augmente le max popups
    private float popupDifficultyTimer = 0f;

    [Header("Distraction")]
    public bool isCalm = false;

    private float interruptionTimer = 0f;
    private bool isInterruptionActive = false;

    void Start()
    {
        blackoutPanel?.SetActive(false);
        popupPanel?.SetActive(false);

        foreach (var popup in popupWindows)
            popup.SetActive(false);
    }

    void Update()
    {
        if (isCalm || isInterruptionActive)
            return;

        interruptionTimer += Time.deltaTime;
        popupDifficultyTimer += Time.deltaTime;

        // Augmente max popups progressif
        if (popupDifficultyTimer >= popupDifficultyIncreaseInterval)
        {
            popupDifficultyTimer = 0f;
            maxPopupsAtOnceStart = Mathf.Min(maxPopupsAtOnceStart + 1, maxPopupsAtOnceMax);
            Debug.Log($"Difficulté popup augmentée: maxPopupsAtOnce = {maxPopupsAtOnceStart}");
        }

        if (interruptionTimer >= timeBetweenInterruptions)
        {
            interruptionTimer = 0f;
            TriggerRandomInterruption();
        }
    }

    public void TriggerRandomInterruption()
    {
        // Si blackout actif => on ne déclenche rien d'autre
        if (blackoutPanel.activeSelf)
            return;

        // Compte popups actives
        int activePopupsCount = 0;
        foreach (var p in popupWindows)
            if (p.activeSelf)
                activePopupsCount++;

        // Si trop de popups déjà, on saute la popup interruption
        bool canShowPopup = activePopupsCount < maxPopupsAtOnceStart;

        int r;
        if (canShowPopup)
        {
            r = Random.Range(0, 3); // Toutes les interruptions possibles
        }
        else
        {
            // Pas de popup possible, on choisit entre blackout ou inversion seulement
            r = Random.Range(0, 2); // 0 ou 1
        }

        switch (r)
        {
            case 0:
                Debug.Log("📴 Blackout triggered");
                StartCoroutine(BlackoutRoutine());
                break;
            case 1:
                if (canShowPopup)
                {
                    Debug.Log("📦 Popup triggered");
                    StartCoroutine(PopupRoutine());
                }
                else
                {
                    Debug.Log("Popup ignorée, max popups atteints");
                }
                break;
            case 2:
                Debug.Log("🔁Invert button triggered");
                StartCoroutine(InvertButtonRoutine());
                break;
        }
    }

    IEnumerator BlackoutRoutine()
    {
        isInterruptionActive = true;
        blackoutPanel.SetActive(true);

        yield return new WaitForSeconds(sabotageDuration);

        blackoutPanel.SetActive(false);
        isInterruptionActive = false;
    }

    IEnumerator PopupRoutine()
    {
        isInterruptionActive = true;

        // On peut afficher plusieurs popups si maxPopupsAtOnce > 1
        int activePopupsCount = 0;
        foreach (var p in popupWindows)
            if (p.activeSelf)
                activePopupsCount++;

        int popupsToShow = Mathf.Min(maxPopupsAtOnceStart - activePopupsCount, 2); // Affiche 1 ou 2 popups max

        for (int i = 0; i < popupsToShow; i++)
        {
            GameObject popup = GetRandomInactivePopup();
            if (popup != null)
            {
                popup.SetActive(true);
                Debug.Log("Popup affichée : " + popup.name);
            }
            else
            {
                Debug.Log("Toutes les popups sont déjà actives !");
                break;
            }
        }

        // Active le conteneur popupPanel si au moins un popup actif
        popupPanel.SetActive(AreAnyPopupsActive());

        // Pas de fermeture auto, joueur ferme manuellement
        isInterruptionActive = false;
        yield return null;
    }

    GameObject GetRandomInactivePopup()
    {
        List<GameObject> inactivePopups = new List<GameObject>();

        foreach (var popup in popupWindows)
        {
            if (!popup.activeSelf)
                inactivePopups.Add(popup);
        }

        if (inactivePopups.Count == 0)
            return null;

        int rand = Random.Range(0, inactivePopups.Count);
        return inactivePopups[rand];
    }

    IEnumerator InvertButtonRoutine()
    {
        isInterruptionActive = true;

        if (proButton == null || spamButton == null)
        {
            Debug.LogWarning("Boutons non assignés !");
            isInterruptionActive = false;
            yield break;
        }

        Vector3 temp = proButton.transform.position;
        proButton.transform.position = spamButton.transform.position;
        spamButton.transform.position = temp;

        yield return new WaitForSeconds(sabotageDuration);

        // Remet les boutons à leur place initiale (ajuste selon ton besoin)
        // Par exemple, stocke la position initiale dans Start() pour remettre ici

        isInterruptionActive = false;
    }

    public void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
        Debug.Log("Popup fermée par l'utilisateur");

        // Désactive le conteneur si plus aucun popup actif
        if (!AreAnyPopupsActive())
        {
            popupPanel.SetActive(false);
        }
    }

    bool AreAnyPopupsActive()
    {
        foreach (var p in popupWindows)
        {
            if (p.activeSelf)
                return true;
        }
        return false;
    }

    public void CalmTheCat()
    {
        StopAllCoroutines();

        blackoutPanel.SetActive(false);
        popupPanel.SetActive(false);
        foreach (var popup in popupWindows)
            popup.SetActive(false);

        isCalm = true;
        StartCoroutine(CalmDuration());
    }

    IEnumerator CalmDuration()
    {
        Debug.Log("Le chat est calme temporairement");
        yield return new WaitForSeconds(15f); // Ou ta variable calmDuration
        isCalm = false;
        Debug.Log("Le chat recommence à embêter !");
    }
}
