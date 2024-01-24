using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int numberOfSpawnedPlates;
    private int maxOfSpawnedPlates = 4;

    private void Update() {

        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax) {

            spawnPlateTimer = 0f;

            if (numberOfSpawnedPlates < maxOfSpawnedPlates) {

                numberOfSpawnedPlates++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        
        if (!player.HasKitchenObject()) {

            if (numberOfSpawnedPlates > 0) {

                numberOfSpawnedPlates--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
