using DG.Tweening;
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
    UIButtonAnimator[] fontSizeBtnsOutline;

    [SerializeField] private Transform fontHoldar;
    [SerializeField] private Sprite[] fontSizeImage;
    [SerializeField] private UIButtonAnimator fontSizeMainButton;

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
      currentRating = currentRating > 3 ? 3 : currentRating;// because current rating shouldn't be greater than 3 stars
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
      for (int x = 0; x < fontSizeBtnsOutline.Length; x++) {
        fontSizeBtnsOutline[x].GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
      }
      fontSizeMainButton.GetComponent<Image>().color = ColoringBookManager.Instance.paintColor;
    }

    private void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
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
  }
}