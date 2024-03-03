using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartMenuController : MonoBehaviour {

    public RawImage start;

    void Start() {
        blink(true);
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            SceneManager.LoadScene("MainMenuScene");
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void blink(bool fade) {
        if(fade) {
            start.DOFade(0, 0.75f).SetDelay(0.5f).OnComplete(()=> blink(false));
        }
        else {
            start.DOFade(1, 0.75f).OnComplete(()=> blink(true));
        }
    }
}
