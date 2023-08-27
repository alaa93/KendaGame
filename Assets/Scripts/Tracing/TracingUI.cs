using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using ScorpionSteps.Utils;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using static KendaGame.Models.TracingModels;

namespace KendaGame.UI {
  public class TracingUI : MonoBehaviour {

    [SerializeField]
    AudioSource musicLoop;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtns;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtnsOutline;
    [SerializeField]
    SkeletonGraphic animatedTracingGuide;
    [SerializeField]
    SkeletonDataAsset[] animatedTracingGuides;
    [SerializeField]
    Vector2[] animatedGuidesAligments;
    [SerializeField]
    AudioClip cameraShutter;
    [SerializeField]
    Transform fontHoldar;
    [SerializeField]
    private Sprite[] fontSizeImage;
    [SerializeField]
    private UIButtonAnimator fontSizeMainButton;

    [Header("LettetSkeleton")]
    [SerializeField] private SkeletonGraphic letterSkeleton;
    [SerializeField]
    private SkeletonDataAsset[] letterSkeletonDataAssets;
    public List<Vector3> letterSkeletonScales = new List<Vector3>();
    public List<Vector3> letterSkeletonPositions = new List<Vector3>();

    [Header("Advanced Tracing")]
    [SerializeField]
    GameObject ratingPopUp;
    [SerializeField]
    GameObject[] tracingBoard;
    [SerializeField]
    Image letterGuide, playerLetter;
    [SerializeField]
    SkeletonGraphic animatedTracingGuideRating;
    [SerializeField]
    GameObject[] partsToHide;
    [SerializeField]
    SkeletonGraphic skeletonGraphic;
    [SerializeField]
    Vector2[] popUpAnimatedLetterAligments;
    [SerializeField]
    Vector2[] popUpAnimatedLetterScale;

    private ColoringPen[] penBtns;
    public RawImage screenshotRef;
    private Texture2D texture2D;

    [Header("Camera Controller")]
    [SerializeField]
    GameObject[] partsToHideForCamera;
    [SerializeField]
    GameObject[] partsToShowForCamera;
    [SerializeField]
    UIButtonAnimator cameraBtn, cameraSubmitBtn;
    [SerializeField]
    NativeCameraController nativeCamera;
    Texture2D cameraScreenshot;
    bool isCameraTracing = false;

    [SerializeField] AudioClip startAudio;
    [SerializeField] AudioClip[] characterVoice;
    [SerializeField] AudioClip[] characterName;
    private static TracingUI _instance;
    public static TracingUI Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<TracingUI>();
        }
        return _instance;
      }
    }

    private void Awake() {
      SceneManager.Instance.OpenCloudsManually();
      InitializeCameraBtns();
      InitFontSizeBtns();
      PrepareBoard();
      StartTracing();
      IntiLettetSkeleton();
    }
    private void Start() {
      int ActiveLetterIndex = LettersHelper.GetActiveLetterIndex();
      AudioController.Instance.AddAudiosToQueue(new List<AudioClip> { startAudio, characterVoice[ActiveLetterIndex],
      characterName[ActiveLetterIndex]});
      AudioController.Instance.PlayAudioFromQueue();
    }

    private void InitializeCameraBtns() {
      isCameraTracing = false;
      cameraBtn.AddClickListener(() => {
        isCameraTracing = true;
        PrepareBoardForCamera();
      }, true);

      cameraSubmitBtn.AddClickListener(async () => {
        cameraScreenshot = await nativeCamera.CaptureCameraScreen();
        if (cameraScreenshot != null) {
          StartCoroutine(PrepareLetterForUpload(OnRatingCompleted, true));
        }
      }, true);
    }

    private void PrepareBoardForCamera() {
      for (int x = 0; x < partsToHideForCamera.Length; x++) {
        partsToHideForCamera[x].SetActive(false);
      }
      for (int x = 0; x < partsToShowForCamera.Length; x++) {
        partsToShowForCamera[x].SetActive(true);
      }
      nativeCamera.StartCamera();
    }

    private void SetTracingGuide(bool state) {
      if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingBasic) {
        animatedTracingGuide.gameObject.SetActive(state);
      }
    }

    private void SetPartsOnScreenShot(bool state) {
      for (int x = 0; x < partsToHide.Length; x++) {
        partsToHide[x].SetActive(state);
      }
    }

    private void LoadTracingGuide() {
      if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingBasic) {
        animatedTracingGuide.skeletonDataAsset = animatedTracingGuides[LettersHelper.GetActiveLetterIndex()];
        animatedTracingGuide.Initialize(true);
        animatedTracingGuide.MatchRectTransformWithBounds();
        animatedTracingGuide.GetComponent<RectTransform>().anchoredPosition = animatedGuidesAligments[LettersHelper.GetActiveLetterIndex()];
        animatedTracingGuide.AnimationState.SetAnimation(0, "animation", true);
      }
    }

    private void InitFontSizeBtns() {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        int temp = x;
        fontSizeBtns[temp].AddClickListener(animate: true, callBackFn: () => {
          OnFontBtnClicked(fontSizeBtns[temp], temp + 1);
        });
      }
    }

    private void OnFontBtnClicked(UIButtonAnimator fontBtn, int fontSize) {
      OnFontSizeSelected(fontBtn, fontSize);
    }

    private void PrepareBoard() {
      PlayerPrefs.SetInt("ColoringList", 0);
      PlayerPrefs.Save();
      ColoringBookManager.ID = "ColoringList0";
      LoadTracingGuide();
    }

    private void StartTracing() {
      musicLoop.Play();
      penBtns = FindObjectsOfType<ColoringPen>();
    }

    public void OnPenClicked(ColoringPen pen) {
      for (int x = 0; x < penBtns.Length; x++) {
        int temp = x;
        if (penBtns[temp] != pen) {
          penBtns[temp].SelectionChanged(true);
        }
      }
      pen.SelectionChanged();
      for (int x = 0; x < fontSizeBtnsOutline.Length; x++) {
        fontSizeBtnsOutline[x].GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
      }
      fontSizeMainButton.GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;

    }

    public void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
      for (int x = 0; x < fontSizeBtns.Length; x++) {

        fontSizeBtnsOutline[x].GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
        if (fontSizeBtns[x] == fontBtn) {
          fontSizeMainButton.GetComponent<Image>().sprite = fontSizeImage[x];
        }

      }
      ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(fontSize);
      fontSizeMainButton.GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
      OnChangeFontSizeSelected();

    }

    public void OnTracingLetterCompleted() {
      AudioUtils.FadeOutAudio(musicLoop, 0.4f);
      letterSkeleton.gameObject.SetActive(false);
      StartCoroutine(PrepareLetterForUpload(OnRatingCompleted));
    }

    private IEnumerator PrepareLetterForUpload(Action<bool, RatingData> onComplete, bool isCamera = false) {
      AudioController.Instance.PlaySfxAtPoint(cameraShutter);
      SetTracingGuide(false);
      SetPartsOnScreenShot(false);
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Texture2D texture = null;
      if (isCamera) {
        texture = cameraScreenshot;
        texture.Apply();
      } else {
        Rect rect = ColoringBookManager.Instance.GUIRectWithObject(ColoringBookManager.Instance.gameObject);
        texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();
      }
      yield return 0;
      byte[] bytes = texture.EncodeToPNG();
      texture2D = texture;
      if (screenshotRef != null) {
        screenshotRef.texture = texture;
      }
      SetPartsOnScreenShot(true);
      HTTPRequestHandler.Instance.UploadTracingLetter(bytes, isCamera, onComplete);
      SetPlayerTracingLetter(texture);
    }

    private void SetPlayerTracingLetter(Texture2D texture2D) {
      if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingAdvanced) {
        playerLetter.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);
      }
    }

    private void OnRatingCompleted(bool success, RatingData ratingData) {
      if (isCameraTracing) {
        for (int x = 0; x < partsToHideForCamera.Length; x++) {
          partsToHideForCamera[x].SetActive(true);
        }
        for (int x = 0; x < partsToShowForCamera.Length; x++) {
          partsToShowForCamera[x].SetActive(false);
        }
        InitializeCameraBtns();
      }
      if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingAdvanced) {
        ShowAdvancedTracingRating(success, ratingData);
      } else {
        ShowNormalRating(success, ratingData);
      }
    }

    private void ShowAdvancedTracingRating(bool success, RatingData ratingData) {
      ratingPopUp.SetActive(true);
      for (int x = 0; x < tracingBoard.Length; x++) {
        tracingBoard[x].SetActive(false);
      }
      string animation = "1Stars_lose";
      ShowLetterComparison(ratingData.cropped_img_base64);
      bool successfullyRated = success && ratingData != null && ratingData.PredictedClass == LettersHelper.GetActiveLetterClass() && ratingData.StarRating > 1;
      animation = successfullyRated ? $"{ratingData.StarRating}Stars" : animation;
      skeletonGraphic.AnimationState.SetAnimation(0, animation, false);
      int.TryParse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex()), out int currentRating);
      currentRating += successfullyRated ? Math.Clamp(ratingData.StarRating, 0, 1) : 0;
      currentRating = currentRating > 3 ? 3 : currentRating;// because current rating shouldn't be greater than 3 stars
      StorageHelper.WriteStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex(), currentRating.ToString());
    }

    private void ShowLetterComparison(string processedImg) {
      ////Player Letter
      if (!string.IsNullOrEmpty(processedImg)) {
        // Decode the Base64 string into a byte array
        byte[] imageBytes = Convert.FromBase64String(processedImg);

        if (imageBytes != null) {
          // Create a new Texture2D
          Texture2D texture = new Texture2D(1, 1);

          // Load the byte array data into the Texture2D
          texture.LoadImage(imageBytes);

          // Apply the changes and return the Texture2D
          texture.Apply();
          playerLetter.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100);
        } else {
          playerLetter.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);
        }
      } else {
        playerLetter.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);
      }
      playerLetter.color = new Color(255f, 255f, 255f, 0.6f);
      //Correct Letter
      animatedTracingGuideRating.skeletonDataAsset = animatedTracingGuides[LettersHelper.GetActiveLetterIndex()];
      animatedTracingGuideRating.Initialize(true);
      animatedTracingGuideRating.MatchRectTransformWithBounds();
      animatedTracingGuide.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
      animatedTracingGuide.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
      animatedTracingGuide.GetComponent<RectTransform>().anchoredPosition = popUpAnimatedLetterAligments[LettersHelper.GetActiveLetterIndex()];
      animatedTracingGuide.GetComponent<RectTransform>().sizeDelta = popUpAnimatedLetterScale[LettersHelper.GetActiveLetterIndex()];
      animatedTracingGuideRating.AnimationState.SetAnimation(0, "animation", true);
    }

    private void ShowNormalRating(bool success, RatingData ratingData) {
      LetterRatingPopUp letterRatingPopUp = PopUpsManager.Instance.GetLetterRatingPopUp();
      if (success && ratingData != null && ratingData.PredictedClass == LettersHelper.GetActiveLetterClass()) {
        if (ratingData != null) {
          letterRatingPopUp.PlayStarsAnimation(ratingData.StarRating);
          int.TryParse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex()), out int currentRating);
          currentRating += Math.Clamp(ratingData.StarRating, 0, 1);
          currentRating = currentRating > 3 ? 3 : currentRating;// because current rating shouldn't be greater than 3 stars
          StorageHelper.WriteStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex(), currentRating.ToString());
        }
      } else {
        letterRatingPopUp.PlayStarsAnimation(1);
      }
    }

    public void OnNextBtnClicked() {
      SceneManager.Instance.OnNextScreenBtnClicked();
    }

    public void OnRestartClicked() {
      SceneManager.Instance.ReloadScene();
    }
    public void OnChangeFontSizeSelected() {
      if (fontHoldar.gameObject.activeInHierarchy) {
        fontHoldar.DOLocalMoveX(-760, 0.5f).SetEase(Ease.InOutBack).OnComplete(() => {

          fontHoldar.gameObject.SetActive(false);
        });

      } else {
        fontHoldar.gameObject.SetActive(true);
        fontHoldar.DOLocalMoveX(-993, 0.5f).SetEase(Ease.InOutBack);
      }
    }

    public void IntiLettetSkeleton() {
      if (LettersHelper.GetActiveLetterIndex() <= letterSkeletonDataAssets.Length) {
        letterSkeleton.skeletonDataAsset = letterSkeletonDataAssets[LettersHelper.GetActiveLetterIndex()];
        letterSkeleton.Initialize(true);
        letterSkeleton.gameObject.GetComponent<SpineTracksUI1>().enabled = true;
        letterSkeleton.transform.localPosition = letterSkeletonPositions[LettersHelper.GetActiveLetterIndex()];
        letterSkeleton.transform.localScale = letterSkeletonScales[LettersHelper.GetActiveLetterIndex()];

      }
    }
  }
}