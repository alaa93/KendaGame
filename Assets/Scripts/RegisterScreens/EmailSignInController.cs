using System;
using System.Text.RegularExpressions;
using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.UI;
using UnityEngine;

namespace KendaGame.RegistrationScreens {
  public class EmailSignInController : MonoBehaviour {
    LoadingBar loadingBar;

    public void SignUpWithEmail(string email, string password, string confirmPassword, Action<bool, string> onComplete = null) {
      if (ValidateEmail(email) && ValidatePassword(password) && PasswordMatch(password, confirmPassword)) {
        GetLoadingBar().ShowLoadingBar();
        GetLoadingBar().IncreaseloadingBarByAnim(0.1f);
        HTTPRequestHandler.Instance.SignUpWithEmail(email, password, (success, user, errorCode) => {
          if (success) {
            if (user != null) {
              onComplete?.Invoke(success, AuthErrorCode.SignUp_Success);
            }
          } else {
            onComplete?.Invoke(success, errorCode);
          }
        });
      }
    }

    public void SignInWithEmail(string email, string password, Action<bool, string> onComplete = null) {
      if (ValidateEmail(email) && ValidatePassword(password)) {
        Debug.Log("both email and password valid");
        GetLoadingBar().ShowLoadingBar();
        GetLoadingBar().IncreaseloadingBarByAnim(0.1f);
        HTTPRequestHandler.Instance.SignInWithEmail(email, password, (success, user, errorCode) => {
          if (success) {
            if (user != null) {
              onComplete?.Invoke(success, AuthErrorCode.Login_Success);
            }
          } else {
            onComplete?.Invoke(success, errorCode);
          }
        });
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
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", AuthErrorCode.Email_Invalid);
        return false;
      }
      // Return true if the email is valid, false otherwise
      return match.Success;
    }

    private bool ValidatePassword(string text) {
      // Check if the password length is greater than 6 characters
      if (text.Length < 6) {
        // Password is too short
        Debug.LogWarning("Password is too short. It should be equal or greater than 6 characters.");
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", AuthErrorCode.Password_Length_Invalid);
        return false;
      }

      // Check if the password contains any spaces
      if (text.Contains(" ")) {
        // Password contains spaces
        Debug.LogWarning("Password should not contain spaces.");
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", AuthErrorCode.Password_Invalid);
        return false;
      }
      // Additional checks for password strength can be added here

      // Password is valid
      return true;
    }

    private bool PasswordMatch(string text, string text2) {
      if (text != text2) {
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.validation_title", AuthErrorCode.Password_No_Match);
        return false;
      }
      return true;
    }

  }
}

public static class AuthErrorCode {
  public static string Canceled = "registration_screens_status_signup_canceled";
  public static string UnknownError = "registration_screens_status_signup_unknown_error";
  public static string SignUp_Success = "registration_screens_status_signup_success";
  public static string Login_Success = "registration_screens_status_signin_success";
  public static string Email_already_exist = "registration_screens_status_email_already_used";
  public static string Email_Invalid = "registration_screens.validate_email_error";
  public static string Password_No_Match = "registration_screens.validate_passwords_match_error";
  public static string Password_Invalid = "registration_screens.validate_password_spaces_error";
  public static string Password_Length_Invalid = "registration_screens.validate_password_length_error";
  public static string Email_Password_404 = "registration_screens.validate_email_404";
  public static string UpdateProfile_Success = "update_profile_photo_success";
  public static string UpdateProfile_Failed = "update_profile_photo_fail";
  public static string UpdateProfile_Photo_Title_Msg = "update_profile_photo";
}