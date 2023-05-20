using System;
using System.Collections;
using KendaGame.Helpers;
using KendaGame.Navigation;
using ScorpionSteps.Utils;
using UnityEngine;
using UnityEngine.UI;
using static KendaGame.Models.TracingModels;

namespace KendaGame.UI {
  public class TracingUI : MonoBehaviour {

    [SerializeField]
    AudioSource musicLoop;
    [SerializeField]
    Material coloringMaterial;
    [SerializeField]
    Sprite selectedFontSize, noneSelectedFontSize;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtns;
    [SerializeField]
    Image tracingGuide;
    [SerializeField]
    AudioClip cameraShutter;

    private ColoringPen[] penBtns;
    public RawImage screenshotRef;

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

    public void SetTracingGuide(bool state) {
      if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.TracingBasic) {
        tracingGuide.gameObject.SetActive(state);
      }
    }

    private void LoadTracingGuide() {
      //TODO load tracing guide based on active letter
      //tracingGuide.sprite = tracingGuides[LettersHelper.GetActiveLetterIndex()];
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
    }

    public void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        fontSizeBtns[x].GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.6f);
      }
      fontBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 1);
      ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(fontSize);
    }

    public void OnTracingLetterCompleted() {
      StartCoroutine(PrepareLetterForUpload(OnRatingCompleted));
    }

    private IEnumerator PrepareLetterForUpload(Action<bool, RatingData> onComplete) {
      AudioController.Instance.PlaySfxAtPoint(cameraShutter);
      SetTracingGuide(false);
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Rect rect = ColoringBookManager.Instance.GUIRectWithObject(ColoringBookManager.Instance.gameObject);
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();
      yield return 0;
      byte[] bytes = texture.EncodeToPNG();
      Destroy(texture);
      SetTracingGuide(true);
      HTTPRequestHandler.Instance.UploadTracingLetter(bytes, onComplete);
    }

    private void OnRatingCompleted(bool success, RatingData ratingData) {
      LetterRatingPopUp letterRatingPopUp = PopUpsManager.Instance.LoadRatingLetterInstance();
      if (success) {
        if (ratingData != null) {
          Debug.Log("ratingData.PredictedClass " + ratingData.PredictedClass);
          Debug.Log("ratingData.PredictedClass " + ratingData.PredictedScore);
          if (letterRatingPopUp != null) {
            SetRating(ratingData.PredictedScore, letterRatingPopUp);
          }
        } else {
          if (letterRatingPopUp != null) {
            SetRating(0f, letterRatingPopUp);
          }
        }
      } else {
        if (letterRatingPopUp != null) {
          SetRating(0f, letterRatingPopUp);
        }
      }
    }

    private void SetRating(float rating, LetterRatingPopUp ratingPopUp) {
      if (rating < ratingPopUp.OneStarRating) {
        ratingPopUp.PlayStarsAnimation(1);
      } else {
        ratingPopUp.PlayStarsAnimation(rating >= ratingPopUp.FullStarsRating ? 3 : 2);
      }
    }
  }
}