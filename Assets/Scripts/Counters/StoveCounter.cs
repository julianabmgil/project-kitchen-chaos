using UnityEngine;


public class StoveCounter : BaseCounter
{
    private enum State {
        Idle,
        Cooking,
        Cooked,
        Overcooking,
    }

    [SerializeField] private CookingRecipeSO[] cookingRecipesSO;
    [SerializeField] private OvercookingRecipeSO[] overcookingRecipesSO;

    private State state;

    private CookingRecipeSO cookingRecipeSO;
    private OvercookingRecipeSO overcookingRecipeSO;
    private float cookingProgress;
    private float overcookingProgress;


    private void Start() {

        state = State.Idle;
    }

    private void Update() {

        if (HasKitchenObject()) {

            switch (state) {

                case State.Idle:
                    break;

                case State.Cooking:

                    cookingProgress += Time.deltaTime;

                    if (cookingProgress > cookingRecipeSO.cookingTimerMax) {

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(cookingRecipeSO.output, this);

                        state = State.Cooked;
                        overcookingProgress = 0f;
                        overcookingRecipeSO = GetOvercookingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }

                    break;

                case State.Cooked:

                    overcookingProgress += Time.deltaTime;

                    if (overcookingProgress > overcookingRecipeSO.overcookingTimerMax) {

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(overcookingRecipeSO.output, this);

                        state = State.Overcooking;
                        Debug.Log("Object is overcooked!");
                    }

                    break;

                case State.Overcooking:
                    break;
            }

            Debug.Log(state);

        }
    }

    public override void Interact(Player player) {

        if (!HasKitchenObject()) {

            // The counter is free to hold a kitchen object
            if (player.HasKitchenObject() && KitchenObjectSOIsCookingRecipeSOInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                // The player is holding a kitchen object and this kitchen object is an input for a cooking recipe
                // Gives the object on the player to the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);


                cookingRecipeSO = GetCookingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                cookingProgress = 0f;
                state = State.Cooking;

            }
            else {
                // The player is not holding anything
            }
        }
        else {
            // The counter is already holding a kitchen object
            if (!player.HasKitchenObject()) {

                this.GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
            }
        }
    }

    private bool KitchenObjectSOIsCookingRecipeSOInput(KitchenObjectSO kitchenObjectSO) {

        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(kitchenObjectSO);

        return cookingRecipeSO != null;
    }

    private CookingRecipeSO GetCookingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipesSO) {

            if (cookingRecipeSO.input == inputKitchenObjectSO) {

                return cookingRecipeSO;
            }
        }

        return null;
    }
    
    private OvercookingRecipeSO GetOvercookingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (OvercookingRecipeSO overcookingRecipeSO in overcookingRecipesSO) {

            if (overcookingRecipeSO.input == inputKitchenObjectSO) {

                return overcookingRecipeSO;
            }
        }

        return null;
    }
}
