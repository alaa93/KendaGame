using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using UnityEngine;
using UnityEngine.Video;

namespace KendaGame.UI {
  public class TracingUI : MonoBehaviour {
    [SerializeField]
    ColoringPen[] penBtns;
    [SerializeField]
    GameObject VideoRenderer;
    [SerializeField]
    VideoPlayer videoPlayer;
    [SerializeField]
    GameObject [] ColoringStuff;
    [SerializeField]
    AudioSource musicLoop;

    private static TracingUI _instance;
    public static TracingUI Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<TracingUI>();
        }
        return _instance;
      }
    }


    private void Awake() {
      penBtns = FindObjectsOfType<ColoringPen>();
    }

    private void OnEnable() {
      if (PlayerPrefs.GetInt("PlayInt3ro", -1) == -1) {
        PlayerPrefs.SetInt("PlayIntro", 1);
        VideoRenderer.SetActive(true);
        videoPlayer.Play();
        StartCoroutine(VideoPlaying());
        for (int x = 0; x < ColoringStuff.Length; x++) {
          ColoringStuff[x].SetActive(false);
        }

      } else {
        VideoRenderer.SetActive(false);
        videoPlayer.Stop();
        for (int x = 0; x < ColoringStuff.Length; x++) {
          ColoringStuff[x].SetActive(true);
        }
      }
    }

    private IEnumerator VideoPlaying() {
      yield return new WaitForSeconds(36.5f);
      VideoRenderer.SetActive(false);
      videoPlayer.Stop();
      musicLoop.Play();
      for (int x = 0; x < ColoringStuff.Length; x++) {
        ColoringStuff[x].SetActive(true);
      }
    }

    private void OnVideoCompletedPlaying(VideoPlayer source) {
      VideoRenderer.SetActive(false);
      videoPlayer.Stop();
      for (int x = 0; x < ColoringStuff.Length; x++) {
        ColoringStuff[x].SetActive(true);
      }
    }

    public void OnPenClicked(ColoringPen pen) {
      for (int x = 0; x < penBtns.Length; x++) {
        int temp = x;
        if (penBtns[temp] != pen) {
          penBtns[temp].SelectionChanged(true);
        }
      }
      pen.SelectionChanged();
    }
  }
}