using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
namespace KendaGame.UI {
  public class StorePopUp : MonoBehaviour {
    public Image[] hats;
    public Image[] glasses;
    public Image[] mustache;

    private int coins;

    private void OnEnable() {
      coins = int.Parse(StorageHelper.ReadStorage(StorageHelper.CoinsAmount, "0"));
    }

    private void Start() {

      // Restore purchased items for the player character
      RestorePurchasedItems(hats, StorageHelper.Hat);
      RestorePurchasedItems(glasses, StorageHelper.Glasses);
      RestorePurchasedItems(mustache, StorageHelper.Mustache);
    }

    private void SaveCoins(int itemPrice) {
      SceneManager.Instance.IncrementCoins(itemPrice);
    }

    public void BuyHat(int hatIndex) {
      if (StorageHelper.ReadStorage(StorageHelper.Hat + "_" + hatIndex, "0") == "0") {//Check if he already owned it
        if (coins >= StoreItemPrices.Hat) {
          coins -= StoreItemPrices.Hat;
          hats[hatIndex].enabled = false;
          StorageHelper.WriteStorage(StorageHelper.Hat + "_" + hatIndex, "1");
          SpineAttachmentUI.instance.changeHats(hatIndex);
          SaveCoins(-StoreItemPrices.Hat);
          StorageHelper.WriteStorage(StorageHelper.UsedHat, $"{hatIndex}");
        }
      } else {
        SpineAttachmentUI.instance.changeHats(hatIndex);
        StorageHelper.WriteStorage(StorageHelper.UsedHat, $"{hatIndex}");
      }
    }

    public void BuyGlasses(int glassesIndex) {
      if (StorageHelper.ReadStorage(StorageHelper.Glasses + "_" + glassesIndex, "0") == "0") {
        if (coins >= StoreItemPrices.Glasses) {
          coins -= StoreItemPrices.Glasses;
          glasses[glassesIndex].enabled = false;
          StorageHelper.WriteStorage(StorageHelper.Glasses + "_" + glassesIndex, "1");
          SpineAttachmentUI.instance.changeGlasses(glassesIndex);
          SaveCoins(-StoreItemPrices.Glasses);
          StorageHelper.WriteStorage(StorageHelper.UsedGlasses, $"{glassesIndex}");
        }
      } else {
        SpineAttachmentUI.instance.changeGlasses(glassesIndex);
        StorageHelper.WriteStorage(StorageHelper.UsedGlasses, $"{glassesIndex}");
      }
    }

    public void BuyMustache(int mustacheIndex) {
      if (StorageHelper.ReadStorage(StorageHelper.Mustache + "_" + mustacheIndex, "0") == "0") {
        if (coins >= StoreItemPrices.Mustache) {
          coins -= StoreItemPrices.Mustache;
          mustache[mustacheIndex].enabled = false;
          StorageHelper.WriteStorage(StorageHelper.Mustache + "_" + mustacheIndex, "1");
          SpineAttachmentUI.instance.changeMustache(mustacheIndex);
          SaveCoins(-StoreItemPrices.Mustache);
          StorageHelper.WriteStorage(StorageHelper.UsedMustache, $"{mustacheIndex}");
        }
      } else {
        SpineAttachmentUI.instance.changeMustache(mustacheIndex);
        StorageHelper.WriteStorage(StorageHelper.UsedMustache, $"{mustacheIndex}");
      }
    }

    public void ShowPopUp() {
      if (gameObject.activeInHierarchy) {
      //  transform.DOScale(0, 0.5f).SetEase(Ease.Flash).OnComplete(() => {
          gameObject.SetActive(false);
      //  });
      } else {
        gameObject.SetActive(true);
     //   transform.DOScale(1, 0.5f);
      }
    }

    private void RestorePurchasedItems(Image[] items, string itemType) {
      for (int i = 0; i < items.Length; i++) {
        if (StorageHelper.ReadStorage(itemType + "_" + i, "0") == "1") {
          items[i].enabled = false;
        }
      }
    }
  }
}