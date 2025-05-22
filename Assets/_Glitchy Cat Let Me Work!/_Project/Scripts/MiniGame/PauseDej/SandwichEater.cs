using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SandwichEater : MonoBehaviour
{
    public Button tapButton;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI successText;
    public GameObject sandwichDonePanel; // Le panel complet à cacher à la fin

    public int tapsToEat = 10;
    private int currentTaps = 0;
    private bool finished = false;

    void Start()
    {
        counterText.text = $"Crocs : {currentTaps}/{tapsToEat}";
        successText.gameObject.SetActive(false);
        tapButton.onClick.AddListener(OnTap);
    }

    void OnTap()
    {
        if (finished) return;

        currentTaps++;
        tapButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 1);
        counterText.text = $"Crocs: {currentTaps}/{tapsToEat}";

        if (currentTaps >= tapsToEat)
        {
            FinishEating();
        }
    }

    void FinishEating()
    {
        finished = true;
        tapButton.interactable = false;
        successText.gameObject.SetActive(true);
        successText.text = "😋 Miam ! Sandwich mangé !";

        Sequence endSequence = DOTween.Sequence();

        // Animation de disparition du sandwich panel
        endSequence.Append(sandwichDonePanel.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        endSequence.AppendCallback(() =>
        {
            sandwichDonePanel.SetActive(false); // le cacher complètement
        });
    }
}
