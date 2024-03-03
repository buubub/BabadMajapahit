using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoMenuController : MonoBehaviour {
    private VideoPlayer videoPlayer;
    public GameObject videoPanel;
    public Texture video;

    void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp) {
        Skip();
    }

    public void Play(VideoClip video) {
        videoPanel.SetActive(true);
        videoPlayer.clip = video;
        videoPlayer.Play();
    }

    public void Skip() {
        videoPlayer.Stop();
        videoPlayer.targetTexture.Release();
        videoPanel.SetActive(false);
    }
}
