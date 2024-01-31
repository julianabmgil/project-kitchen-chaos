using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; } // O C# define o campo referente automaticamente "por baixo dos panos"

    public event EventHandler OnObjectPickup;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField, Tooltip("The game object that contains the GameInput script")] private GameInput gameInput;
    [SerializeField, Tooltip("The speed in wich the player character will move")]   private float movementSpeed = 7.0f; // Serialized private ao invés de public protege o field do acesso inadequado de outras classes
    [SerializeField, Tooltip("The layer where the raycast will target for the counters")] private LayerMask countersLayerMask;
    [SerializeField, Tooltip("The position where the ingredient will be spawn above the counter")] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractionDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() {

        if (Instance != null) {
            Debug.Log("There is more than one Player instance");
        }

        Instance = this;
    }

    private void Start() {

        gameInput.OnInteractAction += GameInput_OnInteraction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e) {

        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update() {

        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement() {

        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        Vector3 movementDirection = new Vector3(inputVector.x, 0, inputVector.y); // 3 dimensões pois é referente à posição do GameObject no world space

        bool isColliding = CheckPlayerCollision(movementDirection);

        if (isColliding) {
            // Se está colidindo em algum eixo, pode ser que esteja em movimento diagonal. Por isso, detectamos o eixo de colisão para tentar mover no outro eixo

            // Try to move through the X
            Vector3 xMovementDirection = new Vector3(movementDirection.x, 0, 0).normalized;
            isColliding = xMovementDirection.x == 0 || CheckPlayerCollision(xMovementDirection);

            if (!isColliding) {
                // Can move only trough X
                movementDirection = xMovementDirection;
            }
            else {
                // Try to move through Z
                Vector3 zMovementDirection = new Vector3(0, 0, movementDirection.z).normalized;
                isColliding = zMovementDirection.z == 0 ||  CheckPlayerCollision(zMovementDirection);

                if (!isColliding) {
                    // Can move only trough Z
                    movementDirection = zMovementDirection;
                }
            }
        }

        if (!isColliding) {
            // Move
            transform.position += (movementDirection * movementSpeed) * Time.deltaTime; // Multiplicar pelo deltaTime desassocia a velocidade de movimento do framerate
        }
        // Pesquisar métodos de rotação abaixo:
        // transform.rotation
        // transform.eulerAngles
        // transform.LookAt()
        // transform.up
        // transform.right
        // Vector3.Lerp vs Slerp
        float rotationSpeed = movementSpeed * 2f;
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotationSpeed); // Rotaciona o GameObject para encarar a direção do movimento        

        isWalking = movementDirection != Vector3.zero;
    }

    private void HandleInteractions() {

        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        Vector3 movementDirection = new Vector3(inputVector.x, 0, inputVector.y); // 3 dimensões pois é referente à posição do GameObject no world space

        if (movementDirection != Vector3.zero) {
            lastInteractionDirection = movementDirection;
        }

        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDirection, out RaycastHit raycastHit, interactionDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter)) {
                // Is/Has ClearCounter
                //clearCounter.Interact();
                if (counter != selectedCounter) {
                    SetSelectedCounter(counter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    private bool CheckPlayerCollision(Vector3 direction) {

        float playerVisualHeight = 2f;
        float playerVisualRadius = 0.7f;

        return Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerVisualHeight, playerVisualRadius, direction, movementSpeed * Time.deltaTime);
    }

    public bool IsWalking() {

        return isWalking;
    }

    public Transform GetKitchenObjectFollowTransform() {

        return kitchenObjectHoldPoint;
    }

    public KitchenObject GetKitchenObject() {

        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {

        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {

            OnObjectPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject() {

        kitchenObject = null;
    }

    public bool HasKitchenObject() {

        return kitchenObject != null;
    }
}