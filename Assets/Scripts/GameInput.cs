using System;
using UnityEngine;

public class GameInput : MonoBehaviour {

    // public delegate void EventHandler(object sender, EventArgs e);
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private PlayerInputActions playerInputActions;

    private void Awake() {

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable(); // Ativa o ActionMap criado

        // ---------------event------------------------subs-listener------------
        playerInputActions.Player.Interaction.performed += Interaction_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {

        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {

        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetNormalizedMovementVector() {

        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>(); // 2 dimensões pois o movimento é em apenas 2 eixos

        return inputVector.normalized;
    }
}
