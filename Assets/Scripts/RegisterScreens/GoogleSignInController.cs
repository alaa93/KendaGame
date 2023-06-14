using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Google;
using TMPro;
using Firebase;
using System;

public class GoogleSignInController : MonoBehaviour {

  public TextMeshProUGUI statusText;

  public string webClientId = "208217938983-n7o03ormbicv6aiqiktb05e23qlqlmub.apps.googleusercontent.com";

  private GoogleSignInConfiguration configuration;
  FirebaseApp auth;

  // Defer the configuration creation until Awake so the web Client ID
  // Can be set via the property inspector in the Editor.
  void Awake() {
    configuration = new GoogleSignInConfiguration {
      WebClientId = webClientId,
      RequestIdToken = true
    };
    CheckFirebaseDependencies();
  }

  private void CheckFirebaseDependencies() {
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      var dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available) {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
        auth = FirebaseApp.DefaultInstance;
        AddStatusText("Firebase initialized successfully");
        // Set a flag here to indicate whether Firebase is ready to use by your app.
      } else {
        Debug.LogError(String.Format(
           "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
        AddStatusText(String.Format(
           "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
      }
    });
  }

  public void OnSignIn() {
    GoogleSignIn.Configuration = configuration;
    GoogleSignIn.Configuration.UseGameSignIn = false;
    GoogleSignIn.Configuration.RequestIdToken = true;
    AddStatusText("Calling SignIn");

    GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
      OnAuthenticationFinished);
  }

  public void OnSignOut() {
    AddStatusText("Calling SignOut");
    GoogleSignIn.DefaultInstance.SignOut();
  }

  public void OnDisconnect() {
    AddStatusText("Calling Disconnect");
    GoogleSignIn.DefaultInstance.Disconnect();
  }

  internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
    if (task.IsFaulted) {
      using (IEnumerator<System.Exception> enumerator =
              task.Exception.InnerExceptions.GetEnumerator()) {
        if (enumerator.MoveNext()) {
          GoogleSignIn.SignInException error =
                  (GoogleSignIn.SignInException)enumerator.Current;
          AddStatusText("Got Error: " + error.Status + " " + error.Message);
        } else {
          AddStatusText("Got Unexpected Exception?!?" + task.Exception);
        }
      }
    } else if (task.IsCanceled) {
      AddStatusText("Canceled");
    } else {
      AddStatusText("Welcome: " + task.Result.DisplayName + "!");
    }
  }

  public void OnSignInSilently() {
    GoogleSignIn.Configuration = configuration;
    GoogleSignIn.Configuration.UseGameSignIn = false;
    GoogleSignIn.Configuration.RequestIdToken = true;
    AddStatusText("Calling SignIn Silently");

    GoogleSignIn.DefaultInstance.SignInSilently()
          .ContinueWith(OnAuthenticationFinished);
  }


  public void OnGamesSignIn() {
    GoogleSignIn.Configuration = configuration;
    GoogleSignIn.Configuration.UseGameSignIn = true;
    GoogleSignIn.Configuration.RequestIdToken = false;

    AddStatusText("Calling Games SignIn");

    GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
      OnAuthenticationFinished);
  }

  private List<string> messages = new List<string>();
  void AddStatusText(string text) {
    //if (messages.Count == 5) {
    //  messages.RemoveAt(0);
    //}
    messages.Add(text);
    string txt = "";
    foreach (string s in messages) {
      txt += "\n" + s;
    }
    statusText.text = txt;
  }
}