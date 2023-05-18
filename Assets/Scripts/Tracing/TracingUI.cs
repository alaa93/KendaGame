using System;
using System.Collections;
using KendaGame.Helpers;
using KendaGame.Navigation;
using ScorpionSteps.Utils;
using UnityEngine;
using UnityEngine.UI;
using static KendaGame.Models.TracingModels;

namespace KendaGame.UI {
  public class TracingUI : MonoBehaviour {

    [SerializeField]
    AudioSource musicLoop;
    [SerializeField]
    Material coloringMaterial;
    [SerializeField]
    Sprite selectedFontSize, noneSelectedFontSize;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtns;
    [SerializeField]
    Image tracingGuide;
    [SerializeField]
    GameObject coloringBoard;


    private ColoringPen[] penBtns;
    public RawImage screenshotRef;

    private static TracingUI _instance;
    public static TracingUI Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<TracingUI>();
        }
        return _instance;
      }
    }

    private void Start() {
      SceneManager.Instance.OpenCloudsManually();
      InitFontSizeBtns();
      PrepareBoard();
      StartTracing();
    }

    private void InitFontSizeBtns() {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        int temp = x;
        fontSizeBtns[temp].AddClickListener(animate: true, callBackFn: () => {
          OnFontBtnClicked(fontSizeBtns[temp], temp + 1);
        });
      }
    }

    private void OnFontBtnClicked(UIButtonAnimator fontBtn, int fontSize) {
      OnFontSizeSelected(fontBtn, fontSize);
    }

    private void PrepareBoard() {
      PlayerPrefs.SetInt("ColoringList", 0);
      PlayerPrefs.Save();

      ColoringBookManager.maskTexIndex = -1;
      ColoringBookManager.ID = "ColoringList0";
      coloringBoard.SetActive(true);
      //coloringMaterial.SetVector("_MaskTex",new Vector4(0.68f,1f, 0.16f,1.02f));
      //if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingAdvanced) {
      //  //TODO load related guide letter
      //}
    }

    private void StartTracing() {
      musicLoop.Play();
      penBtns = FindObjectsOfType<ColoringPen>();
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

    public void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        fontSizeBtns[x].GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.6f);
      }
      //fontBtn.GetComponent<Image>().sprite = selectedFontSize;
      fontBtn.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
      ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(fontSize);
    }

    public void OnTracingLetterCompleted() {
      SceneManager.tracingGuide = 1;
      StartCoroutine(CropLetterArea((bytes) => {
        RequestLetterRating(bytes);
      }));
    }

    private void RequestLetterRating(byte[] bytes) {
      if (bytes != null) {
        LoadingBar loadingBar = PopUpsManager.Instance.LoadLoadingBar();
        loadingBar.ShowLoadingBar(title: "Uploading In Progress"); // I2.Loc.LocalizationManager.GetTermTranslation("")
        HTTPRequestHandler.Instance.UploadTracingLetter(bytes, LettersHelper.GetActiveLetterClass(), (success, ratingData) => {
          OnRatingCompleted(success, ratingData);
        });
      }
    }

    private void OnRatingCompleted(bool success, RatingData ratingData) {
      LetterRatingPopUp letterRatingPopUp = PopUpsManager.Instance.LoadRatingLetterInstance();
      if (success) {
        if (ratingData != null) {
          Debug.Log("ratingData.PredictedClass " + ratingData.PredictedClass);
          Debug.Log("ratingData.PredictedClass " + ratingData.PredictedScore);
          if (letterRatingPopUp != null) {
            SetRating(ratingData.PredictedScore, letterRatingPopUp);
          }
        } else {
          if (letterRatingPopUp != null) {
            SetRating(0f, letterRatingPopUp);
          }
        }
      } else {
        if (letterRatingPopUp != null) {
          SetRating(0f, letterRatingPopUp);
        }
      }
    }

    private void SetRating(float rating, LetterRatingPopUp ratingPopUp) {
      if (rating < 0.50f) {
        ratingPopUp.SetAnimation(1);
      } else {
        ratingPopUp.SetAnimation(rating >= 0.80f ? 3 : 2);
      }
    }

    private IEnumerator CropLetterArea(Action<byte[]> callBack) {
      yield return new WaitForEndOfFrame();
      Rect rect = CameraUtils.GUIRectWithObject(ColoringBookManager.Instance.gameObject);
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();
      screenshotRef.texture = texture;
      byte[] bytes = texture.EncodeToPNG();
      callBack?.Invoke(bytes);
      //Destroy(texture);
    }
  }
}

//using System.Collections;
//using KendaGame.Navigation;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;

//namespace KendaGame.UI {
//  public class TracingUI : MonoBehaviour {
//    [SerializeField]
//    ColoringPen[] penBtns;
//    [SerializeField]
//    AudioSource musicLoop;

//    public GameObject tracingGuide;
//    public RawImage screenshotRef;
//    public ColoringBookManager coloringBookManager;
//    public Image fillLoadingBar;
//    public GameObject loadingBar;

//    private static TracingUI _instance;
//    public static TracingUI Instance {
//      get {
//        if (_instance == null) {
//          _instance = FindObjectOfType<TracingUI>();
//        }
//        return _instance;
//      }
//    }


//    private void OnEnable() {
//      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(SceneManager.Scenes.TracingLetter.ToString())) {
//        SceneManager.Instance.OpenCloudsManually();
//        PlayerPrefs.SetInt("ColoringList", 0);
//        PlayerPrefs.Save();

//        ColoringBookManager.maskTexIndex = -1;
//        ColoringBookManager.ID = "ColoringList0";

//        if (SceneManager.tracingGuide > 0) {
//          tracingGuide.SetActive(false);
//        }
//        return;
//      }
//      StartColoring();
//    }

//    public void TracingCompleted() {
//      // show rating screen, close clouds then do something
//      PlayerPrefs.SetInt("ColoringCompleted", 1);
//      PopUpsManager.Instance.LoadRatingLetter();
//    }

//    private void StartColoring() {
//      musicLoop.Play();
//      penBtns = FindObjectsOfType<ColoringPen>();
//    }

//    public void OnPenClicked(ColoringPen pen) {
//      for (int x = 0; x < penBtns.Length; x++) {
//        int temp = x;
//        if (penBtns[temp] != pen) {
//          penBtns[temp].SelectionChanged(true);
//        }
//      }
//      pen.SelectionChanged();
//    }


//    public void OnTracingLetterCompleted() {
//      StartCoroutine(Upload());
//      SceneManager.tracingGuide = 1;
//    }

//    public IEnumerator Upload() {

//      if (tracingGuide != null) {
//        tracingGuide.SetActive(false);
//      }
//      MusicController.USE.PlaySound(MusicController.USE.cameraSound);
//      yield return new WaitForEndOfFrame();
//      Debug.Log("uploading screenshot to server...");
//      Rect rect = coloringBookManager.GUIRectWithObject(coloringBookManager.gameObject);
//      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
//      texture.ReadPixels(rect, 0, 0);
//      texture.Apply();
//      yield return 0;
//      byte[] bytes = texture.EncodeToPNG();
//      Destroy(texture);
//      if (tracingGuide != null && SceneManager.tracingGuide == 0) {
//        tracingGuide.SetActive(true);
//      }
//      //string base64Image = $"data:image/png;base64,{System.Convert.ToBase64String(bytes)}"
//      string _headerValue = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";
//      WWWForm formrequest = new WWWForm();
//      formrequest.AddBinaryData("Imageurl", bytes, "test.jpeg", "image/jpeg");
//      formrequest.AddField("RequiredClass", "alef");
//      UnityWebRequest www = UnityWebRequest.Post("http://137.184.184.223:8000/ClientPredict/", formrequest);
//      www.SetRequestHeader("Authorization", _headerValue);
//      StartCoroutine(UploadProgressBar(www));
//      yield return www.SendWebRequest();
//      if (www.downloadHandler != null && www.downloadHandler.error != null)
//        Debug.Log(www.downloadHandler.error);
//      if (www.downloadHandler != null && www.downloadHandler.text != null) {
//        Debug.Log(www.downloadHandler.text);
//        LetterRatingPopUp letterRatingPopUp = PopUpsManager.Instance.LoadRatingLetterInstance();
//        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
//          letterRatingPopUp.SetAnimation(1);
//        } else {
//          RatingData ratingData = JsonUtility.FromJson<RatingData>(www.downloadHandler.text);
//          if (ratingData != null && ratingData.PredictedClass == "alef") {
//            Debug.Log("Rating Data letter " + ratingData.PredictedClass);
//            Debug.Log("Rating Data letter SCORE " + ratingData.PredictedScore);
//            if (ratingData.PredictedScore < 0.50f) {
//              letterRatingPopUp.SetAnimation(1);
//            } else {
//              letterRatingPopUp.SetAnimation(ratingData.PredictedScore >= 0.85f ? 3 : 2);
//            }
//          } else {
//            letterRatingPopUp.SetAnimation(1);
//          }
//        }
//      }
//    }

//    IEnumerator UploadProgressBar(UnityWebRequest request) {
//      loadingBar.SetActive(true);
//      while (!request.isDone) {
//        fillLoadingBar.fillAmount += 0.1f;
//        yield return new WaitForSeconds(1f);
//      }
//      loadingBar.SetActive(false);
//    }
//  }


//  //{"PredictedScore":"0.81","PredictedClass":"alef"}
//  [System.Serializable]
//  public class RatingData {
//    public float PredictedScore;
//    public string PredictedClass;
//  }
//}