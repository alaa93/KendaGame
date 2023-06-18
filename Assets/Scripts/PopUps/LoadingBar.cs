using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class LoadingBar : MonoBehaviour {

    [SerializeField]
    Image fillImage;
    [SerializeField]
    Localize localize;

    public void ShowLoadingBar(string term) {
      if (localize != null) {
        localize.Term = !string.IsNullOrEmpty(term) ? term : "tracing.rating_in_progress";
      }
      gameObject.SetActive(true);
    }

    public void HideLoadingBar() {
      gameObject.SetActive(false);
      fillImage.fillAmount = 0f;
    }

    public void IncreaseLoadingBar(float amount) {
      fillImage.fillAmount += amount;
    }

    public void IncreaseloadingBarByAnim(float animDuration = 2f) {
      StartCoroutine(IncreaseBarByAnimation(animDuration));
    }

    private IEnumerator IncreaseBarByAnimation(float duration) {
      while (fillImage.fillAmount < 1) {
        yield return new WaitForSeconds(0.2f);
        fillImage.fillAmount += 0.1f;
      }
    }

  }
}
