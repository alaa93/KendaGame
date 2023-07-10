using System;
using System.Collections;
using KendaGame.Navigation;
using KendaGame.UI;
using UnityEngine;
using UnityEngine.Networking;
using static KendaGame.Models.TracingModels;

namespace KendaGame.Helpers {
  public class HTTPRequestHandler : MonoBehaviour {

    private static readonly string WEB_URL = "http://137.184.184.223:8000";
    private static readonly string TRACING_MODULE = "/ClientPredict/";
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
    LoadingBar loadingBar;

    public void UploadTracingLetter(byte[] bytes,bool cameraImage, Action<bool, RatingData> statusCallback) {
      StartCoroutine(UploadTracingLetterProcess(bytes,cameraImage, statusCallback));
    }

    private IEnumerator UploadTracingLetterProcess(byte[] bytes, bool cameraImage, Action<bool, RatingData> statusCallback) {
      WWWForm formrequest = new WWWForm();
      formrequest.AddBinaryData("Imageurl", bytes, "letter.png", "image/png");
      formrequest.AddField("RequiredClass", LettersHelper.GetActiveLetterClass());
      formrequest.AddField("ImageSource", cameraImage ? "Camera" : "Board");
      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + TRACING_MODULE, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www, "tracing.rating_in_progress"));
      yield return www.SendWebRequest();
      if(loadingBar != null) {
        loadingBar.HideLoadingBar();
      }
      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null);
      } else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(true, null);
        } else {
          RatingData ratingData = JsonUtility.FromJson<RatingData>(www.downloadHandler.text);
          statusCallback?.Invoke(true, ratingData);
        }
      } else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null);
      }
    }

    private IEnumerator UploadProgressBar(UnityWebRequest request, string loadingTitle) {
      loadingBar = PopUpsManager.Instance.LoadLoadingBar();
      loadingBar.ShowLoadingBar(loadingTitle);
      while (!request.isDone) {
        loadingBar.IncreaseLoadingBar(0.1f);
        yield return new WaitForSeconds(1f);
      }
      loadingBar.HideLoadingBar();
    }
  }
}