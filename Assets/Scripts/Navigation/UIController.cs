using UnityEngine;
using UnityEngine.UI;
using System;
using KendaGame.Helpers;
using TMPro;

namespace KendaGame.Navigation {
  public class UIController : MonoBehaviour {
    [SerializeField]
    UIButtonAnimator backBtn;
    [SerializeField]
    GameObject coinsObject;
    [SerializeField]
    TextMeshProUGUI coinsText;
    int coinsAmount = 0;

    private void Awake() {
      InitUI();
    }

    private void InitUI() {
      InitBackBtn();
      coinsObject.SetActive(false);
      int.TryParse(StorageHelper.ReadStorage(StorageHelper.CoinsAmount), out int coins);
      coinsAmount = coins;
      coinsText.text = $"{coinsAmount}";
    }

    private void InitBackBtn() {
      backBtn.gameObject.SetActive(false);
      backBtn.AddClickListener(() => {
        SceneManager.Instance.OnBackClicked();
      });
    }

    public void SetBackBtnState(bool state) {
      backBtn.gameObject.SetActive(state);
    }

    public void SetCoinsObjectState(bool state) {
      coinsObject.gameObject.SetActive(state);
    }

    public void IncrementCoins(int addedAmount) {
      coinsAmount += addedAmount;
      coinsText.text = $"{coinsAmount}";
      StorageHelper.WriteStorage(StorageHelper.CoinsAmount, $"{coinsAmount}");
    }
  }
}
