using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class LoadingBar : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    Image fillImage;

    bool inProgress;
    float fillSpeed = 0.1f;
    float fillDelay = 1f;

    Coroutine progressBarRoutine;

    public void ShowLoadingBar(string title, float fillSpeed = 0.1f, float fillDelay = 1f) {
      this.fillSpeed = fillSpeed;
      this.title.text = string.IsNullOrEmpty(title) ? "" : title;
      this.fillDelay = fillDelay;
      this.gameObject.SetActive(true);

      //if (progressBarRoutine == null) {
      //  progressBarRoutine = StartCoroutine(StartProgressBar());
      //} else {
      //  ResetLoadingBar();
      //  StopCoroutine(progressBarRoutine);
      //  progressBarRoutine = StartCoroutine(StartProgressBar());
      //}
    }

    public void HideLoadingBar() {
      inProgress = false;
      //if (progressBarRoutine != null) {
      //  StopCoroutine(progressBarRoutine);
      //}
      gameObject.SetActive(false);
    }

    private void ResetLoadingBar() {
      fillImage.fillAmount = 0f;
    }

    public void IncreaseLoadingBar(float amount) {
      fillImage.fillAmount += amount;
    }

    IEnumerator StartProgressBar() {
      while (inProgress) {
        fillImage.fillAmount += fillSpeed;
        yield return new WaitForSeconds(fillDelay);
      }
    }

  }
}
