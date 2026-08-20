using UnityEngine;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] private GameObject promptTxt;

    void OnEnable()
    {
        DialogueManager.OnDialogueStarted += HidePrompt;
        DialogueManager.OnDialogueEnded += ShowPrompt;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= HidePrompt;
        DialogueManager.OnDialogueEnded -= ShowPrompt;
    }

    public void ShowPrompt() => promptTxt.SetActive(true);
    public void HidePrompt(DialogueManager.DialogueTarget t = null) => promptTxt.SetActive(false);
}