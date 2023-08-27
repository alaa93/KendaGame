using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class StorePopUp : MonoBehaviour {
    public GameObject[] hats;
    public GameObject[] glasses;
    public GameObject[] mustache;
    [SerializeField] Sprite purchaseBackgroundImage;
    private int coins;

    private void OnEnable() {
      SceneManager.Instance.SetCoinsObjectState(true);
      coins = int.Parse(StorageHelper.ReadStorage(StorageHelper.CoinsAmount, "0"));
    }

    private void OnDisable() {
      SceneManager.Instance.SetCoinsObjectState(false);
    }

    private void Start() {
      RestorePurchasedItems(hats, StorageHelper.Hat);
      RestorePurchasedItems(glasses, StorageHelper.Glasses);
      RestorePurchasedItems(mustache, StorageHelper.Mustache);
    }

    private void SaveCoins(int itemPrice) {
      SceneManager.Instance.IncrementCoins(itemPrice);
    }

    private void BuyItem(GameObject[] items, string itemType, int itemIndex, int itemPrice, System.Action<int> changeFunction, string usedItemType) {
      if (StorageHelper.ReadStorage(itemType + "_" + itemIndex, "0") == "0") {
        if (coins >= itemPrice) {
          coins -= itemPrice;
          items[itemIndex].SetActive(false);
          StorageHelper.WriteStorage(itemType + "_" + itemIndex, "1");
          changeFunction(itemIndex);
          SaveCoins(-itemPrice);
          StorageHelper.WriteStorage(usedItemType, $"{itemIndex}");
          items[itemIndex].GetComponentInParent<Image>().sprite = purchaseBackgroundImage;
        } else {
          PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp(ErrorMessages.InsufficientFunds, ErrorMessages.LetsPlayToPurchaseMore);
        }
      } else {
        changeFunction(itemIndex);
        StorageHelper.WriteStorage(usedItemType, $"{itemIndex}");
      }
    }

    public void BuyHat(int hatIndex) {
      BuyItem(hats, StorageHelper.Hat, hatIndex, StoreItemPrices.Hat, SpineAttachmentUI.instance.changeHats, StorageHelper.UsedHat);
    }

    public void BuyGlasses(int glassesIndex) {
      BuyItem(glasses, StorageHelper.Glasses, glassesIndex, StoreItemPrices.Glasses, SpineAttachmentUI.instance.changeGlasses, StorageHelper.UsedGlasses);
    }

    public void BuyMustache(int mustacheIndex) {
      BuyItem(mustache, StorageHelper.Mustache, mustacheIndex, StoreItemPrices.Mustache, SpineAttachmentUI.instance.changeMustache, StorageHelper.UsedMustache);
    }

    public void ShowPopUp() {
      if (gameObject.activeInHierarchy) {
        gameObject.SetActive(false);
      } else {
        gameObject.SetActive(true);
      }
    }

    private void RestorePurchasedItems(GameObject[] items, string itemType) {
      for (int i = 0; i < items.Length; i++) {
        if (StorageHelper.ReadStorage(itemType + "_" + i, "0") == "1") {
          items[i].SetActive(false);
          items[i].GetComponentInParent<Image>().sprite = purchaseBackgroundImage;
        }
      }
    }
  }
}
