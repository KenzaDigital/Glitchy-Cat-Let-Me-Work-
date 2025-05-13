using UnityEngine;

[CreateAssetMenu(fileName = "NewMail", menuName = "Mail/MailData")]
public class MailData : ScriptableObject
{
    [TextArea(2, 5)]
    public string content;
    public bool isPro; // true = pro, false = spam
    internal bool isSpam;
}
