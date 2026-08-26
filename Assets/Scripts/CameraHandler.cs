using System;
using System.Linq;
using UnityEngine;
using static DialogueManager;

public enum EPlayerState
{
    Player,
    Dialogue,
    Hiding
}

public class CameraHandler : Singleton<CameraHandler>
{
    public static event Action<EPlayerState> OnPlayerStateChanged;
    public static event Action OnResumePlayer;
    public static event Action OnStopPlayer;

    protected PlayerInputHandler PInput => PlayerInputHandler.Instance;
    
    [SerializeField] private float mouseSens = 100f;
    [SerializeField] private Transform playerBody;
    private Camera cam;
    private float xRotation = 0;

    public EPlayerState CurrentState { get => currentState; set
        {
            currentState = value;
            OnPlayerStateChanged?.Invoke(currentState);
            if(currentState != EPlayerState.Player) OnStopPlayer?.Invoke();
            else OnResumePlayer?.Invoke();
        }
    }
    protected EPlayerState currentState = EPlayerState.Player;

    private DialogueTarget dialogueTarget;
    private Quaternion prevQuat;
    private Vector3 prevPos;
    private Vector3 prevDir;
    private float prevFOV;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 4f;
    private IInteractable latestInteractable;

    protected Vector3 RaycastStart => CurrentState == EPlayerState.Player ? transform.position : prevPos;
    protected Vector3 RaycastDirection => CurrentState == EPlayerState.Player ? transform.forward : prevDir;

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

        Locker.OnHidingStart += StartHiding;
        Locker.OnHidingEnded += EndDialogue;
    }

    void OnDisable()
    {
        OnDialogueStarted -= StartDialogue;
        OnDialogueEnded -= EndDialogue;

        Locker.OnHidingStart -= StartHiding;
        Locker.OnHidingEnded -= EndDialogue;
    }

    void Update()
    {
        HandleInteractions();
        
        if(CurrentState == EPlayerState.Player) HandlePlayer();
        else if(CurrentState == EPlayerState.Dialogue || CurrentState == EPlayerState.Hiding) HandleTarget();
    }

    private void HandleInteractions()
    {
        var hits = Physics.RaycastAll(RaycastStart, RaycastDirection, interactionDistance)
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
        float mouseX = PInput.MouseMovement.x * mouseSens * Time.deltaTime;
        float mouseY = PInput.MouseMovement.y * mouseSens * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleTarget()
    {
        transform.position = dialogueTarget.cameraAt.position;
        transform.LookAt(dialogueTarget.lookAt);

        cam.fieldOfView = dialogueTarget.cameraFOV;
    }

    public void StartDialogue(DialogueTarget target)
    {
        CurrentState = EPlayerState.Dialogue;
        SetPreviousAndLooks(target);
    }

    public void StartHiding(DialogueTarget target)
    {
        CurrentState = EPlayerState.Hiding;
        SetPreviousAndLooks(target);
    }

    private void SetPreviousAndLooks(DialogueTarget target)
    {
        prevFOV = cam.fieldOfView;
        prevPos = transform.position;
        prevDir = transform.forward;
        prevQuat = transform.rotation;
        dialogueTarget = target;
    }

    public void EndDialogue()
    {
        CurrentState = EPlayerState.Player;

        transform.position = prevPos;
        transform.rotation = prevQuat;
        cam.fieldOfView = prevFOV;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = latestInteractable == null ? Color.red : Color.green;
        Gizmos.DrawRay(RaycastStart, RaycastDirection * interactionDistance);        
    }
}