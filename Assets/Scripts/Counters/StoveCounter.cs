using System;
using UnityEngine;


public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStageChangedEventArgs> OnStateChanged;
    public class OnStageChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
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

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = cookingProgress / cookingRecipeSO.cookingTimerMax
                    });

                    if (cookingProgress > cookingRecipeSO.cookingTimerMax) {

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(cookingRecipeSO.output, this);

                        overcookingProgress = 0f;
                        overcookingRecipeSO = GetOvercookingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        state = State.Cooked;
                        OnStateChanged?.Invoke(this, new OnStageChangedEventArgs {
                            state = state
                        });

                    }

                    break;

                case State.Cooked:

                    overcookingProgress += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = overcookingProgress / overcookingRecipeSO.overcookingTimerMax
                    });

                    if (overcookingProgress > overcookingRecipeSO.overcookingTimerMax) {

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(overcookingRecipeSO.output, this);

                        state = State.Overcooking;
                        OnStateChanged?.Invoke(this, new OnStageChangedEventArgs {
                            state = state
                        }); ;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }

                    break;

                case State.Overcooking:
                    break;
            }

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
                OnStateChanged?.Invoke(this, new OnStageChangedEventArgs {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = cookingProgress / cookingRecipeSO.cookingTimerMax
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

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStageChangedEventArgs {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else {

                // The player os not holding anything, so we give them the object on the counter
                this.GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStageChangedEventArgs {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
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
