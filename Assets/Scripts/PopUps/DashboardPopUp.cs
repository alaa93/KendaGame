using UnityEngine;
using KendaGame.Helpers;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using static KendaGame.Models.UserModels;
using KendaGame.Navigation;
using System;

public class DashboardPopUp : MonoBehaviour {
  [SerializeField]
  UIButtonAnimator exitButton;
  [SerializeField]
  TextMeshProUGUI starsEarned;

  [SerializeField]
  TMP_InputField player_name;

  [SerializeField]
  UIButtonAnimator changePhoto;

  [SerializeField]
  Image profileImage;

  private static event UnityAction OnSaveBtnPressed;
  private void OnEnable() {
    OnSaveBtnPressed += ChangeName;
  }

  private void OnDisable() {
    OnSaveBtnPressed -= ChangeName;
  }
  private void Start() {
    InitializeButtons();
    SetProgressBar();
    SetStarsEarned();
    LoadCachedImage();
  }
  private void LoadCachedImage() {
    string storedUser = StorageHelper.ReadStorage(StorageHelper.UserSession);
    if (!string.IsNullOrEmpty(storedUser)) {
      UserSession user = JsonUtility.FromJson<UserSession>(storedUser);
      if (user != null) {
        Debug.Log("user.profile.profile_photo " + user.profile.profile_photo);
        if (user.profile != null && user.profile.profile_photo != null) {
          byte[] imageBytes = Convert.FromBase64String(user.profile.profile_photo);
          if (imageBytes != null) {
            // Create a new Texture2D
            Texture2D texture = new Texture2D(1, 1);

            // Load the byte array data into the Texture2D
            texture.LoadImage(imageBytes);

            // Apply the changes and return the Texture2D
            texture.Apply();
            profileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100);
          }
        }
      }
    }
  }

  private void InitializeButtons() {
    exitButton.AddClickListener(OnExitButtonPressed);
    changePhoto.AddClickListener(OnChangePhotoClick);
  }
  public void ChangeName() {
    //is gonna be implemented after database is all set and done
    //name in the database = player_name.text
  }
  private void SetStarsEarned() {
    //retrieve stars earned from the database
    int query = 0;
    starsEarned.text = query.ToString();
  }
  private void SetProgressBar() {
    //is gonna be implemented after database implementation
  }
  private void OnExitButtonPressed() {
    gameObject.SetActive(false);
  }
  public void ShowPopUp() {
    gameObject.SetActive(true);
  }
  public void OnChangePhotoClick() {
    GalleryHelper.PickImage(PhotoChangeCompleted);
  }
  private void PhotoChangeCompleted(bool success, Texture2D image) {
    if (success) {
      profileImage.sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
      HTTPRequestHandler.Instance.UpdateUserProfile("male", "012345678", image, (success, userSession, msg) => {
        if (success) {
          string storedUser = StorageHelper.ReadStorage(StorageHelper.UserSession);
          UserSession user = JsonUtility.FromJson<UserSession>(storedUser);
          user.profile = userSession.profile;
          string newUser = JsonUtility.ToJson(user);
          StorageHelper.WriteStorage(StorageHelper.UserSession, newUser);
          PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp(AuthErrorCode.UpdateProfile_Photo_Title_Msg, AuthErrorCode.UpdateProfile_Success);
        } else {
          PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp(AuthErrorCode.UpdateProfile_Photo_Title_Msg, AuthErrorCode.UpdateProfile_Failed);
        }
      });
    } else {
      Debug.Log(success);
    }
  }
}
