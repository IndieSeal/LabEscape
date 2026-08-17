using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float mouseSens = 100f;
    [SerializeField] private Transform playerBody;
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);
    }
}