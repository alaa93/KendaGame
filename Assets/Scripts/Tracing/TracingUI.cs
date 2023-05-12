using System.Collections;
using System.Collections.Generic;
using System.IO;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.Networking;
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
    public GameObject tracingGuide;

    private static TracingUI _instance;
    public static TracingUI Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<TracingUI>();
        }
        return _instance;
      }
    }


        private void OnEnable()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(SceneManager.Scenes.TracingLetter.ToString()))
            {
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

            if (PlayerPrefs.GetInt("PlayIntro12", -1) == -1)
            {
                PlayerPrefs.SetInt("PlayIntro1", 1);
                for (int x = 0; x < ColoringStuff.Length; x++)
                {
                    ColoringStuff[x].SetActive(false);
                }
                VideoRenderer.SetActive(true);
                videoPlayer.Prepare();
                videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
            }
            else
            {
                SceneManager.Instance.OpenCloudsManually();
                VideoRenderer.SetActive(false);
                videoPlayer.Stop();
                if (PlayerPrefs.GetInt("Game1Completed1", 0) == 0)
                {
                    SetGame1(true);
                }
                else
                {
                    if (PlayerPrefs.GetInt("ColoringCompleted1", 0) == 0)
                    {
                        StartColoring();
                    }
                    else
                    {
                        SceneManager.Instance.MoveTo(SceneManager.Scenes.SharkGame.ToString());
                    }
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

    public void TracingCompleted() {
      // show rating screen, close clouds then do something
      PlayerPrefs.SetInt("ColoringCompleted", 1);
      PopUpsManager.Instance.LoadRatingLetter();
      //SceneManager.Instance.MoveTo(SceneManager.Scenes.SharkGame.ToString());
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


    public void OnTracingLetterCompleted() {
      //GameObject.FindAnyObjectByType<ColoringBookManager>().OnScreenshotButtonClicked((path)=> {
      //    Debug.Log("file name " + ScreenshotManager.screenshotFilename);
      //    StartCoroutine(Upload(path));
      //});
      SceneManager.tracingGuide = 1;
      GameObject.FindObjectOfType<PopUpsManager>().LoadRatingLetter();
    }


        public IEnumerator Upload(string filePath)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("Imageurl", File.ReadAllBytes(filePath)); // , "imageName.jpg"
            form.AddField("RequiredClass", "alef");

            UnityWebRequest www = UnityWebRequest.Post("http://137.184.184.223:8000/ClientPredict/", form);
            //www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "8b38c163cdd104d13ca887f5c17fd4c3324c4a13");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("photo upload complete! " + www.downloadHandler.text);

            }
        }
    }
}