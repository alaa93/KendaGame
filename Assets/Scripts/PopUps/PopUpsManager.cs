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
    private LoadingBar loadingBar;
    private GameRatingPopUp gameRating;
    private LetterRatingPopUp letterRating;

    private static PopUpsManager _instance;
    public static PopUpsManager Instance {
      get {
        if (_instance == null) {
          _instance = GameObject.FindObjectOfType<PopUpsManager>();
        }
        return _instance;
      }
    }

    public LetterRatingPopUp GetLetterRatingPopUp() {
      if (letterRating == null) {
        GameObject letterRatingObj = Instantiate(RatingLetter, Parent, false);
        letterRating = letterRatingObj.GetComponent<LetterRatingPopUp>();
        if (letterRating != null) {
          return letterRating;
        } else {
          return null;
        }
      }
      return letterRating;
    }

    public GameRatingPopUp GetGameRatingPopUp() {
      if (gameRating == null) {
        GameObject gameRatingObj = Instantiate(RatingLevel, Parent, false);
        gameRating = gameRatingObj.GetComponent<GameRatingPopUp>();
        if (gameRating != null) {
          return gameRating;
        } else {
          return null;
        }
      }
      return gameRating;
    }

    public bool IsLetterRatingActive() {
      return letterRating != null && letterRating.gameObject.activeInHierarchy;
    }

    public bool IsGameRatingActive() {
      return gameRating != null && gameRating.gameObject.activeInHierarchy;
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
