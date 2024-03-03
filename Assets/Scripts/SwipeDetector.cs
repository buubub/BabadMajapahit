using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwipeDetector : MonoBehaviour {
    public GameObject controller;
    public Canvas canvas;
    private float startPos, endPos, ratio, width;
    public List<RectTransform> menu = new List<RectTransform>();
    private int currentMenu = 0;

    //void Start() {
    //    width = menu[0].GetComponent<RectTransform>().rect.width;
    //    ratio = width / canvas.GetComponent<CanvasScaler>().referenceResolution.x;
    //    for (int i = 0; i < menu.Count; i++) {
    //        float width = menu[i].GetComponent<RectTransform>().rect.width;
    //        menu[i].GetComponent<RectTransform>().DOAnchorPosX((i * 100000) - (width * currentMenu), 0f);
    //    }
    //}

    //void Update() {
    //    if (!controller.GetComponent<MainMenuController>().playerCountPanel.activeSelf) {
    //        if (Input.touchCount > 0) {
    //            Touch touch = Input.GetTouch(0);
    //            switch (touch.phase) {
    //                case TouchPhase.Began:
    //                    startPos = touch.position.x;
    //                    break;

    //                case TouchPhase.Moved:
    //                    for (int i = 0; i < menu.Count; i++) {
    //                        menu[i].GetComponent<RectTransform>().DOAnchorPosX((i * width) - (width * currentMenu) + (touch.position.x - startPos) / ratio, 0f);
    //                    }
    //                    break;

    //                case TouchPhase.Ended:
    //                    endPos = (touch.position.x - startPos) / width;
    //                    if (endPos < -0.3f) {
    //                        if (currentMenu < 1) {
    //                            NextMenu();
    //                        }
    //                        else {
    //                            CancelMove();
    //                        }
    //                    }
    //                    else if (endPos > 0.3f) {
    //                        if (currentMenu > 0) {
    //                            PrevMenu();
    //                        }
    //                        else {
    //                            CancelMove();
    //                        }
    //                    }
    //                    else {
    //                        CancelMove();
    //                    }
    //                    break;
    //            }
    //        }

    //        if (Input.GetKeyUp(KeyCode.RightArrow)) {
    //            NextMenu();
    //        }
    //        else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
    //            PrevMenu();
    //        }
    //    }
    //}

    //private void NextMenu() {
    //    currentMenu++;
    //    for (int i = 0; i < menu.Count; i++) {
    //        menu[i].GetComponent<RectTransform>().DOAnchorPosX((i * width) - width * currentMenu, 0.25f);
    //    }
    //}

    //private void PrevMenu() {
    //    currentMenu--;
    //    for (int i = 0; i < menu.Count; i++) {
    //        menu[i].GetComponent<RectTransform>().DOAnchorPosX((i * width) - width * currentMenu, 0.25f);
    //    }
    //}

    //public void CancelMove() {
    //    for (int i = 0; i < menu.Count; i++) {
    //        menu[i].GetComponent<RectTransform>().DOAnchorPosX((i * width) - width * currentMenu, 0.25f);
    //    }
    //}
}
