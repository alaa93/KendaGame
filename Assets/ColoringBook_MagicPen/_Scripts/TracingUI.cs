using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.Video;

namespace KendaGame.UI {
  public class TracingUI : MonoBehaviour {
    [SerializeField]
    ColoringPen[] penBtns;
    [SerializeField]
    GameObject Game1;
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


    private void OnEnable() {
      if (PlayerPrefs.GetInt("PlayIntro1", -1) == -1) {
        PlayerPrefs.SetInt("PlayIntro1", 1);
        for (int x = 0; x < ColoringStuff.Length; x++) {
          ColoringStuff[x].SetActive(false);
        }
        VideoRenderer.SetActive(true);
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        }
        else {
        SceneManager.Instance.OpenCloudsManually();
        VideoRenderer.SetActive(false);
        videoPlayer.Stop();
        if(PlayerPrefs.GetInt("Game1Completed", 0) == 0)
        {
            SetGame1(true);
        }
        else
        {
            // start game 2 or 3
            StartColoring();
        }
      }
    }

        private void VideoPlayer_prepareCompleted(VideoPlayer source)
        {
            SceneManager.Instance.SetBackBtnState(false);
            SceneManager.Instance.OpenCloudsManually(animated : true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += VideoCompletedPlaying;
        }

        private void VideoCompletedPlaying(VideoPlayer source)
        {
            SceneManager.Instance.CloseCloudsManually(animated: true,onComplete:(state)=> {
                VideoRenderer.SetActive(false);
                videoPlayer.Stop();
                SetGame1(true);
                SceneManager.Instance.SetBackBtnState(true);
                SceneManager.Instance.OpenCloudsManually(animated: true);
            });
        }

        private IEnumerator VideoPlaying() {
      yield return new WaitForSeconds(36.5f);
     
    }

    private void SetGame1(bool state)
    {
        Game1.SetActive(state);
    }

    public void OnGameCompleted(int gameId)
    {
        Debug.Log("game "+gameId + "completed");
        if(gameId == 1)
        {
            PlayerPrefs.SetInt("Game1Completed", 1);
            SceneManager.Instance.CloseCloudsManually(onComplete:(state)=> {
                SetGame1(false);
                StartColoring();
                SceneManager.Instance.OpenCloudsManually();
            });
        }
    }

    public void TracingCompleted()
    {
            // show rating screen, close clouds then do something
        SceneManager.Instance.CloseCloudsManually(onComplete: (state) => {
           // Do something
        });
    }

    private void StartColoring()
    {
        musicLoop.Play();
        for (int x = 0; x < ColoringStuff.Length; x++)
        {
            ColoringStuff[x].SetActive(true);
        }
         penBtns = FindObjectsOfType<ColoringPen>();
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
            if(penBtns[temp] != pen)
            {
                penBtns[temp].SelectionChanged(true);
            }
        }
      pen.SelectionChanged();
    }
  }
}