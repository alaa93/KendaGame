using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class ColoringUI : MonoBehaviour {

    [SerializeField]
    AudioSource musicLoop;
    [SerializeField]
    Material coloringMaterial;
    [SerializeField]
    Sprite selectedFontSize, noneSelectedFontSize;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtns;
    [SerializeField]
    UIButtonAnimator[] fontSizeColors;
    [SerializeField]
    UIButtonAnimator[] fontSizeBtnsOutline;

    private ColoringPen[] penBtns;
    public RawImage screenshotRef;

    private static ColoringUI _instance;
    public static ColoringUI Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<ColoringUI>();
        }
        return _instance;
      }
    }


    private void Start() {
      SceneManager.Instance.OpenCloudsManually();
      InitFontSizeBtns();
      PrepareBoard();
      StartColoring();
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
    }

    public void OnColoringCompleted() {
      PopUpsManager.Instance.GetLetterRatingPopUp().PlayStarsAnimation(3);
      int.TryParse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex()), out int currentRating);
      if (currentRating <= 0) {
        currentRating += 1;
      }
      StorageHelper.WriteStorage(StorageHelper.MapLetterProgress + "_" + LettersHelper.GetActiveLetterIndex(), $"{currentRating}");
    }

    private void StartColoring() {
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

    private void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        fontSizeColors[x].GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
        fontSizeBtnsOutline[x].gameObject.SetActive(false);
        if (fontSizeBtns[x] == fontBtn) {
          fontSizeBtnsOutline[x].gameObject.SetActive(true);
        }
      }
      ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(fontSize);
    }

  }
}