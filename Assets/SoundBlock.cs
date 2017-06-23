using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SoundBlock : VRTK_InteractableObject {

    private AudioSource sound;
    // todo: perhaps save script instead
    private GameObject lastTouching;
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

        // pedal
        if (lastTouching) {
            VRTK_ControllerEvents script = lastTouching.GetComponent<VRTK_ControllerEvents>();
            if (!script.gripPressed) {
                sound.Stop();
                lastTouching = null;
            }
        }

        // volume
        float scaleDelta = transform.localScale.x / initialScaleX;
        sound.volume = initialVolume * scaleDelta*scaleDelta;
    }

    // block hit
    public override void StartTouching(GameObject currentTouchingObject) {
        base.StartTouching(currentTouchingObject);

        if (touchingObjects.Count == 1) {
            sound.Play();
        }
    }

    // block release
    public override void StopTouching(GameObject previousTouchingObject) {
        base.StopTouching(previousTouchingObject);
        
        if (touchingObjects.Count == 0) {
            lastTouching = previousTouchingObject;
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
