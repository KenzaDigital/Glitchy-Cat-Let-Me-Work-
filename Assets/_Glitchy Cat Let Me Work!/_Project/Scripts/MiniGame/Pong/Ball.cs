using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 8f;
    private Rigidbody2D rb;
    public PongGameManager manager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        LaunchBall();
    }

    void LaunchBall()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(-0.5f, 0.5f);
        rb.linearVelocity = new Vector2(x, y).normalized * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // ✅ Joue le son "Blac" à chaque contact avec un paddle
            audioManager.instance.PlaySFX("Blah");

            float y = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            float directionX = rb.linearVelocity.x > 0 ? 1 : -1;
            Vector2 dir = new Vector2(directionX, y).normalized;
            rb.linearVelocity = dir * Mathf.Max(rb.linearVelocity.magnitude * 1.1f, initialSpeed);
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            if (manager != null)
                manager.ScorePoint(collision.gameObject.name);

            ResetBall();
        }
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Debug.Log("Ball reset, waiting to relaunch...");
        Invoke(nameof(LaunchBall), 1f);
    }
}
