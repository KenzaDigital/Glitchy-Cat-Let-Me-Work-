using UnityEngine;
using System.Collections;

public class ChatBehavior : MonoBehaviour
{
    public float walkSpeed = 2f; // Unités Unity (meters/second)
    public Transform[] walkPoints; // Waypoints dans la scène
    public Animator animator;
    public SpriteRenderer chatImage;
    public float walkInterval = 10f; // Pause entre déplacements

    private bool isWalking = false;
    private Transform chatTransform;
    private bool isPeting = false;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (chatImage == null) chatImage = GetComponent<SpriteRenderer>();
        chatTransform = transform;

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

        Transform target = walkPoints[Random.Range(0, walkPoints.Length)];
        Vector3 start = chatTransform.position;
        Vector3 end = target.position;

        // Inverser sprite selon la direction
        // Inverser le sprite avec flipX
        chatImage.flipX = end.x < start.x;


        animator.SetBool("isWalking", true);
       // Debug.Log("Chat commence à marcher.");

        while (Vector3.Distance(chatTransform.position, end) > 0.1f)
        {
            chatTransform.position = Vector3.MoveTowards(
                chatTransform.position,
                end,
                walkSpeed * Time.deltaTime
            );
            yield return null;
        }

        chatTransform.position = end;
        animator.SetBool("isWalking", false);
       // Debug.Log("Chat est arrivé et devient idle.");

        isWalking = false;
    }

    public void Meow()
    {
        if (!isWalking)
        {
            Debug.Log("Chat : Miaou !");
        }
    }

    public void StartSabotage()
    {
        if (!isWalking)
        {
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

    public void Pet ()
    {
        if (!isPeting&& !isWalking )
        {
            isPeting= true;
            Debug.Log("Chat : Je suis content !");
            animator.SetTrigger("CatPet");
            StartCoroutine(EndPetting() );
        }
    }


    IEnumerator EndPetting()
    {
        yield return new WaitForSeconds(1.0f); // Durée de la caresse (à ajuster)
        isPeting = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                Pet();
            }
        }
    }
}

