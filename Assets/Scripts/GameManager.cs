using System;
using UnityEngine;


public class GameManager : MonoBehaviour {


    public static GameManager Instance;

    public event EventHandler OnGameStateChanged;

    private enum State {

        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State gameState;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 10f;


    private void Awake() {

        Instance = this;

        gameState = State.WaitingToStart;
    }

    private void Update() {
        
        switch (gameState) {

            case State.WaitingToStart:

                waitingToStartTimer -= Time.deltaTime;

                if (waitingToStartTimer < 0f) {

                    gameState = State.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:

                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 0f) {

                    gameState = State.GamePlaying;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:

                gamePlayingTimer -= Time.deltaTime;

                if (gamePlayingTimer < 0f) {

                    gameState = State.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying() {

        return gameState == State.GamePlaying;
    }

    public bool IsCountingDownToStart() {

        return gameState == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer() {

        return countdownToStartTimer;
    }
}
