using UnityEngine;
using UnityEngine.InputSystem;

public class InteractInputAction : MonoBehaviour
{
    [Header("Player Actions")]
    [SerializeField]
    public bool playerInteract;

    public void OnInteract(InputValue interactValue)
    {
        InteractInput(interactValue.isPressed);
    }
    public void InteractInput(bool newInteractState)
    {
        playerInteract = newInteractState;
    }
}
