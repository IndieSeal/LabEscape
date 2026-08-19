using UnityEngine;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    public Vector2 Movement => GetMovement();
    public bool WasInteractPressed => Input.GetKeyDown(KeyCode.E);

    private Vector2 GetMovement()
    {
        float x = Input.GetAxisRaw("Horizontal") == 0 ? 0 : Input.GetAxis("Horizontal");
        float z = Input.GetAxisRaw("Vertical") == 0 ? 0 : Input.GetAxis("Vertical");

        return new Vector2(x, z);
    }
}