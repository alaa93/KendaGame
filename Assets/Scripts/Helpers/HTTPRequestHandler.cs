using System;
using System.Collections;
using System.IO;
using KendaGame.Navigation;
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

    public void UploadTracingLetter(byte[] imageBytes, string targetLetter, Action<bool, RatingData> statusCallback) {
      StartCoroutine(UploadTracingLetterProcess(imageBytes,targetLetter, statusCallback));
    }

    public IEnumerator UploadTracingLetterProcess(byte[] imageBytes, string targetLetter, Action<bool, RatingData> statusCallback) {
      //string base64Image = $"data:image/png;base64,{System.Convert.ToBase64String(imageBytes)}";
      Debug.Log("uploading letter bytes "+targetLetter);
      WWWForm formrequest = new WWWForm();
      //formrequest.AddField("Imageurl", base64Image);
      formrequest.AddBinaryData("Imageurl", imageBytes, "test.jpeg", "image/jpeg");
      
      formrequest.AddField("RequiredClass", targetLetter);
      string combinedPath = Path.Combine(WEB_URL, TRACING_MODULE);
      Debug.Log("combinedPath "+ combinedPath);
      UnityWebRequest www = UnityWebRequest.Post(Path.Combine(WEB_URL, TRACING_MODULE), formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      yield return www.SendWebRequest();
      PopUpsManager.Instance.LoadLoadingBar().HideLoadingBar();
      if (www.downloadHandler != null && www.downloadHandler.error != null) {
        Debug.LogError("API Response Error: " + www.downloadHandler.error);
      } if (www.downloadHandler != null && www.downloadHandler.text != null) {
        Debug.Log("<color=green> API Response: " + www.downloadHandler.text + " </color>");
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(true, null);
        } else {
          RatingData ratingData = JsonUtility.FromJson<RatingData>(www.downloadHandler.text);
          statusCallback?.Invoke(true, ratingData);
        }
      }
    }

  }
}