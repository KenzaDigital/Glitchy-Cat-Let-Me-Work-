using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform ballTransform;
    public float speed = 8f;
    public float yLimit = 4.5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (ballTransform == null)
            return;

        float direction = 0f;

        if (ballTransform.position.y > transform.position.y + 0.1f)
            direction = 1f;
        else if (ballTransform.position.y < transform.position.y - 0.1f)
            direction = -1f;

        Vector2 newPos = rb.position + Vector2.up * direction * speed * Time.deltaTime;
        newPos.y = Mathf.Clamp(newPos.y, -yLimit, yLimit);

        rb.MovePosition(newPos);
    }
}
