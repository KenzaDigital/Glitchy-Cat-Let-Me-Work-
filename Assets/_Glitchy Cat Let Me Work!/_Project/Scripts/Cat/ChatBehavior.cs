using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatBehavior : MonoBehaviour
{
    public float walkSpeed = 100f; // UI = pixels
    public RectTransform[] walkPoints; // Points de destination UI
    public Animator animator;
    public Image chatImage;
    public float walkInterval = 10f; // Pause entre les déplacements

    private bool isWalking = false;
    private RectTransform chatRectTransform;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (chatImage == null) chatImage = GetComponent<Image>();
        chatRectTransform = chatImage.rectTransform;

        StartCoroutine(WalkLoop());
    }

    IEnumerator WalkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(walkInterval);
            if (!isWalking)
            {
                StartWalking();
            }
        }
    }

    public void StartWalking()
    {
        if (!isWalking && walkPoints.Length > 0)
        {
            StartCoroutine(WalkToRandomPoint());
        }
    }

    IEnumerator WalkToRandomPoint()
    {
        isWalking = true;

        // Choisir une destination aléatoire
        RectTransform target = walkPoints[Random.Range(0, walkPoints.Length)];
        Vector2 start = chatRectTransform.anchoredPosition;
        Vector2 end = target.anchoredPosition;

        // Inverser l'image selon la direction
        Vector3 scale = chatRectTransform.localScale;
        scale.x = end.x < start.x ? -1 : 1;
        chatRectTransform.localScale = scale;

        // Activer animation de marche via le booléen
        animator.SetBool("isWalking", true);
        Debug.Log("Chat commence à marcher.");

        // Mouvement vers le point
        while (Vector2.Distance(chatRectTransform.anchoredPosition, end) > 5f)
        {
            chatRectTransform.anchoredPosition = Vector2.MoveTowards(
                chatRectTransform.anchoredPosition,
                end,
                walkSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Arrivé → animation Idle
        chatRectTransform.anchoredPosition = end;
        animator.SetBool("isWalking", false);
        Debug.Log("Chat est arrivé et devient idle.");

        isWalking = false;
    }

    public void Meow()
    {
        if (!isWalking)
        {
            animator.SetBool("isWalking", false);
            Debug.Log("Chat : Miaou !");
        }
    }

    public void StartSabotage()
    {
        if (!isWalking)
        {
            animator.SetBool("isWalking", false);
            Debug.Log("Chat : Sabotage !");
        }
    }

    public void Idle()
    {
        if (!isWalking)
        {
            animator.SetBool("isWalking", false);
        }
    }
}
