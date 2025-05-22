using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 startPos;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private bool overBoard = false;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Stoppe la physique pendant le drag
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (overBoard)
        {
            // Laisse l’objet sur la planche, sans gravité
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            // Revenir à la position d’origine et reprendre la gravité
            StartCoroutine(ReturnToStart());
        }
    }

    private System.Collections.IEnumerator ReturnToStart()
    {
        Vector3 currentPos = transform.position;
        float elapsed = 0f;
        float duration = 0.3f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(currentPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Appelé par la planche à découper via OnTriggerEnter2D/Exit2D
    public void SetOverBoard(bool isOver)
    {
        overBoard = isOver;
    }
}
