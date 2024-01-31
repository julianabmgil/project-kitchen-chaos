using UnityEngine;


public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private float volume = 10f;

    private Vector3 mainCameraPosition;

    private void Awake() {

        Instance = this;

        mainCameraPosition = Camera.main.transform.position;
    }

    private void Start() {

        BaseCounter.OnAnyObjectDrop += BaseCounter_OnAnyObjectDrop;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        DeliveryManager.Instance.OnRecipeSucceed += DeliveryManager_OnRecipeSucceed;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        Player.Instance.OnObjectPickup += Player_OnObjectPickup;

        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void BaseCounter_OnAnyObjectDrop(object sender, System.EventArgs e) {

        //BaseCounter baseCounter = sender as BaseCounter;
        //PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
        PlaySound(audioClipRefsSO.objectDrop, mainCameraPosition);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {

        //CuttingCounter cuttingCounter = sender as CuttingCounter;
        //PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
        PlaySound(audioClipRefsSO.chop, mainCameraPosition);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {

        //DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        //PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
        PlaySound(audioClipRefsSO.deliveryFail, mainCameraPosition);
    }

    private void DeliveryManager_OnRecipeSucceed(object sender, System.EventArgs e) {

        //DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        //PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
        PlaySound(audioClipRefsSO.deliverySuccess, mainCameraPosition);
    }

    private void Player_OnObjectPickup(object sender, System.EventArgs e) {

        //PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
        PlaySound(audioClipRefsSO.objectPickup, mainCameraPosition);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {

        //TrashCounter trashCounter = sender as TrashCounter;
        //PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
        PlaySound(audioClipRefsSO.trash, mainCameraPosition);
    }

    public void PlayFootsepsSound() {

        PlaySound(audioClipRefsSO.footstep, mainCameraPosition);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position) {

        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position) {

        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
