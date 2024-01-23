using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;


    public virtual void Interact(Player player) {

        Debug.LogError("Interact() should not be called from the base BaseCounter class");
    }

    public virtual void InteractAlternate(Player player) {

        Debug.LogError("InteractAlternate() should not be called from the base BaseCounter class");
    }

    public Transform GetKitchenObjectFollowTransform() {

        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject() {

        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {

        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject() {

        kitchenObject = null;
    }

    public bool HasKitchenObject() {

        return kitchenObject != null;
    }
}