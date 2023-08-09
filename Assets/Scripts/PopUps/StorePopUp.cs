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
      for (int i = 0; i < hats.Length; i++) {
        if (StorageHelper.ReadStorage(StorageHelper.Hat + "_" + i, "0") == "1") {
          hats[i].enabled = false;
        }
      }

      for (int i = 0; i < glasses.Length; i++) {
        if (StorageHelper.ReadStorage(StorageHelper.Glasses + "_" + i, "0") == "1") {
          glasses[i].enabled = false;
        }
      }

      for (int i = 0; i < mustache.Length; i++) {
        if (StorageHelper.ReadStorage(StorageHelper.Mustache + "_" + i, "0") == "1") {
          mustache[i].enabled = false;
        }
      }
    }

    private void SaveCoins(int itemPrice) {
      SceneManager.Instance.IncrementCoins(coins);
    }

    public void BuyHat(int hatIndex) {
      int hatCost = 100;
      if (StorageHelper.ReadStorage(StorageHelper.Hat + "_" + hatIndex, "0") == "0") {//Check if he already owned it
        if (coins >= hatCost) {
          coins -= hatCost;
          hats[hatIndex].enabled = false;
          StorageHelper.WriteStorage(StorageHelper.Hat + "_" + hatIndex, "1");
          SpineAttachmentUI.instance.changeHats(hatIndex);
          SaveCoins(-100);
        }
      } else {
        SpineAttachmentUI.instance.changeHats(hatIndex);
      }
      StorageHelper.WriteStorage(StorageHelper.UsedHat, $"{hatIndex}");
    }

    public void BuyGlasses(int glassesIndex) {
      int glassesCost = 100;
      if (StorageHelper.ReadStorage(StorageHelper.Glasses + "_" + glassesIndex, "0") == "0") {
        if (coins >= glassesCost) {
          coins -= glassesCost;
          glasses[glassesIndex].enabled = false;
          StorageHelper.WriteStorage(StorageHelper.Glasses + "_" + glassesIndex, "1");
          SpineAttachmentUI.instance.changeGlasses(glassesIndex);
          SaveCoins(-100);
        }
      } else {
        SpineAttachmentUI.instance.changeGlasses(glassesIndex);
      }
      StorageHelper.WriteStorage(StorageHelper.UsedGlasses, $"{glassesIndex}");
    }

    public void BuyMustache(int mustacheIndex) {
      int mustacheCost = 100;
      if (StorageHelper.ReadStorage(StorageHelper.Mustache + "_" + mustacheIndex, "0") == "0") {
        if (coins >= mustacheCost) {
          coins -= mustacheCost;
          mustache[mustacheIndex].enabled = false;
          StorageHelper.WriteStorage(StorageHelper.Mustache + "_" + mustacheIndex, "1");
          SpineAttachmentUI.instance.changeMustache(mustacheIndex);
          SaveCoins(-100);
        }
      } else {
        SpineAttachmentUI.instance.changeMustache(mustacheIndex);
      }
      StorageHelper.WriteStorage(StorageHelper.UsedMustache, $"{mustacheIndex}");
    }

    public void ShowPopUp() {
      if (gameObject.activeInHierarchy) {
        transform.DOScale(0, 0.5f).SetEase(Ease.Flash).OnComplete(() => {
          gameObject.SetActive(false);
        });
      } else {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.5f);
      }
    }
  }
}