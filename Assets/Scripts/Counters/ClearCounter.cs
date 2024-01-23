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
            if (!player.HasKitchenObject()) {
                // The player os not holding anything
                // Gives the object on the counter to the player
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
