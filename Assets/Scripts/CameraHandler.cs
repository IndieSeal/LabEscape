using System.Linq;
using UnityEngine;
using static DialogueManager;

public class CameraHandler : Singleton<CameraHandler>
{
    public enum EState
    {
        Player,
        Dialogue
    }

    protected PlayerInputHandler PInput => PlayerInputHandler.Instance;
    
    [SerializeField] private float mouseSens = 100f;
    [SerializeField] private Transform playerBody;
    private Camera cam;
    private float xRotation = 0;

    public EState CurrentState = EState.Player;

    private DialogueTarget dialogueTarget;
    private Quaternion prevQuat;
    private Vector3 prevPos;
    private float prevFOV;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 4f;
    private IInteractable latestInteractable;

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
        HandleInteractions();
        
        if(CurrentState == EState.Player) HandlePlayer();
        else if(CurrentState == EState.Dialogue) HandleDialogue();
    }

    private void HandleInteractions()
    {
        var hits = Physics.RaycastAll(transform.position, transform.forward, interactionDistance)
            .Select(x => x.collider.GetComponent<IInteractable>()).Where(x => x != null).ToList();
        if(latestInteractable == null && hits.Count > 0)
        {
            latestInteractable = hits[0];
            latestInteractable.OnEnter();
        }
        else if(latestInteractable != null)
        {
            if(!hits.ToList().Contains(latestInteractable))
            {
                latestInteractable.OnExit();
                latestInteractable = null;
            }
            else if(PInput.WasInteractPressed) latestInteractable.OnInteract();
        }
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

    void OnDrawGizmos()
    {
        Gizmos.color = latestInteractable == null ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * interactionDistance);        
    }
}