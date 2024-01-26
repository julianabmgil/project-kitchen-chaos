using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI orderNameText;
    [SerializeField] private Transform orderRecipeIconsContainer;
    [SerializeField] private Transform ingredientIconTemplate;

    private void Awake() {

        ingredientIconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO) {

        orderNameText.text = recipeSO.recipeName;

        foreach (Transform child in orderRecipeIconsContainer) {

            if (child == ingredientIconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList) {

            Transform iconTransform = Instantiate(ingredientIconTemplate, orderRecipeIconsContainer);

            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
