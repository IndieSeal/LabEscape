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
    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCRadius, groundMask);
        
        float x = Input.GetAxisRaw("Horizontal") == 0 ? 0 : Input.GetAxis("Horizontal");
        float z = Input.GetAxisRaw("Vertical") == 0 ? 0 : Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        controller.Move(moveDirection * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        if(isGrounded && velocity.y < 0) velocity.y = -2f;
        
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCRadius);
    }
}