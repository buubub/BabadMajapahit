using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public GameObject mainPanel, playPanel, nextButton, prevButton, playButton, extraButton, exitButton, plusButton, minusButton, startButton;
    public Text playerCountText;
    public int playerCount = 2;

    void Start() {
        playerCountText.text = playerCount.ToString();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (playPanel.activeSelf) {
                playPanel.SetActive(false);
                mainPanel.SetActive(true);
            }
            else {
                SceneManager.LoadScene("StartMenuScene");
            }
        }
    }

    public void OnClickPlay() {
        mainPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void OnClickExtra() {
        SceneManager.LoadScene("ExtraMenuScene");
    }

    public void OnClickExit() {
        if (playPanel.activeSelf) {
            playPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        else {
            SceneManager.LoadScene("StartMenuScene");
        }
    }

    public void OnClickNext() {
        playButton.SetActive(false);
        nextButton.SetActive(false);
        extraButton.SetActive(true);
        prevButton.SetActive(true);
    }

    public void OnClickPrev() {
        extraButton.SetActive(false);
        prevButton.SetActive(false);
        playButton.SetActive(true);
        nextButton.SetActive(true);
    }

    public void OnClickPlus() {
        if (playerCount < 6) {
            playerCount++;
            if (playerCount == 5) {
                playerCount++;
            }
            playerCountText.text = playerCount.ToString();
        }
    }

    public void OnClickMinus() {
        if (playerCount > 2) {
            playerCount--;
            if (playerCount == 5) {
                playerCount--;
            }
            playerCountText.text = playerCount.ToString();
        }
    }

    public void OnClickStart() {
        Data.playerCount = playerCount;
        SceneManager.LoadScene("GameMenuScene");
    }
}

public static class Data {
    public static int playerCount;
}