using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MailSorterManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI mailText;
    public Button proButton;
    public Button spamButton;
    public Slider productivitySlider;

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
      

        if (mails.Length == 0)
        {
            Debug.LogError("Aucun mail assigné !");
            return;
        }

        ShowMail();
    }

  

    void ShowMail()
    {
        if (currentIndex >= mails.Length)
        {
            mailText.text = " Tous les mails sont triés !";
            proButton.interactable = false;
            spamButton.interactable = false;
            return;
        }

        mailText.text = mails[currentIndex].content;
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
            Debug.Log("✅ Tri correct !");
        }
        else
        {
            productivity -= wrongPenalty;
            Debug.Log("❌ Mauvais tri !");
        }

        productivity = Mathf.Clamp(productivity, 0f, 100f);
        productivitySlider.value = productivity / 100f;

        currentIndex++;
        ShowMail();
    }
}
