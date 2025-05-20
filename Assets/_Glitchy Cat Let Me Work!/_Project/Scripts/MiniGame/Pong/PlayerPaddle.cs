using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public float speed = 8f;
    public string inputAxis = "Vertical";// utiliser "W" ou "S"

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw(inputAxis);
        rb.linearVelocity = new Vector2(0f, move) * speed;
    }
}

