using System;

public class TrashCounter : BaseCounter
{
    // Since we can have multiple trash counters, a static event allows for one single subscription
    public static event EventHandler OnAnyObjectTrashed;

    public override void Interact(Player player) {

        if (player.HasKitchenObject()) {
            player.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
