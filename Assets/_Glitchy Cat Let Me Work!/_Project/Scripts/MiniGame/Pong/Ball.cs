using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 8f;
    private Rigidbody2D rb;
    public PongGameManager gameManager;

    void Awake() // Appelé avant Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
       
        LaunchBall();
    }

    void LaunchBall()
    {
        // Direction aléatoire pour le démarrage
        float x = Random.Range(0, 2) == 0 ? -1 : 1; 
        float y = Random.Range(0, 2) == 0 ? -1 : 1; 

        rb.linearVelocity = new Vector2(x, y).normalized * initialSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            // Affiche le nom du mur touché : "WallLeft" ou "WallRight"
            Debug.Log("But détecté sur : " + other.name);

            // Appelle le PongGameManager
            gameManager.ScorePoint(other.name);

            ResetBall();
        }
    }

    // Appelé pour réinitialiser la position de la balle
    public void ResetBall()
    {
        rb.linearVelocity = Vector2.zero; // Arrête la balle
        transform.position = Vector2.zero; // Centre la balle
        Invoke("LaunchBall", 1f); // Relance la balle après 1 seconde
    }

  
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle")) 
            // Augmenter légèrement la vitesse de la balle après un rebond sur la raquette
            rb.linearVelocity *= 1.05f; // Augmente la vitesse de 5%
        }
    }
