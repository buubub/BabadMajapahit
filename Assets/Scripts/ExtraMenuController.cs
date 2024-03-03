using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ExtraMenuController : MonoBehaviour {
    public GameObject controller, panel, videoPanel, mainPanel; 
	public RawImage charaImage;
	public Text charaTextName, charaTextDescription;
	public GameObject[] chara;
    public Camera camera, ARcamera;
    
    void Start() {
    }
    
    void Update() {
        
    }

    public void ShowCharacter(GameObject chara) {
        this.charaTextName.text = chara.GetComponent<CharacterScript>().charaName;
        this.charaTextDescription.text = chara.GetComponent<CharacterScript>().charaDescription;
        this.charaImage.texture = chara.GetComponent<CharacterScript>().charaImage;
        panel.SetActive(true);
    }

    public void HideCharacter() {
        panel.SetActive(false);
    }

    public void changeCamera(Camera cam) {
        if (cam == camera) {
            camera.gameObject.SetActive(true);
            ARcamera.gameObject.SetActive(false);
        }
        else {
            camera.gameObject.SetActive(false);
            ARcamera.gameObject.SetActive(true);
            HideCharacter();
        }
    }

    public void ShowVideo(bool show) {
        if(show) {
            mainPanel.SetActive(false);
            videoPanel.SetActive(true);
        }
        else {
            mainPanel.SetActive(true);
            videoPanel.SetActive(false);
        }
    }

    public void PlayVideo(VideoClip Video) {
        GetComponent<VideoMenuController>().Play(Video);
    }

    public void Back() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
