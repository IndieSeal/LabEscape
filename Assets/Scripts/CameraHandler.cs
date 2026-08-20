using UnityEngine;
using static DialogueManager;

public class CameraHandler : Singleton<CameraHandler>
{
    public enum EState
    {
        Player,
        Dialogue
    }
    
    [SerializeField] private float mouseSens = 100f;
    [SerializeField] private Transform playerBody;
    private Camera cam;
    private float xRotation = 0;

    public EState CurrentState = EState.Player;

    private DialogueTarget dialogueTarget;
    private Quaternion prevQuat;
    private Vector3 prevPos;
    private float prevFOV;

    protected override void Awake()
    {
        base.Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        OnDialogueStarted += StartDialogue;
        OnDialogueEnded += EndDialogue;
    }

    void OnDisable()
    {
        OnDialogueStarted -= StartDialogue;
        OnDialogueEnded -= EndDialogue;
    }

    void Update()
    {
        if(CurrentState == EState.Player) HandlePlayer();
        else if(CurrentState == EState.Dialogue) HandleDialogue();
    }

    private void HandlePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleDialogue()
    {
        transform.position = dialogueTarget.cameraAt.position;
        transform.LookAt(dialogueTarget.lookAt);

        cam.fieldOfView = dialogueTarget.cameraFOV;
    }

    public void StartDialogue(DialogueTarget target)
    {
        CurrentState = EState.Dialogue;

        prevFOV = cam.fieldOfView;
        prevPos = transform.position;
        prevQuat = transform.rotation;
        dialogueTarget = target;
    }

    public void EndDialogue()
    {
        CurrentState = EState.Player;

        transform.position = prevPos;
        transform.rotation = prevQuat;
        cam.fieldOfView = prevFOV;
    }
}