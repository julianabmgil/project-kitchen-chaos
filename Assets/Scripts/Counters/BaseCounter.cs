using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    // Since we can have multiple counters, a static event allows for one single subscription
    public static event EventHandler OnAnyObjectDrop;

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;


    public virtual void Interact(Player player) {

        Debug.LogError("Interact() should not be called from the base BaseCounter class");
    }

    public virtual void InteractAlternate(Player player) {

        //Debug.LogError("InteractAlternate() should not be called from the base BaseCounter class");
    }

    public Transform GetKitchenObjectFollowTransform() {

        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject() {

        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {

        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {

            OnAnyObjectDrop?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject() {

        kitchenObject = null;
    }

    public bool HasKitchenObject() {

        return kitchenObject != null;
    }
}
