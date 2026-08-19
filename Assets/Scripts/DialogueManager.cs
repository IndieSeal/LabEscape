using System;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [Serializable]
    public class DialogueTarget
    {
        public Transform cameraAt;
        public Transform lookAt;
        public float cameraFOV = 30;
    }
    
    [SerializeField] private TMP_Text dialogueTxt;
    [SerializeField] private GameObject promptTxt;
    public DialogueTarget Target { get; private set; }

    public void ShowPrompt() => promptTxt.SetActive(true);
    public void HidePrompt() => promptTxt.SetActive(false);

    // Will change to scriptable objects later (or just in-scene variables, depends on wether I want the camera to be movable to point at things)
    public void TriggerDialogue(string text, DialogueTarget target)
    {
        Target = target;

        dialogueTxt.gameObject.SetActive(true);
        dialogueTxt.text = text;

        HidePrompt();
        
        CameraHandler.Instance.StartDialogue(target);
    }
}