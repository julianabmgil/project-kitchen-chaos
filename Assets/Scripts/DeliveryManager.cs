using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance { get; private set; } // singleton

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;
    public event EventHandler OnRecipeSucceed;
    public event EventHandler OnRecipeFailed;


    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private float waitingRecipesMax = 4f;

    private int amountOfSucceededDeliveries = 0;


    private void Awake() {

        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {

        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f) {

            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax) {

                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {

        for (int i = 0; i < waitingRecipeSOList.Count; i++) {

            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {

                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {

                    bool ingredientFound = false;

                    // Cycling through all ingredients in the recipe
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {

                        // Cycling throught all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {

                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound) {

                        // This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe) {

                    // Player delivered a recipe that was on the waiting list
                    amountOfSucceededDeliveries++;

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                    OnRecipeSucceed?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }

        // No matches found
        // The recipe the player delivered was not on the waiting list
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {

        return waitingRecipeSOList;
    }

    public int GetAmountOfSucceededDeliveries() {

        return amountOfSucceededDeliveries;
    }
}
