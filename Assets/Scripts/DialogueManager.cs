using System;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public static Action<DialogueTarget> OnDialogueStarted;
    public static Action OnDialogueEnded;
    
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

    private DialogueSO dialogueSO;
    private int index;
    
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

    // Will change to scriptable objects later (or just in-scene variables, depends on wether I want the camera to be movable to point at things)
    public void TriggerDialogue(DialogueSO text, DialogueTarget target)
    {
        if(Target != null)
        {
            ContinueDialogue();
            return;
        }
        
        StartDialogue(text, target);
    }
    
    public void StartDialogue(DialogueSO text, DialogueTarget target)
    {
        isTypewriterDone = false;
        
        Target = target;

        dialogueSO = text;
        index = 0;

        UpdateDialogue();

        OnDialogueStarted?.Invoke(Target);
    }

    private void EndDialogue()
    {
        typewriter.TextAnimator.SetText("");
        dialogueBox.SetActive(false);

        Target = null;

        OnDialogueEnded?.Invoke();
    }

    public void ContinueDialogue()
    {
        if (isTypewriterDone)
        {
            index++;
            
            if(index >= dialogueSO.dialogues.Count) EndDialogue();
            else UpdateDialogue();
        }
        else
        {
            // skip dialogue if I want to implement that.
        }
    }

    private void UpdateDialogue()
    {
        isTypewriterDone = false;
        
        dialogueBox.SetActive(true);
        typewriter.TextAnimator.SetTextToSource(dialogueSO.dialogues[index]);
    }

    public void OnTextShowed() => isTypewriterDone = true;
}