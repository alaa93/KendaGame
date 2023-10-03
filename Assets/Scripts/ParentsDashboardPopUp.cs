using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static KendaGame.Models.TracingModels;

public class ParentsDashboardPopUp : MonoBehaviour {

  [SerializeField]
  Image cureentLetterImage;
  [SerializeField]
  Image currentStarImage;
  [SerializeField]
  Image cureentLetterBackGround;
  [SerializeField]
  Sprite [] currentLettersBackGround;

  [SerializeField]
  Sprite[] starsImage;
  [SerializeField]
  Sprite[] lettersImage;

  [SerializeField]
  TextMeshProUGUI timeText;
  List<LetterProgress> lettersRating;
  [SerializeField]
  Slider writingSkillsScoreSlider;
  [SerializeField]
  Slider analysisAndFocusSlider;
  float writingSkillsScore = 0;
  float numberOfPlayedLetter = 0;
  float analysisAndFocus = 0;
 
  public void PreparingLettersRating() {
    HTTPRequestHandler.Instance.GetProfileInformationInterface((success, profileData) => {

      if (success) {
        gameObject.SetActive(true);
        lettersRating = profileData.data;
        timeText.text = profileData.last_seven_days_time.ToString();
        SetAnalysisAndWritingScore();
        ChangeCurrentLetterRating(0);
      }
    });
  }
  public void SetAnalysisAndWritingScore() {
    foreach (var letter in lettersRating) {
      if (letter != null && letter.letter_rating != 0) {
        writingSkillsScore += (letter.tracing_advanced_completed + letter.tracing_basic_completed) / 2;
        analysisAndFocus += (letter.game_1_completed + letter.game_2_completed) / 2;
        numberOfPlayedLetter++;
      }
    }
    writingSkillsScoreSlider.value = (writingSkillsScore / numberOfPlayedLetter) / 3;
    analysisAndFocusSlider.value = (analysisAndFocus / numberOfPlayedLetter) / 3;
  }

  public void ChangeCurrentLetterRating(int letterIndex) {
    int rating = (lettersRating[letterIndex].tracing_advanced_completed + lettersRating[letterIndex].tracing_basic_completed) / 2;
    if (rating >= 0) {
      currentStarImage.sprite = starsImage[Mathf.Clamp(rating, 0, starsImage.Length - 1)];
    }
    if (CheckIfLetterPlayed(letterIndex)) {
      cureentLetterBackGround.sprite = currentLettersBackGround[1];
    } else {
      cureentLetterBackGround.sprite = currentLettersBackGround[0];
    }
    cureentLetterImage.sprite = lettersImage[Mathf.Clamp(letterIndex, 0, lettersImage.Length - 1)];
  }

  public bool CheckIfLetterPlayed(int letterIndex) {
    return !(lettersRating[letterIndex].letter_rating == 0);
  }

  public void ShowPopUp() {
    if (!gameObject.activeInHierarchy) {
      PreparingLettersRating();

    } else {
      gameObject.SetActive(false);
    }
  }
}
