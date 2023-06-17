using System;
using System.Text.RegularExpressions;
using Firebase.Auth;
using KendaGame.Navigation;
using KendaGame.UI;
using UnityEngine;

namespace KendaGame.RegistrationScreens {
  public class EmailSignInController : MonoBehaviour {
    private FirebaseAuth auth;
    LoadingBar loadingBar;

    public async void SignUpWithEmail(string email, string password, string confirmPassword, Action<bool, string> onComplete = null) {
      if (ValidateEmail(email) && ValidatePassword(password) && PasswordMatch(password, confirmPassword)) {
        GetLoadingBar().ShowLoadingBar("");
        GetLoadingBar().IncreaseloadingBarByAnim(0.1f);
        if (auth == null) {
          auth = FirebaseAuth.DefaultInstance;
        }
        string status = "";
        bool success = false;
        try {
          await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
              status = "registration_screens_status_signup_canceled";
              Debug.LogError(status);
            }
            if (task.IsFaulted) {
              status = task.Exception.Message;
              Debug.LogError(status);
              status = "registration_screens_status_signup_unknown_error";
            }
            // Firebase user has been created.
            AuthResult result = task.Result;

            if (task.Result.User == null) {
              status = "Fail to create user in database";
              Debug.LogError(status);
              status = "registration_screens_status_signup_unknown_error";
            } else {
              status = "registration_screens_status_signup_success";
              success = true;
              Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            }
          });
        } catch (Exception e) {
          string error = e.Message;
          Debug.Log("Exception caught " + error);
          if (error.Contains("The email address is already in use by another account")) {
            error = "registration_screens_status_email_already_used";
          }
          GetLoadingBar().HideLoadingBar();
          onComplete?.Invoke(false, error);
        }
        GetLoadingBar().HideLoadingBar();
        onComplete?.Invoke(success, status);
      }
    }

    public async void SignInWithEmail(string email, string password, Action<bool, string> onComplete = null) {
      if (ValidateEmail(email) && ValidatePassword(password)) {
        GetLoadingBar().ShowLoadingBar("");
        GetLoadingBar().IncreaseloadingBarByAnim(0.1f);

        if (auth == null) {
          auth = FirebaseAuth.DefaultInstance;
        }
        string status = "";
        bool success = false;

        Credential credential = EmailAuthProvider.GetCredential(email, password);
        await auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
          if (task.IsCanceled) {
            Debug.LogError("Login Canceled!");
            status = "registration_screens_status_login_canceled";
            return;
          }
          if (task.IsFaulted) {
            Debug.LogError("error: " + task.Exception.Message);
            status = task.Exception.Message;
            if (status.Contains("The password is invalid")) {
              status = "registration_screens_status_invalid_password";
            }
            return;
          }
          AuthResult result = task.Result;
          Debug.LogFormat("User signed in successfully: {0} ({1})",
              result.User.DisplayName, result.User.UserId);
          success = true;
          status = "registration_screens_status_signin_success";
        });
        GetLoadingBar().HideLoadingBar();
        onComplete?.Invoke(success, status);
      }
    }

    private LoadingBar GetLoadingBar() {
      if (loadingBar == null) {
        loadingBar = PopUpsManager.Instance.LoadLoadingBar();
      }
      return loadingBar;
    }

    private bool ValidateEmail(string text) {
      // Regular expression pattern for email validation
      string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

      // Create a Regex object with the pattern
      Regex regex = new Regex(pattern);

      // Use the Match method to check if the email matches the pattern
      Match match = regex.Match(text);
      if (!match.Success) {//"Validation"
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", "registration_screens.validate_email_error");
      }
      // Return true if the email is valid, false otherwise
      return match.Success;
    }

    private bool ValidatePassword(string text) {
      // Check if the password length is greater than 6 characters
      if (text.Length < 6) {
        // Password is too short
        Debug.LogWarning("Password is too short. It should be equal or greater than 6 characters.");
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", "registration_screens.validate_password_length_error");
        return false;
      }

      // Check if the password contains any spaces
      if (text.Contains(" ")) {
        // Password contains spaces
        Debug.LogWarning("Password should not contain spaces.");
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", "registration_screens.validate_password_spaces_error");
        return false;
      }
      // Additional checks for password strength can be added here

      // Password is valid
      return true;
    }

    private bool PasswordMatch(string text, string text2) {
      if (text != text2) {
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", "registration_screens.validate_passwords_match_error");
        return false;
      }
      return true;
    }

  }
}