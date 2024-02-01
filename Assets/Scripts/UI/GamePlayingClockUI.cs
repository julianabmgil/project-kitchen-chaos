using UnityEngine;
using UnityEngine.UI;


public class GamePlayingClockUI : MonoBehaviour {

    [SerializeField] private Image timerImage;


    private void Start() {

        timerImage.fillAmount = 0f;
    }

    private void Update() {

        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
