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
    UIButtonAnimator settingsBtn;
    [SerializeField]
    UIButtonAnimator homeBtn;
    [SerializeField]
    GameObject coinsObject;
    [SerializeField]
    TextMeshProUGUI coinsText;
    int coinsAmount = 0;

    private void Awake() {
      InitUI();
    }

    private void InitUI() {
      InitBtn();
      coinsObject.SetActive(false);
      int.TryParse(StorageHelper.ReadStorage(StorageHelper.CoinsAmount), out int coins);
      coinsAmount = coins;
      coinsText.text = $"{coinsAmount}";
    }

    private void InitBtn() {
      backBtn.gameObject.SetActive(false);
      settingsBtn.gameObject.SetActive(false);
      homeBtn.gameObject.SetActive(false);
            backBtn.AddClickListener(() => {
        SceneManager.Instance.OnBackClicked();
      });
      settingsBtn.AddClickListener(() => {
        SceneManager.Instance.OnSettingsBtnClicked();
      });

            homeBtn.AddClickListener(() => {
                SceneManager.Instance.OnHomeClicked();
            });
        }

    public void SetBackBtnState(bool state) {
      backBtn.gameObject.SetActive(state);
    }

    public void SetSettingsBtnState(bool state) {
      settingsBtn.gameObject.SetActive(state);
    }
        
        public void SetHomeBtnState(bool state) {
      homeBtn.gameObject.SetActive(state);
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
