using System;
using System.Collections;
using System.Threading.Tasks;
using I2.Loc;
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
    private static readonly string UPDATE_PLAYER_CONFIG = "/user-update-appconfig-by-token";
    private static readonly string USER_GET_DETAILS_BY_TOKEN_SOCIALMEDIA = "user-get-details-by-token-socialmedia";
    private static readonly string USER_REGISTER_BY_SOCIALMEDIA = "/user-register-by-socialmedia";
    private static readonly string GET_USER_DETIALS_BY_SOCIALMEDIA_TOKEN = "/user-get-details-by-token-socialmedia";

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

    public void UploadTracingLetter(byte[] bytes, bool cameraImage, Action<bool, RatingData> statusCallback) {
      StartCoroutine(UploadTracingLetterProcess(bytes, cameraImage, statusCallback));
    }

    public void SignUpWithEmail(string email, string password,
      Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(SignUpRequest(email, password, statusCallback));
    }

    public void SignInWithEmail(string email, string password,
      Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(SignInRequest(email, password, statusCallback));
    }

    public void UpdateUserProfile(string gender, string phone, Texture2D profilePhoto,
      Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(UpdateUserProfileRequest(gender, phone, profilePhoto, statusCallback));
    }

    public void UpdatePlayerConfig(Action<bool, PlayerConfig, string> statusCallback = null) {
      StartCoroutine(UpdatePlayerConfigRequest(statusCallback));
    }

    public void UserUpdateLetterProgress(LetterProgress progress,
      Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(UserUpdateLetterProgressRequest(progress, statusCallback));
    }

    public void RegisterBySocialMedia(string firstname, string lastname, string registerBy, string token_SocialMedia,
      Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(RegisterBySocialMediaRequest(firstname, lastname, registerBy, token_SocialMedia, statusCallback));
    }

    public void GetUserDetailsBySocialMediaToken(string token,
      Action<bool, UserSession, string> statusCallback = null) {
      StartCoroutine(GetUserDetailsBySocialMediaTokenRequest(token, statusCallback));
    }

    private IEnumerator UploadTracingLetterProcess(byte[] bytes, bool cameraImage,
      Action<bool, RatingData> statusCallback) {
      WWWForm formrequest = new WWWForm();
      formrequest.AddBinaryData("Imageurl", bytes, "letter.png", "image/png");
      formrequest.AddField("RequiredClass", LettersHelper.GetActiveLetterClass());
      formrequest.AddField("ImageSource", cameraImage ? "Camera" : "Board");
      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + TRACING_MODULE, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }

      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null);
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(true, null);
        }
        else {
          RatingData ratingData = JsonUtility.FromJson<RatingData>(www.downloadHandler.text);
          statusCallback?.Invoke(true, ratingData);
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null);
      }
    }

    private IEnumerator SignUpRequest(string email, string password,
      Action<bool, UserSession, string> statusCallback = null) {
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
        if (www.downloadHandler.error.Contains("username already exists") ||
            www.downloadHandler.error.Contains("This field must be unique")) {
          error = AuthErrorCode.Email_already_exist;
        }

        statusCallback?.Invoke(false, null, error);
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        }
        else {
          UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
          statusCallback?.Invoke(true, user, AuthErrorCode.SignUp_Success);
          StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }

    private IEnumerator SignInRequest(string email, string password,
      Action<bool, UserSession, string> statusCallback = null) {
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
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        }
        else {
          if (www.downloadHandler.text.Contains("invalid user")) {
            statusCallback?.Invoke(false, null, AuthErrorCode.Email_Password_404);
          }
          else {
            UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
            statusCallback?.Invoke(true, user, AuthErrorCode.Login_Success);
            StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
          }
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }

    private IEnumerator UpdateUserProfileRequest(string gender, string phone, Texture2D profilePhoto,
      Action<bool, UserSession, string> statusCallback = null) {
      WWWForm formrequest = new WWWForm();
      formrequest.AddField("gender", gender);
      formrequest.AddField("phone", phone);
      profilePhoto = GalleryHelper.GetReadableTexture(profilePhoto);
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
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        }
        else {
          if (www.downloadHandler.text.Contains("invalid user")) {
            statusCallback?.Invoke(false, null, AuthErrorCode.Email_Password_404);
          }
          else {
            UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
            statusCallback?.Invoke(true, user, AuthErrorCode.Login_Success);
            StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
          }
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }

    private IEnumerator UpdatePlayerConfigRequest(Action<bool, PlayerConfig, string> statusCallback) {
      PlayerConfig playerConfigBeforeRequest = new PlayerConfig();
      playerConfigBeforeRequest.app_version = "0.1";
      playerConfigBeforeRequest.freeze_app = false;
      playerConfigBeforeRequest.force_update = false;
      playerConfigBeforeRequest.mute_music =
        (StorageHelper.ReadStorage(StorageHelper.ISMUSICMUTED) == "false" ? false : true);
      playerConfigBeforeRequest.mute_sfx =
        (StorageHelper.ReadStorage(StorageHelper.ISSFXMUTED) == "false" ? false : true);
      playerConfigBeforeRequest.local_lang = LocalizationManager.CurrentLanguageCode;

      WWWForm formrequest = new WWWForm();
      formrequest.AddField("app_version", "0.1");
      formrequest.AddField("freeze_app", "false");
      formrequest.AddField("force_update", "false");
      formrequest.AddField("mute_music", StorageHelper.ReadStorage(StorageHelper.ISMUSICMUTED));
      formrequest.AddField("mute_sfx", StorageHelper.ReadStorage(StorageHelper.ISSFXMUTED));
      formrequest.AddField("local_lang", LocalizationManager.CurrentLanguageCode);

      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + UPDATE_PLAYER_CONFIG, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }

      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(true, null, AuthErrorCode.UnknownError);
        }
        else {

          PlayerConfigs playerConfigAfterRequest = JsonUtility.FromJson<PlayerConfigs>(www.downloadHandler.text);
          if (playerConfigAfterRequest.appconfig == playerConfigBeforeRequest) {
            Debug.Log(playerConfigAfterRequest.appconfig.mute_music);
            Debug.Log(playerConfigAfterRequest.appconfig.mute_sfx);
            Debug.Log(playerConfigAfterRequest.appconfig.freeze_app);
            Debug.Log(playerConfigAfterRequest.appconfig.force_update);
            Debug.Log(playerConfigAfterRequest.appconfig.app_version);
            Debug.Log(playerConfigAfterRequest.appconfig.local_lang);
            statusCallback?.Invoke(true, playerConfigAfterRequest.appconfig, "Success!");
          }
          else {
            statusCallback?.Invoke(true, playerConfigAfterRequest.appconfig, "didn't send to server!");
          }
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, "null response/unknown response.");
      }
    }

    private IEnumerator RegisterBySocialMediaRequest(string firstname, string lastname, string registerBy,string token_SocialMedia,
      Action<bool, UserSession, string> statusCallback = null) {
      WWWForm formrequest = new();

      formrequest.AddField("first_name", firstname);
      formrequest.AddField("last_name", lastname);
      formrequest.AddField("RegisterBy", registerBy);
      formrequest.AddField("token_SocialMedia", token_SocialMedia);

      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + USER_REGISTER_BY_SOCIALMEDIA, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }

      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        string error = AuthErrorCode.UnknownError;
        if (www.downloadHandler.error.Contains("username already exists") ||
            www.downloadHandler.error.Contains("This field must be unique")) {
          error = "account already exists for this platform.";
        }

        statusCallback?.Invoke(false, null, error);
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        }
        else {
          UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
          statusCallback?.Invoke(true, user, AuthErrorCode.SignUp_Success);
          StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }

    }


    private IEnumerator UserUpdateLetterProgressRequest(LetterProgress progress,
      Action<bool, UserSession, string> statusCallback = null) {

      WWWForm formrequest = new WWWForm();

      formrequest.AddField("letter_id", progress.letter_id);
      formrequest.AddField("letter_rating", progress.letter_rating);
      formrequest.AddField("game_1_completed", progress.game_1_completed.ToString());
      formrequest.AddField("game_2_completed", progress.game_2_completed.ToString());
      formrequest.AddField("game_3_completed", progress.game_3_completed.ToString());
      formrequest.AddField("coloring_completed", progress.coloring_completed.ToString());
      formrequest.AddField("tracing_basic_completed", progress.tracing_basic_completed.ToString());
      formrequest.AddField("tracing_advanced_completed", progress.tracing_advanced_completed.ToString());

      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + USER_GET_DETAILS_BY_TOKEN_SOCIALMEDIA, formrequest);
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
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        }
        else {
          if (www.downloadHandler.text.Contains("invalid user")) {
            statusCallback?.Invoke(false, null, AuthErrorCode.Email_Password_404);
          }
          else {
            UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
            statusCallback?.Invoke(true, user, AuthErrorCode.Login_Success);
            StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
          }
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
    }

    public async Task<(bool, string)> MakeWebRequest(string url, WWWForm formRequest) {
      Debug.Log("url " + url);
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
        }
        else {
          Debug.Log("Response success " + request.downloadHandler.text);
          tcs.SetResult((true, request.downloadHandler.text));
        }
      };

      // Wait for the task to complete and return the result
      return await tcs.Task;
    }

    private IEnumerator GetUserDetailsBySocialMediaTokenRequest(string socialMediaToken,
      Action<bool, UserSession, string> statusCallback = null) {
      WWWForm formrequest = new WWWForm();
      formrequest.AddField("token_SocialMedia", socialMediaToken);
      UnityWebRequest www = UnityWebRequest.Post(WEB_URL + GET_USER_DETIALS_BY_SOCIALMEDIA_TOKEN, formrequest);
      www.SetRequestHeader("Authorization", REQUEST_HEADER);
      StartCoroutine(UploadProgressBar(www));
      yield return www.SendWebRequest();
      if (loadingBar != null) {
        loadingBar.HideLoadingBar();
      }

      if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.error)) {
        Debug.Log("Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
      else if (www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text)) {
        Debug.Log("Response success " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("<!DOCTYPE html>")) {
          statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
        }
        else {
          UserSession user = JsonUtility.FromJson<UserSession>(www.downloadHandler.text);
          statusCallback?.Invoke(true, user, "success!");
          StorageHelper.WriteStorage(StorageHelper.UserSession, www.downloadHandler.text);
        }
      }
      else {
        Debug.Log("Unknown Response " + www.downloadHandler.text);
        Debug.Log("Unknown Response error " + www.downloadHandler.error);
        statusCallback?.Invoke(false, null, AuthErrorCode.UnknownError);
      }
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

    private string SocialMediaTokenGenerator(int len) {
      string token = "";
      string domain = "abcdefghijklmnopqrstuvwxyz0123456789";
      for (int i = 0; i < len; i++) {
        token += domain[UnityEngine.Random.Range(0, domain.Length)];
      }

      return token;
    }
  }
}

// Custom exception class for UnityWebRequest errors
class UnityWebRequestException : Exception {
  public UnityWebRequestException(string message) : base(message) {
  }
}