using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TokenTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
    public GameObject controller;
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    protected virtual void OnEnable() {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDisable() {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy() {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus) {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (controller.GetComponent<GameMenuController>().gameState == GameState.GameScanCard) {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
                Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
                if(mTrackableBehaviour.TrackableName == "MajapahitToken") {
                    controller.GetComponent<GameMenuController>().getToken();
                }
                else if(mTrackableBehaviour.TrackableName == "FireTrapToken") {
                    controller.GetComponent<GameMenuController>().triggerTrap("FireTrap");
                }
                else if(mTrackableBehaviour.TrackableName == "PushTrapToken") {
                    controller.GetComponent<GameMenuController>().triggerTrap("PushTrap");
                }
            }
            else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                     newStatus == TrackableBehaviour.Status.NO_POSE) {
                Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            }
            else {
                // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
                // Vuforia is starting, but tracking has not been lost or found yet
                // Call OnTrackingLost() to hide the augmentations
            }
        }
    }
}
