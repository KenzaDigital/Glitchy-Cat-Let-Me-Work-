using UnityEngine;
using TMPro;

public class MailSorterManager : MonoBehaviour
{
    public TextMeshProUGUI mailContentText;  // Texte à afficher
    public MailData[] mails;  // Tableau des mails

    private int currentIndex = 0;  // Suivi du mail actuel

    void Start()
    {
        ShowCurrentMail();
    }

    void ShowCurrentMail()
    {
        if (mails.Length > 0 && currentIndex < mails.Length)
        {
            MailData currentMail = mails[currentIndex];
            mailContentText.text = currentMail.content;
        }
    }

    public void SortAsPro()
    {
        CheckMail(false);  // false = non-spam
    }

    public void SortAsSpam()
    {
        CheckMail(true);  // true = spam
    }

    void CheckMail(bool userSaysSpam)
    {
        bool isCorrect = mails[currentIndex].isSpam == userSaysSpam;
        Debug.Log("Tri correct ? " + isCorrect);

        // Passer au mail suivant
        currentIndex++;
        if (currentIndex < mails.Length)
            ShowCurrentMail();
        else
            Debug.Log("Tous les mails triés !");
    }
}
