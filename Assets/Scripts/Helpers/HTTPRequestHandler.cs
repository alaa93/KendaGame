using System;
using System.Collections;
using System.IO;
using KendaGame.Navigation;
using KendaGame.UI;
using UnityEngine;
using UnityEngine.Networking;
using static KendaGame.Models.TracingModels;

namespace KendaGame.Helpers {
  public class HTTPRequestHandler : MonoBehaviour {

    private static readonly string WEB_URL = "http://137.184.184.223:8000";
    private static readonly string TRACING_MODULE = "ClientPredict";
    private static readonly string REQUEST_HEADER = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";

    private static HTTPRequestHandler _instance;
    public static HTTPRequestHandler Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<HTTPRequestHandler>();
        }
        return _instance;
      }
    }

    public void UploadTracingLetter(byte[] imageBytes = null, string targetLetter = "alef", Action<bool, RatingData> statusCallback = null) {
      StartCoroutine(UploadTracingLetterProcess(imageBytes, targetLetter, statusCallback));
    }

    public void UploadLetter() {
      StartCoroutine(Upload());
    }

    public IEnumerator UploadTracingLetterProcess(byte[] imageBytes, string targetLetter, Action<bool, RatingData> statusCallback) {
      MusicController.USE.PlaySound(MusicController.USE.cameraSound);
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Rect rect = ColoringBookManager.Instance.GUIRectWithObject(ColoringBookManager.Instance.gameObject);
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();
      yield return 0;
      byte[] bytes = texture.EncodeToPNG();
      Destroy(texture);
      //string base64Image = $"data:image/png;base64,{System.Convert.ToBase64String(imageBytes)}";
      //string _headerValue = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";
      Debug.Log("uploading letter bytes " + targetLetter);
      WWWForm formrequest = new WWWForm();
      //formrequest.AddField("Imageurl", base64Image);
      formrequest.AddBinaryData("Imageurl", bytes, targetLetter + ".png", "image/png");
      formrequest.AddField("RequiredClass", targetLetter);
      string combinedPath = Path.Combine(WEB_URL, TRACING_MODULE);
      Debug.Log("combinedPath " + combinedPath);
      UnityWebRequest www = UnityWebRequest.Post(combinedPath, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      Debug.Log("Loading...");
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      //PopUpsManager.Instance.LoadLoadingBar().HideLoadingBar();
      if (www.downloadHandler != null && www.downloadHandler.error != null) {
        Debug.LogError("API Response Error: " + www.downloadHandler.error);
      }
      if (www.downloadHandler != null && www.downloadHandler.text != null) {
        Debug.Log("<color=green> API Response: " + www.downloadHandler.text + " </color>");
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(true, null);
        } else {
          RatingData ratingData = JsonUtility.FromJson<RatingData>(www.downloadHandler.text);
          statusCallback?.Invoke(true, ratingData);
        }
      }
    }


    public IEnumerator Upload() {
      MusicController.USE.PlaySound(MusicController.USE.cameraSound);
      TracingUI.Instance.SetTracingGuide(false);
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Rect rect = ColoringBookManager.Instance.GUIRectWithObject(ColoringBookManager.Instance.gameObject);
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();
      yield return 0;
      byte[] bytes = texture.EncodeToPNG();
      Destroy(texture);
      TracingUI.Instance.SetTracingGuide(true);
      //string base64Image = $"data:image/png;base64,{System.Convert.ToBase64String(bytes)}"
      string _headerValue = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";
      WWWForm formrequest = new WWWForm();
      formrequest.AddBinaryData("Imageurl", bytes, "test.jpeg", "image/jpeg");
      formrequest.AddField("RequiredClass", LettersHelper.GetActiveLetterClass());
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
    }

    IEnumerator UploadProgressBar(UnityWebRequest request) {
      LoadingBar loadingBar = PopUpsManager.Instance.LoadLoadingBar();
      loadingBar.ShowLoadingBar("Loading");
      while (!request.isDone) {
        loadingBar.IncreaseLoadingBar(0.1f);
        yield return new WaitForSeconds(1f);
      }
      loadingBar.HideLoadingBar();
    }
  }
}