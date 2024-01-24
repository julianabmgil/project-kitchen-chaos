using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipesSO;

    private int cuttingProgress;

    public override void Interact(Player player) {

        if (!HasKitchenObject()) {

            // The counter is free to hold a kitchen object
            if (player.HasKitchenObject() && KitchenObjectSOIsCuttingRecipeSOInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                // The player is holding a kitchen object and this kitchen object is an input for a cutting recipe
                // Gives the object on the player to the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);

                cuttingProgress = 0;
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
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
            }
            else {

                // The player os not holding anything, so we give them the object on the counter
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {

        KitchenObjectSO kitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();

        if (HasKitchenObject() && KitchenObjectSOIsCuttingRecipeSOInput(kitchenObjectSO)) {

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);

            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {

                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(kitchenObjectSO);

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool KitchenObjectSOIsCuttingRecipeSOInput(KitchenObjectSO kitchenObjectSO) {

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipesSO) {

            if (cuttingRecipeSO.input == inputKitchenObjectSO) {

                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
