using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaInit : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        var vuforia = VuforiaARController.Instance;    
  		vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);    
		vuforia.RegisterOnPauseCallback(OnPaused);
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnVuforiaStarted() {
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	private void OnPaused(bool paused) {
	    if (!paused) {
	        // Set again autofocus mode when app is resumed
	    	CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	    }
	}
}
