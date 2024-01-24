﻿using System.Collections.Generic;
using UnityEngine;


public class PlateKitchenObject : KitchenObject {

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    public void Awake() {

        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {

        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Not a valid ingredient
            return false;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Already has this ingredient
            return false;
        }

        kitchenObjectSOList.Add(kitchenObjectSO);

        return true;
    }
}