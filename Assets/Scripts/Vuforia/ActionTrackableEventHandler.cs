﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ActionTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
    public GameObject controller;
    public ActionCard actionCard;
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
                if (mTrackableBehaviour.TrackableName == "Rally") {
                    actionCard = new Rally();
                    controller.GetComponent<GameMenuController>().getActionCard(actionCard);
                }
                else if (mTrackableBehaviour.TrackableName == "PushTrap") {
                    actionCard = new PushTrap();
                    controller.GetComponent<GameMenuController>().getActionCard(actionCard);
                }
                else if (mTrackableBehaviour.TrackableName == "FireTrap") {
                    actionCard = new FireTrap();
                    controller.GetComponent<GameMenuController>().getActionCard(actionCard);
                }
                else if (mTrackableBehaviour.TrackableName == "Tactic") {
                    actionCard = new Tactic();
                    controller.GetComponent<GameMenuController>().getActionCard(actionCard);
                }
                else if (mTrackableBehaviour.TrackableName == "Steal") {
                    actionCard = new Steal();
                    controller.GetComponent<GameMenuController>().getActionCard(actionCard);
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
