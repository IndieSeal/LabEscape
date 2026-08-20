using System;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public static event Action<DialogueTarget> OnDialogueStarted;
    public static event Action OnDialogueEnded;
    
    [Serializable]
    public class DialogueTarget
    {
        public Transform cameraAt;
        public Transform lookAt;
        public float cameraFOV = 30;
    }
    
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueTxt;
    private TypewriterCore typewriter;
    
    [SerializeField] private GameObject promptTxt;
    public DialogueTarget Target { get; private set; }
    private bool isTypewriterDone = false;

    protected override void Awake()
    {
        base.Awake();

        typewriter = dialogueTxt.GetComponent<TypewriterCore>();
    }

    void OnEnable()
    {
        typewriter.onTextShowed.AddListener(OnTextShowed);
    }

    void OnDisable()
    {
        typewriter.onTextShowed.RemoveListener(OnTextShowed);
    }

    //These shouldn't be in DialogueManager, they need to be managed by the Triggers.
    public void ShowPrompt() => promptTxt.SetActive(true);
    public void HidePrompt() => promptTxt.SetActive(false);

    // Will change to scriptable objects later (or just in-scene variables, depends on wether I want the camera to be movable to point at things)
    public void TriggerDialogue(string text, DialogueTarget target)
    {
        if(Target != null)
        {
            ContinueDialogue();
            return;
        }
        
        StartDialogue(text, target);
    }
    
    public void StartDialogue(string text, DialogueTarget target)
    {
        isTypewriterDone = false;
        
        Target = target;

        dialogueBox.SetActive(true);
        typewriter.TextAnimator.SetTextToSource(text);

        HidePrompt();
        
        OnDialogueStarted?.Invoke(Target);
    }

    private void EndDialogue()
    {
        typewriter.TextAnimator.SetText("");
        dialogueBox.SetActive(false);
        ShowPrompt();

        Target = null;

        OnDialogueEnded?.Invoke();
    }

    public void ContinueDialogue()
    {
        if (isTypewriterDone)
        {
            // continue
            EndDialogue();
        }
        else
        {
            // skip dialogue if I want to implement that.
        }
    }

    public void OnTextShowed() => isTypewriterDone = true;
}