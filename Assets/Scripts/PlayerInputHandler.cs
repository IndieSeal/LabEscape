using UnityEngine;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    public Vector2 Movement => GetMovement();
    public Vector2 MouseMovement => GetMouseMovement();

    public bool WasInteractPressed => Input.GetKeyDown(KeyCode.E);
    public bool WasFlashlightPressed => Input.GetKeyDown(KeyCode.F);

    private Vector2 GetMovement()
    {
        float x = Input.GetAxisRaw("Horizontal") == 0 ? 0 : Input.GetAxis("Horizontal");
        float z = Input.GetAxisRaw("Vertical") == 0 ? 0 : Input.GetAxis("Vertical");

        return new Vector2(x, z);
    }

    private Vector2 GetMouseMovement()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        return new Vector2(x, y);
    }
}