using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public float speed = 6f;
    public Transform ball;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (ball == null)
            return;

        // Suivre la position Y de la balle uniquement
        float direction = ball.position.y > transform.position.y ? 1 : -1;
        float distance = Mathf.Abs(ball.position.y - transform.position.y);

        if (distance > 0.2f)
        {
            rb.linearVelocity = new Vector2(0f, direction * speed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
