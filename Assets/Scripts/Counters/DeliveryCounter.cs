
public class DeliveryCounter : BaseCounter
{
    // Based on design, there will be only one delivery counter on the game
    public static DeliveryCounter Instance { get; private set; }

    private void Awake() {

        Instance = this;
    }

    public override void Interact(Player player) {

        if (player.HasKitchenObject()) {

            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                plateKitchenObject.DestroySelf();
            }
        }
    }
}
