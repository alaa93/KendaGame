using System;
using System.Collections;
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
    UIButtonAnimator[] fontSizeColors;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtnsOutline;
    [SerializeField]
    SkeletonGraphic animatedTracingGuide;
    [SerializeField]
    SkeletonDataAsset[] animatedTracingGuides;
    [SerializeField]
    Vector2 [] animatedGuidesAligments;
    [SerializeField]
    AudioClip cameraShutter;

    [Header("Advanced Tracing")]
    [SerializeField]
    GameObject ratingPopUp;
    [SerializeField]
    GameObject [] tracingBoard;
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
      InitFontSizeBtns();
      PrepareBoard();
      StartTracing();
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
      for (int x = 0; x < fontSizeColors.Length; x++) {
        fontSizeColors[x].GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
      }
    }

    public void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        fontSizeColors[x].GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
        fontSizeBtnsOutline[x].gameObject.SetActive(false);
        if (fontSizeBtns[x] == fontBtn) {
          fontSizeBtnsOutline[x].gameObject.SetActive(true);
        }
      }
      ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(fontSize);
    }

    public void OnTracingLetterCompleted() {
      AudioUtils.FadeOutAudio(musicLoop, 0.4f);
      StartCoroutine(PrepareLetterForUpload(OnRatingCompleted));
    }

    private IEnumerator PrepareLetterForUpload(Action<bool, RatingData> onComplete) {
      AudioController.Instance.PlaySfxAtPoint(cameraShutter);
      SetTracingGuide(false);
      SetPartsOnScreenShot(false);
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Rect rect = ColoringBookManager.Instance.GUIRectWithObject(ColoringBookManager.Instance.gameObject);
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();
      yield return 0;
      byte[] bytes = texture.EncodeToPNG();
      texture2D = texture;
      if(screenshotRef != null) {
        screenshotRef.texture = texture;
      }
      //SetTracingGuide(true);
      SetPartsOnScreenShot(true);
      HTTPRequestHandler.Instance.UploadTracingLetter(bytes, onComplete);
      SetPlayerTracingLetter(texture);
    }

    private void SetPlayerTracingLetter(Texture2D texture2D) {
      if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingAdvanced) {
        playerLetter.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);
      }
    }
    private void OnRatingCompleted(bool success, RatingData ratingData) {
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
      ShowLetterComparison();
      bool successfullyRated = success && ratingData != null && ratingData.PredictedClass == LettersHelper.GetActiveLetterClass() && ratingData.StarRating > 1;
      animation = successfullyRated ? $"{ratingData.StarRating}Stars" : animation;
      skeletonGraphic.AnimationState.SetAnimation(0, animation, false);

      int.TryParse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex()), out int currentRating);
      currentRating += successfullyRated ? Math.Clamp(ratingData.StarRating, 0, 1) : 0;
      currentRating = currentRating > 2 ? 2 : currentRating;// because current rating shouldn't be greater than 2 -> equivalent to 3 stars
      StorageHelper.WriteStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex(), currentRating.ToString());
    }

    private void ShowLetterComparison() {
      //Player Letter
      playerLetter.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);
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
          currentRating = currentRating > 2 ? 2 : currentRating;// because current rating shouldn't be greater than 2 -> equivalent to 3 stars
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

  }
}