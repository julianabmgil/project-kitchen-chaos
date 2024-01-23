using UnityEngine;


public class StoveCounter : BaseCounter
{
    [SerializeField] private CookingRecipeSO[] cookingRecipesSO;

    private int cookingProgress;


    public override void Interact(Player player) {

        if (!HasKitchenObject()) {

            // The counter is free to hold a kitchen object
            if (player.HasKitchenObject() && KitchenObjectSOIsCookingRecipeSOInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                // The player is holding a kitchen object and this kitchen object is an input for a cooking recipe
                // Gives the object on the player to the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);

                cookingProgress = 0;
                CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                //OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                //    progressNormalized = (float)cookingProgress / cookingRecipeSO.cookingTimerMax
                //});
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

    private bool KitchenObjectSOIsCookingRecipeSOInput(KitchenObjectSO kitchenObjectSO) {

        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(kitchenObjectSO);

        return cookingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {

        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputKitchenObjectSO);

        if (cookingRecipeSO != null) {
            return cookingRecipeSO.output;
        }

        return null;
    }

    private CookingRecipeSO GetCookingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipesSO) {

            if (cookingRecipeSO.input == inputKitchenObjectSO) {

                return cookingRecipeSO;
            }
        }

        return null;
    }
}
