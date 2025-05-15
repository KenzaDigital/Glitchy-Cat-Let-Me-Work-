using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailSorterManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI mailText;
    public Button proButton;
    public Button spamButton;
    public Slider productivitySlider;
    public Canvas CanvasMail;  // Référence au Canvas contenant le mail
    public Button openMailButton;  // Référence au bouton "Open Mail"
   

    [Header("Données")]
    public MailData[] mails;

    [Header("Réglages")]
    public float productivity = 100f;
    public float correctBonus = 10f;
    public float wrongPenalty = 15f;

    private int currentIndex = 0;

    void Start()
    {
        Debug.Log("Nombre de mails reçus : " + mails.Length);
        CanvasMail.gameObject.SetActive(false);  // Assure que le canvas est désactivé au début
        ShowMail();

        if (mails.Length == 0)
        {
            Debug.LogError("Aucun mail assigné !");
            return;
        }

        // Assigner la fonction du bouton "Open Mail"
        openMailButton.onClick.AddListener(OpenMail);
    }

    void ShowMail()
    {
        if (currentIndex >= mails.Length)
        {
            mailText.text = "Tous les mails sont triés !";
            proButton.interactable = false;
            spamButton.interactable = false;
            return;
        }

        mailText.text = mails[currentIndex].content;
    }

    // Fonction pour ouvrir le Canvas Mail
    public void OpenMail()
    {
        CanvasMail.gameObject.SetActive(true);  // Activer le canvas pour afficher le mail
        Debug.Log("Mail ouvert !");
    }

    public void SortAsPro()
    {
        HandleSort(true); // L'utilisateur dit "Pro"
    }

    public void SortAsSpam()
    {
        HandleSort(false); // L'utilisateur dit "Spam"
    }

    void HandleSort(bool userSaysPro)
    {
        if (currentIndex >= mails.Length)
            return;

        bool mailIsPro = mails[currentIndex].isPro;

        if (mailIsPro == userSaysPro)
        {
            productivity += correctBonus;
            Debug.Log(" Tri correct !");
        }
        else
        {
            productivity -= wrongPenalty;
            Debug.Log(" Mauvais tri !");
        }

        productivity = Mathf.Clamp(productivity, 0f, 100f);
        productivitySlider.value = productivity / 100f;

        currentIndex++;
        ShowMail();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasMail.gameObject.SetActive(false);  // Désactiver le canvas si l'utilisateur appuie sur Échap
            Debug.Log("Mail fermé !");
        }
    }
}
