using UnityEngine;
using DG.Tweening;

public class DraggableObject : MonoBehaviour
{
    public string ingredientType;

    private Vector3 startPos;
    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool overBoard = false;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        SandwichManager.Instance.RegisterIngredientObject(this);
    }
    public void SetOverBoard(bool value)
    {
        overBoard = value;
    }

    void OnMouseDown()
    {
        isDragging = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (overBoard)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;

            if (SandwichManager.Instance != null)
            {
                SandwichManager.Instance.RegisterIngredient(ingredientType, this);
            }
        }
        else
        {
            ReturnToStart();
        }
    }
    public void ResetPosition()
    {
        transform.DOMove(startPos, 0.5f).SetEase(Ease.OutBack);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void PopAndDisappear()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack));
        seq.Append(transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        seq.OnComplete(() => gameObject.SetActive(false));
    }

    void ReturnToStart()
    {
        transform.DOMove(startPos, 0.5f).SetEase(Ease.OutBack);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CuttingBoard"))
        {
            overBoard = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CuttingBoard"))
        {
            overBoard = false;
        }
    }
}
