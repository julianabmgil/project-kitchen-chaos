using System;
using System.Collections.Generic;
using UnityEngine;


public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject {

        public KitchenObjectSO kitchenObject;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;


    public void Start() {

        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList) {

            if (kitchenObjectSOGameObject.kitchenObject == e.kitchenObjectSO) {

                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
