using TMPro;
using UnityEngine;


public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;


    public void Start() {

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        Hide();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e) {

        if (GameManager.Instance.IsGameOver()) {

            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetAmountOfSucceededDeliveries().ToString();
        }
        else {

            Hide();
        }
    }

    private void Show() {

        gameObject.SetActive(true);
    }

    private void Hide() {

        gameObject.SetActive(false);
    }
}
