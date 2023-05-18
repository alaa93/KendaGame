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

      ColoringBookManager.maskTexIndex = -1;
      ColoringBookManager.ID = "ColoringList0";
      //coloringMaterial.SetVector("_MaskTex",new Vector4(0.68f,1f, 0.16f,1.02f));
    }

    public void OnColoringCompleted() {
      // show rating screen, close clouds then do something
      PopUpsManager.Instance.LoadRatingLetter();
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
    }

    public void OnFontSizeSelected(UIButtonAnimator fontBtn, int fontSize) {
      for (int x = 0; x < fontSizeBtns.Length; x++) {
        fontSizeBtns[x].GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.6f);
      }
      //fontBtn.GetComponent<Image>().sprite = selectedFontSize;
      fontBtn.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
      ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(fontSize);
    }

  }
}