using UnityEngine;
using KendaGame.Helpers;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

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
  public void OnChangePhotoClick(){
    Debug.Log(1);
    GalleryHelper.PickImage(PhotoChangeCompleted);
  }
  private void PhotoChangeCompleted(bool succes,Texture2D image){
    if (succes){
      profileImage.sprite= Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
    } else {
      Debug.Log(succes);
    }
  }
}
