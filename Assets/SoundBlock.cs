using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SoundBlock : VRTK_InteractableObject {

    private AudioSource sound;
    private GameObject pedalHolder;
    private float initialScaleX; // x, y, z all equal
    private float initialVolume;

    protected override void Awake() {
        base.Awake();

        sound = GetComponent<AudioSource>();
        initialScaleX = transform.localScale.x;
        initialVolume = sound.volume;
    }

    protected override void Update() {
        base.Update();

        // volume
        float scaleDelta = transform.localScale.x / initialScaleX;
        sound.volume = initialVolume * scaleDelta * scaleDelta;

        // pedal
        if (pedalHolder) {
            VRTK_ControllerEvents eventsScript = pedalHolder.GetComponent<VRTK_ControllerEvents>();
            if (!eventsScript.gripPressed) {
                sound.Stop();
                pedalHolder = null;
            } else {
                // pedal rumble
                rumble(pedalHolder, 0.05f*scaleDelta);
            }
        }

        // touching rumble
        foreach (GameObject controller in touchingObjects) {
            rumble(controller, 0.05f*scaleDelta);
        }
    }

    private void rumble(GameObject controller, float strength) {
        VRTK_ControllerActions actionsScript = controller.GetComponent<VRTK_ControllerActions>();
        actionsScript.TriggerHapticPulse(strength);
    }

    // block hit
    public override void StartTouching(GameObject currentTouchingObject) {
        base.StartTouching(currentTouchingObject);

        pedalHolder = null;
        if (touchingObjects.Count == 1) {
            sound.Play();
        }
    }

    // block release
    public override void StopTouching(GameObject previousTouchingObject) {
        base.StopTouching(previousTouchingObject);
        
        if (touchingObjects.Count == 0) {
            pedalHolder = previousTouchingObject;
            // handle audio stop in update
        }
    }

    // overriding to not toggle if currently grabbed
    public override void ToggleHighlight(bool toggle) {
        InitialiseHighlighter();

        if (touchHighlightColor != Color.clear && objectHighlighter && !IsGrabbed()) {
            if (toggle) {
                objectHighlighter.Highlight(touchHighlightColor);
            } else {
                objectHighlighter.Unhighlight();
            }
        }
    }
}
