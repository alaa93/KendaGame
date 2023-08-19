using System;
using System.Collections;
using System.Threading.Tasks;
using KendaGame.Navigation;
using KendaGame.UI;
using UnityEngine;
using UnityEngine.Networking;
using static KendaGame.Models.TracingModels;
using static KendaGame.Models.UserModels;

namespace KendaGame.Helpers {

  public class HTTPRequestHandler : MonoBehaviour {



    private static readonly string REQUEST_HEADER = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";
    private static readonly string WEB_URL = "http://137.184.184.223:8000";
    private static readonly string TRACING_MODULE = "/ClientPredict/";
    private static readonly string USER_REGISTER_MODULE = "/user-register";
    private static readonly string USER_LOGIN_MODULE = "/user-login";
    private static readonly string USER_UPDATE_PROFILE_BY_TOKEN = "/user-update-profile-by-token";


    private static HTTPRequestHandler _instance;
    public static HTTPRequestHandler Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<HTTPRequestHandler>();
        }
        return _instance;
      }
    }
    private LoadingBar loadingBar;

   
    public void UploadTracingLetter(byte[] bytes,bool cameraImage, Action<bool, RatingData> statusCallback) {
      StartCoroutine(UploadTracingLetterProcess(bytes,cameraImage, statusCallback));
    }

    public void SignUpWithEmail(string email, string password, Action<bool, UserSession,string> statusCallback = null) {
      StartCoroutine(SignUpRequest(email, password, statusCallback));
    }

    public void SignInWithEmail(string email, string password, Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(SignInRequest(email, password, statusCallback));
    }
    public void UpdateUserProfile(string gender, string phone, Texture2D profilePhoto, Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(UpdateUserProfileRequest(gender, phone, profilePhoto,statusCallback));
    }
    
    private IEnumerator UploadTracingLetterProcess(byte[] bytes, bool cameraImage, Action<bool, RatingData> statusCallback) {
      WWWForm formrequest = new WWWForm();
      formrequest.AddBinaryData("Imageurl", bytes, "letter.png", "image/png");
      formrequest.AddField("RequiredClass", LettersHelper.GetActiveLetterClass());
      formrequest.AddField("ImageSource", cameraImage ? "Camera" : "Board");
      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + TRACING_MODULE, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));
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

    private IEnumerator SignUpRequest(string email, string password, Action<bool, UserSession, string> statusCallback = null) {
      WWWForm formrequest = new();

      formrequest.AddField("email", email);
      formrequest.AddField("password", password);
      formrequest.AddField("first_name", "first_name");
      formrequest.AddField("last_name", "last_name");

      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + USER_REGISTER_MODULE, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }
      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        string error = AuthErrorCode.UnknownError;
        if(www.downloadHandler.error.Contains("username already exists") || www.downloadHandler.error.Contains("This field must be unique")) {
          error = AuthErrorCode.Email_already_exist;
        }
        statusCallback?.Invoke(false, null, error);
      } else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        } else {
          UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
          statusCallback?.Invoke(true, user, AuthErrorCode.SignUp_Success);
          StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
        }
      } else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }

    private IEnumerator SignInRequest(string email, string password, Action<bool, UserSession, string> statusCallback = null) {
      WWWForm formrequest = new WWWForm();
      formrequest.AddField("email", email);
      formrequest.AddField("password", password);
      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + USER_LOGIN_MODULE, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));

      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }
      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        string error = AuthErrorCode.UnknownError;
        if (www.downloadHandler.error.Contains("invalid user")) {
          error = AuthErrorCode.Email_Password_404;
        }
        statusCallback?.Invoke(false, null, error);
      } else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        } else {
          if (www.downloadHandler.text.Contains("invalid user")) {
            statusCallback?.Invoke(false, null, AuthErrorCode.Email_Password_404);
          } else {
            UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
            statusCallback?.Invoke(true, user, AuthErrorCode.Login_Success);
            StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
          }
        }
      } else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }

    private IEnumerator UpdateUserProfileRequest(string gender, string phone, Texture2D profilePhoto, Action<bool, UserSession, string> statusCallback = null) {
      
      WWWForm formrequest = new WWWForm();
      formrequest.AddField("gender", gender);
      formrequest.AddField("phone", phone);
      profilePhoto = GetReadableTexture(profilePhoto);
      byte[] photoBytes = profilePhoto.EncodeToJPG();
      formrequest.AddBinaryData("profile_photo", photoBytes, "profile.png", "image/png");

      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + USER_UPDATE_PROFILE_BY_TOKEN, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));

      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }
      Destroy(profilePhoto);
      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        string error = AuthErrorCode.UnknownError;
        if (www.downloadHandler.error.Contains("invalid user")) {
          error = AuthErrorCode.Email_Password_404;
        }
        statusCallback?.Invoke(false, null, error);
      } else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        } else {
          if (www.downloadHandler.text.Contains("invalid user")) {
            statusCallback?.Invoke(false, null, AuthErrorCode.Email_Password_404);
          } else {
            UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
            statusCallback?.Invoke(true, user, AuthErrorCode.Login_Success);
            StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
          }
        }
      } else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }
    private Texture2D GetReadableTexture(Texture2D source) {
      RenderTexture renderTex = RenderTexture.GetTemporary(
                            source.width,
                            source.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

      Graphics.Blit(source, renderTex);
      RenderTexture previous = RenderTexture.active;
      RenderTexture.active = renderTex;
      Texture2D readableText = new Texture2D(source.width, source.height);
      readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
      readableText.Apply();
      RenderTexture.active = previous;
      RenderTexture.ReleaseTemporary(renderTex);
      return readableText;
    }

    public async Task<(bool, string)> MakeWebRequest(string url, WWWForm formRequest) {
      Debug.Log("url "+url);
      UnityWebRequest request = UnityWebRequest.Post(url, formRequest);
      request.SetRequestHeader("Authorization", REQUEST_HEADER);
      var tcs = new TaskCompletionSource<(bool, string)>();

      //Show loading bar
      StartCoroutine(UploadProgressBar(request));

      // Start the web request asynchronously
      request.SendWebRequest().completed += operation => {

        //Hide loading bar
        HideLoadingBar();
        if (request.result != UnityWebRequest.Result.Success) {
          Debug.Log("Response error " + request.error);
          tcs.SetResult((false, request.error));
          //tcs.SetException(new UnityWebRequestException(request.error));
        } else {
          Debug.Log("Response success " + request.downloadHandler.text);
          tcs.SetResult((true, request.downloadHandler.text));
        }
      };

      // Wait for the task to complete and return the result
      return await tcs.Task;
    }

    private IEnumerator UploadProgressBar(UnityWebRequest request) {
      loadingBar = PopUpsManager.Instance.LoadLoadingBar();
      loadingBar.ShowLoadingBar();
      while (!request.isDone) {
        loadingBar.IncreaseLoadingBar(0.1f);
        yield return new WaitForSeconds(1f);
      }
      loadingBar.HideLoadingBar();
    }

    private void HideLoadingBar() {
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }
    }
  }
}
 
// Custom exception class for UnityWebRequest errors
class UnityWebRequestException : Exception {
  public UnityWebRequestException(string message) : base(message) { }
}