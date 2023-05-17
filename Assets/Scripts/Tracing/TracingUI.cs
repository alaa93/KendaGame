using System.Collections;
using System.Collections.Generic;
using System.IO;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
    GameObject[] ColoringStuff;
    [SerializeField]
    AudioSource musicLoop;
    public GameObject tracingGuide;
    public RawImage screenshotRef;
    public ColoringBookManager coloringBookManager;
    public Image fillLoadingBar;
    public GameObject loadingBar;

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
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(SceneManager.Scenes.TracingLetter.ToString())) {
        SceneManager.Instance.OpenCloudsManually();
        PlayerPrefs.SetInt("ColoringList", 0);
        PlayerPrefs.Save();

        ColoringBookManager.maskTexIndex = -1;
        ColoringBookManager.ID = "ColoringList0";

        if (SceneManager.tracingGuide > 0) {
          tracingGuide.SetActive(false);
        }
        return;
      }

      if (PlayerPrefs.GetInt("PlayIntro12", -1) == -1) {
        PlayerPrefs.SetInt("PlayIntro1", 1);
        for (int x = 0; x < ColoringStuff.Length; x++) {
          ColoringStuff[x].SetActive(false);
        }
        VideoRenderer.SetActive(true);
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
      } else {
        SceneManager.Instance.OpenCloudsManually();
        VideoRenderer.SetActive(false);
        videoPlayer.Stop();
        if (PlayerPrefs.GetInt("Game1Completed1", 0) == 0) {
          SetGame1(true);
        } else {
          if (PlayerPrefs.GetInt("ColoringCompleted1", 0) == 0) {
            StartColoring();
          } else {
            SceneManager.Instance.MoveTo(SceneManager.Scenes.SharkGame.ToString());
          }
        }
      }

    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source) {
      SceneManager.Instance.SetBackBtnState(false);
      SceneManager.Instance.OpenCloudsManually(animated: true);
      videoPlayer.Play();
      videoPlayer.loopPointReached += VideoCompletedPlaying;
    }

    private void VideoCompletedPlaying(VideoPlayer source) {
      SceneManager.Instance.CloseCloudsManually(animated: true, onComplete: (state) => {
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

    private void SetGame1(bool state) {
      Game1.SetActive(state);
    }

    public void OnGameCompleted(int gameId) {
      Debug.Log("game " + gameId + "completed");
      if (gameId == 1) {
        PlayerPrefs.SetInt("Game1Completed", 1);
        SceneManager.Instance.CloseCloudsManually(onComplete: (state) => {
          SetGame1(false);
          StartColoring();
          SceneManager.Instance.OpenCloudsManually();
        });
      }
    }

    public void TracingCompleted() {
      // show rating screen, close clouds then do something
      PlayerPrefs.SetInt("ColoringCompleted", 1);
      PopUpsManager.Instance.LoadRatingLetter();
      //SceneManager.Instance.MoveTo(SceneManager.Scenes.SharkGame.ToString());
    }

    private void StartColoring() {
      musicLoop.Play();
      for (int x = 0; x < ColoringStuff.Length; x++) {
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
        if (penBtns[temp] != pen) {
          penBtns[temp].SelectionChanged(true);
        }
      }
      pen.SelectionChanged();
    }


    public void OnTracingLetterCompleted() {
      StartCoroutine(Upload());
      //GameObject.FindObjectOfType<ColoringBookManager>().OnScreenshotButtonClicked((path) => {
      //  Debug.Log("file name " + path);
      //});
      SceneManager.tracingGuide = 1;
      //GameObject.FindObjectOfType<PopUpsManager>().LoadRatingLetter();
    }

    public IEnumerator Upload() {

      if (tracingGuide != null) {
        tracingGuide.SetActive(false);
      }
      MusicController.USE.PlaySound(MusicController.USE.cameraSound);
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Rect rect = coloringBookManager.GUIRectWithObject(coloringBookManager.gameObject);
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();
      yield return 0;
      byte[] bytes = texture.EncodeToPNG();
      Destroy(texture);
      if (tracingGuide != null && SceneManager.tracingGuide == 0) {
        tracingGuide.SetActive(true);
      }
      //string base64Image = $"data:image/png;base64,{System.Convert.ToBase64String(bytes)}"
      string _headerValue = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";
      WWWForm formrequest = new WWWForm();
      formrequest.AddBinaryData("Imageurl", bytes, "test.jpeg", "image/jpeg");
      formrequest.AddField("RequiredClass", "alef");
      UnityWebRequest www = UnityWebRequest.Post("http://137.184.184.223:8000/ClientPredict/", formrequest);
      www.SetRequestHeader("Authorization", _headerValue);
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      if (www.downloadHandler != null && www.downloadHandler.error != null)
        Debug.Log(www.downloadHandler.error);
      if (www.downloadHandler != null && www.downloadHandler.text != null) {
        Debug.Log(www.downloadHandler.text);
        LetterRatingPopUp letterRatingPopUp = PopUpsManager.Instance.LoadRatingLetterInstance();
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          letterRatingPopUp.SetAnimation(1);
        } else {
          RatingData ratingData = JsonUtility.FromJson<RatingData>(www.downloadHandler.text);
          if (ratingData != null && ratingData.PredictedClass == "alef") {
            Debug.Log("Rating Data letter " + ratingData.PredictedClass);
            Debug.Log("Rating Data letter SCORE " + ratingData.PredictedScore);
            if (ratingData.PredictedScore < 0.50f) {
              letterRatingPopUp.SetAnimation(1);
            } else {
              letterRatingPopUp.SetAnimation(ratingData.PredictedScore >= 0.85f ? 3 : 2);
            }
          } else {
            letterRatingPopUp.SetAnimation(1);
          }
        }
      }
      bytes = null;
      rect = Rect.zero;
    }
    IEnumerator UploadProgressBar(UnityWebRequest request) {
      loadingBar.SetActive(true);
      while (!request.isDone) {
        fillLoadingBar.fillAmount += 0.1f;
        yield return new WaitForSeconds(1f);
      }
      loadingBar.SetActive(false);
    }
  }

  
  //{"PredictedScore":"0.81","PredictedClass":"alef"}
  [System.Serializable]
  public class RatingData {
    public float PredictedScore;
    public string PredictedClass;
  }
}