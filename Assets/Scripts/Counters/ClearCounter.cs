using UnityEngine;


public class ClearCounter : BaseCounter, IKitchenObjectParent {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player) {

        if (!HasKitchenObject()) {

            // The counter is free to hold a kitchen object
            if (player.HasKitchenObject()) {

                // The player is holding a kitchen object
                // Gives the object on the player to the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                // The player is not holding anything
            }
        }
        else {

            // The counter is already holding a kitchen object
            if (player.HasKitchenObject()) {

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {

                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {

                        this.GetKitchenObject().DestroySelf();
                    }
                }
                else {

                    // Player is holding something other than a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {

                        // Counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {

                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else {

                // The player os not holding anything, so we give them the object on the counter
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
