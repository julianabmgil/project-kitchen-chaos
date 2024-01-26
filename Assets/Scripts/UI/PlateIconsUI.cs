using UnityEngine;


public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;


    private void Awake() {

        iconTemplate.gameObject.SetActive(false);
    }

    public void Start() {

        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {

        UpdateVisual();
    }

    private void UpdateVisual() {

        // Cleanup the icons before spawning them again with the new one
        foreach (Transform child in this.transform) {

            if (child == iconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {

            Transform iconTransform = Instantiate(iconTemplate, this.transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
