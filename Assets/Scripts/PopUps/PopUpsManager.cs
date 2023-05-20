using KendaGame.Tutorial;
using KendaGame.UI;
using UnityEngine;

namespace KendaGame.Navigation {
  public class PopUpsManager : MonoBehaviour {

    public Transform Parent;
    public GameObject RatingLetter;
    public GameObject RatingLevel;
    public GameObject TutorialObject;
    public LoadingBar ProgressBar;

    private TutorialController tutorialController;
    private GameObject letterRatingObj,gameRatingObj;
    private LoadingBar loadingBar;

    private static PopUpsManager _instance;
    public static PopUpsManager Instance {
      get {
        if (_instance == null) {
          _instance = GameObject.FindObjectOfType<PopUpsManager>();
        }
        return _instance;
      }
    }

    public void LoadRatingLetter() {
        letterRatingObj = Instantiate(RatingLetter, Parent, false);
      letterRatingObj.GetComponent<LetterRatingPopUp>();
    }

    public LetterRatingPopUp LoadRatingLetterInstance() {
      letterRatingObj = Instantiate(RatingLetter, Parent, false);
      return letterRatingObj.GetComponent<LetterRatingPopUp>();
    }

    public void LoadRatingLevel() {
      GameObject rating = Instantiate(RatingLevel, Parent, false);
    }

    public TutorialController LoadTutorial(Transform parentObj) {
      if (tutorialController == null) {
        GameObject tutorialObject = Instantiate(TutorialObject, parentObj, false);
        tutorialController = tutorialObject.GetComponent<TutorialController>();
        if (tutorialController != null) {
          return tutorialController;
        } else {
          return null;
        }
      }
      return tutorialController;
    }

    public LoadingBar LoadLoadingBar(Transform parentObj = null) {
      if (loadingBar == null) {
        GameObject loadingBarObj = Instantiate(ProgressBar.gameObject, parentObj == null ? Parent : parentObj, false);
        loadingBar = loadingBarObj.GetComponent<LoadingBar>();
        if (loadingBar != null) {
          return loadingBar;
        } else {
          return null;
        }
      }
      return loadingBar;
    }

  }
}
