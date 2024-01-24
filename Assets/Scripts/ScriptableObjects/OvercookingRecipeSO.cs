using UnityEngine;


[CreateAssetMenu()]
public class OvercookingRecipeSO : ScriptableObject {

    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float overcookingTimerMax;
}
