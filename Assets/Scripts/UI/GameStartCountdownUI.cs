using TMPro;
using UnityEngine;


public class GameStartCountdownUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI countdownText;


    public void Start() {

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        Hide();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e) {
        
        if (GameManager.Instance.IsCountingDownToStart()) {

            Show();
        }
        else {

            Hide();
        }
    }

    private void Update() {

        countdownText.text = Mathf.Ceil(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }

    private void Show() {

        gameObject.SetActive(true);
    }
    
    private void Hide() {

        gameObject.SetActive(false);
    }
}
