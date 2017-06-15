using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SoundBlock : VRTK_InteractableObject {

    private AudioSource sound;

    protected override void Awake() {
        base.Awake();

        sound = GetComponent<AudioSource>();
    }

    public override void StartTouching(GameObject currentTouchingObject) {
        base.StartTouching(currentTouchingObject);

        if (currentTouchingObject.CompareTag("ControllerScript")) {
            sound.Play();
        }
    }

    public override void StopTouching(GameObject previousTouchingObject) {
        base.StopTouching(previousTouchingObject);

        if (previousTouchingObject.CompareTag("ControllerScript")) {
            sound.Stop();
        }
    }
}
