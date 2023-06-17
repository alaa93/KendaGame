using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Google;
using Firebase;
using System;
using Firebase.Auth;

public class GoogleSignInController : MonoBehaviour {

  private readonly string webClientId = "208217938983-n7o03ormbicv6aiqiktb05e23qlqlmub.apps.googleusercontent.com";
  private GoogleSignInConfiguration configuration;
  private FirebaseAuth auth;

  void Awake() {
    configuration = new GoogleSignInConfiguration {
      WebClientId = webClientId,
      UseGameSignIn = false,
      RequestEmail = true,
      RequestProfile = true,
      RequestAuthCode = true,
      RequestIdToken = true,
    };
  }

  private void Start() {
    CheckFirebaseDependencies();
  }

  private void CheckFirebaseDependencies() {
    LogStatusText("Initializing Firebase");
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      var dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available) {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
        auth = FirebaseAuth.DefaultInstance;
        LogStatusText("Firebase initialized successfully");
        // Set a flag here to indicate whether Firebase is ready to use by your app.
      } else {
        Debug.LogError(string.Format(
           "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
        LogStatusText(string.Format(
           "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
      }
    });
  }

  public void OnSignIn() {
    GoogleSignIn.Configuration = configuration;
    LogStatusText("Calling SignIn");
    GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
      OnAuthenticationFinished);
  }

  public void OnSignOut() {
    LogStatusText("Calling SignOut");
    GoogleSignIn.DefaultInstance.SignOut();
  }

  public void OnDisconnect() {
    LogStatusText("Calling Disconnect");
    GoogleSignIn.DefaultInstance.Disconnect();
  }

  internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
    if (task.IsFaulted) {
      using IEnumerator<Exception> enumerator =
              task.Exception.InnerExceptions.GetEnumerator(); if (enumerator.MoveNext()) {
        GoogleSignIn.SignInException error =
                (GoogleSignIn.SignInException)enumerator.Current;
        LogStatusText("Got Error: " + error.Status + " " + error.Message);
      } else {
        LogStatusText("Got Unexpected Exception?!?" + task.Exception);
      }
    } else if (task.IsCanceled) {
      LogStatusText("Canceled");
    } else {
      LogStatusText("Welcome: " + task.Result.DisplayName + "!");
      // Get the Google sign-in result
      GoogleSignInUser signInUser = task.Result;
      string idToken = signInUser.IdToken;
      // Authenticate with Firebase using the Google ID token
      Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
      auth.SignInWithCredentialAsync(credential).ContinueWith(HandleSignInResult);
    }
  }

  private void HandleSignInResult(Task<FirebaseUser> task) {
    if (task.IsCanceled) {
      Debug.LogError("Firebase sign-in canceled.");
      return;
    }

    if (task.IsFaulted) {
      Debug.LogError("Firebase sign-in encountered an error: " + task.Exception);
      return;
    }

    // Firebase sign-in successful
    FirebaseUser user = task.Result;
    Debug.Log("Signed in as " + user.DisplayName + " (" + user.UserId + ")" + " email: " + user.Email + " PhotoUrl " + user.PhotoUrl + " provider " + user.ProviderId);
  }

  public void OnSignInSilently() {
    GoogleSignIn.Configuration = configuration;
    GoogleSignIn.Configuration.UseGameSignIn = false;
    GoogleSignIn.Configuration.RequestIdToken = true;
    LogStatusText("Calling SignIn Silently");

    GoogleSignIn.DefaultInstance.SignInSilently()
          .ContinueWith(OnAuthenticationFinished);
  }


  public void OnGamesSignIn() {
    GoogleSignIn.Configuration = configuration;
    GoogleSignIn.Configuration.UseGameSignIn = true;
    GoogleSignIn.Configuration.RequestIdToken = false;
    LogStatusText("Calling Games SignIn");
    GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
      OnAuthenticationFinished);
  }

  void LogStatusText(string text) {
    Debug.Log("FIR Status " + text);
  }
}