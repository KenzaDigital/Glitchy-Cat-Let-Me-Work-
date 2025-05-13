using UnityEngine;
using UnityEngine.UI;

public class MailSortingMinigame : MonoBehaviour
{
    [System.Serializable]
    public class Mail
    {
        public string content;
        public bool isProfessional;
    }

    public Text mailText;
    public Button buttonPro;
    public Button buttonSpam;
    public ProductivityMeter prodMeter;

    public Mail[] mailList;
    private int currentMail = 0;

    void Start()
    {
        ShowNextMail();
    }

    void ShowNextMail()
    {
        if (currentMail >= mailList.Length)
        {
            // Fin du mini-jeu
            mailText.text = "Tous les mails sont triés !";
            buttonPro.interactable = false;
            buttonSpam.interactable = false;
            return;
        }

        mailText.text = mailList[currentMail].content;
    }

    public void ChoosePro()
    {
        if (mailList[currentMail].isProfessional)
            prodMeter.ReduceWaste(10f);
        else
            prodMeter.current -= 5f;

        currentMail++;
        ShowNextMail();
    }

    public void ChooseSpam()
    {
        if (!mailList[currentMail].isProfessional)
            prodMeter.ReduceWaste(10f);
        else
            prodMeter.current -= 5f;

        currentMail++;
        ShowNextMail();
    }
}
