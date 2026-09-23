using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public void OnMove(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Debug.Log($"Move Input: {moveInput}");

        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"Jumped {context.phase}");
        }

    }
}
