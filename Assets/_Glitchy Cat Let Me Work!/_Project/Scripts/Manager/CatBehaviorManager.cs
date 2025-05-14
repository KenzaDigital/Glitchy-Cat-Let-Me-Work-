using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirusChatManager : MonoBehaviour
{
    [Header("Interruption")]
    public GameObject blackoutPanel; // Correction du nom : "blakoutPanel" → "blackoutPanel"
    public GameObject popupPanel;
   
    public float timeBetweenInterruptions = 15f;
    public float sabotageDuration = 5f;

    [Header("Distraction")]
    public bool isCalm = false;
    public float calmDuration = 15f;

    private float interruptionTimer = 0f;

   
    public Button proButton;
    public Button spamButton;

    void Start()
    {
        if (blackoutPanel != null)
        {
            blackoutPanel.SetActive(false);
        }
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isCalm) return;

        interruptionTimer += Time.deltaTime;
        if (interruptionTimer >= timeBetweenInterruptions)
        {
            interruptionTimer = 0f;
            TriggerRandomInterruption();
        }
    }

    public void TriggerRandomInterruption()
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                Debug.Log("Blackout triggered");
                StartCoroutine(BlackoutRoutine());
                break;
            case 1:
                Debug.Log("Popup triggered");
                StartCoroutine(PopupRoutine());
                break;
            case 2:
                Debug.Log("Invert button triggered");
                StartCoroutine(InvertButtonRoutine());
                break;
        }
    }

    IEnumerator BlackoutRoutine()
    {
        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(sabotageDuration);
        blackoutPanel.SetActive(false);
    }

    IEnumerator PopupRoutine()
    {
        Debug.Log("Popup Active");
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(sabotageDuration);
        popupPanel.SetActive(false);
    }

    IEnumerator InvertButtonRoutine()
    {
        if (proButton == null || spamButton == null)
        {
            Debug.LogWarning("proButton or spamButton is not assigned!");
            yield break;
        }

        Vector3 tempPos = proButton.transform.position;
        proButton.transform.position = spamButton.transform.position;
        spamButton.transform.position = tempPos;
    }

    public void CalmTheCat()
    {
        StopAllCoroutines();
        blackoutPanel.SetActive(false);
        popupPanel.SetActive(false);
        isCalm = true;
        StartCoroutine(CalmDuration());
    }

    IEnumerator CalmDuration()
    {
        yield return new WaitForSeconds(calmDuration);
        isCalm = false;
    }
}
