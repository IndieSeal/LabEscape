using System;
using UnityEngine;

public class PlayerMovHandler : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private CharacterController controller;
    
    [Header("Is Grounded")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCRadius = 0.2f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    
    [Header("Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float gravity = -9.8f;
    private Vector3 velocity;
    private bool canMove = true;

    private IInteractable latestInteractable;

    protected PlayerInputHandler Input => PlayerInputHandler.Instance;

    void OnEnable()
    {
        DialogueManager.OnDialogueStarted += DisablePlayerMovement;
        DialogueManager.OnDialogueEnded += EnablePlayerMovement;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= DisablePlayerMovement;
        DialogueManager.OnDialogueEnded -= EnablePlayerMovement;
    }

    void Update()
    {
        TryInteract();
        if(!canMove) return;
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCRadius, groundMask);
        
        Vector2 mov = Input.Movement;

        Vector3 moveDirection = transform.right * mov.x + transform.forward * mov.y;
        controller.Move(moveDirection * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        if(isGrounded && velocity.y < 0) velocity.y = -2f;
        
        controller.Move(velocity * Time.deltaTime);
    }

    public bool IsGrounded(out Collider collider)
    {
        collider = null;
        
        var targets = Physics.OverlapSphere(groundCheck.position, groundCRadius, groundMask);
        if(targets.Length > 0) collider = targets[0];
        
        return targets.Length > 0;
    }

    private void DisablePlayerMovement(DialogueManager.DialogueTarget target = null)
    {
        canMove = false;
    }

    private void EnablePlayerMovement()
    {
        canMove = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interac))
        {
            interac.OnEnter();
            latestInteractable = interac;
        }
    }

    private void TryInteract()
    {
        if(latestInteractable == null) return;

        if (Input.WasInteractPressed) latestInteractable.OnInteract();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interac))
        {
            interac.OnExit();
            latestInteractable = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCRadius);
    }
}