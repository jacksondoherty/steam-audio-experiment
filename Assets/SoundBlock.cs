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

    public override void StartTouching(GameObject currentTouchingObject) {
        base.StartTouching(currentTouchingObject);

        if (touchingObjects.Count == 1) {
            sound.Play();
        }
    }

    public override void StopTouching(GameObject previousTouchingObject) {
        base.StopTouching(previousTouchingObject);

        if (touchingObjects.Count == 0) {   // can put pedal boolean here
            sound.Stop();
        }
    }
}
