using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private void Awake() {

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {

        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStageChangedEventArgs e) {

        bool playSound = e.state == StoveCounter.State.Cooking || e.state == StoveCounter.State.Cooked;

        if (playSound) {

            audioSource.Play();
        }
        else {

            audioSource.Pause();
        }
    }
}
