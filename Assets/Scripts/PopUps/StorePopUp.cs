using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class StorePopUp : MonoBehaviour {
    public GameObject[] hats;
    public GameObject[] glasses;
    public GameObject[] mustache;
    [SerializeField] Sprite activeItemBackground;
    [SerializeField] Sprite inactiveItemBackground;
    [SerializeField] Sprite priceImage;
    [SerializeField] NarrationSounds narrationSounds;
    [SerializeField]
    GameObject StoreMenu;


    private int coins;
    private int currentHatIndex = -1;
    private int currentGlassesIndex = -1;
    private int currentMustacheIndex = -1;

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
      currentHatIndex = int.Parse(StorageHelper.ReadStorage(StorageHelper.UsedHat, "-1"));
      currentGlassesIndex = int.Parse(StorageHelper.ReadStorage(StorageHelper.UsedGlasses, "-1"));
      currentMustacheIndex = int.Parse(StorageHelper.ReadStorage(StorageHelper.UsedMustache, "-1"));
      EquipItem(hats, currentHatIndex);
      EquipItem(glasses, currentGlassesIndex);
      EquipItem(mustache, currentMustacheIndex);
    }

    private void SaveCoins(int itemPrice) {
      SceneManager.Instance.IncrementCoins(itemPrice);
    }

    private void EquipItem(GameObject[] items, int itemIndex) {
      if (itemIndex >= 0 && itemIndex < items.Length) {
        items[itemIndex].GetComponentInParent<Image>().sprite = activeItemBackground;
        for (int i = 0; i < items.Length; i++) {
          if (i != itemIndex) {
            items[i].GetComponentsInParent<Image>()[0].sprite = inactiveItemBackground;
            items[i].GetComponent<Image>().sprite = priceImage;
          }
        }
      }
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
          EquipItem(items, itemIndex);
        } else {
          StoreMenu.transform.DOShakePosition(0.5f, strength: new Vector3(50, 50, 0), vibrato: 20, randomness: 180).OnComplete(() => {
            PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp(ErrorMessages.InsufficientFunds, ErrorMessages.LetsPlayToPurchaseMore);
          });
          AudioController.Instance.PlayNarrationAtPoint(narrationSounds.failPurchasingSound);
          SceneManager.Instance.CoinsAnimation();
        }
      } else {
        changeFunction(itemIndex);
        StorageHelper.WriteStorage(usedItemType, $"{itemIndex}");
        EquipItem(items, itemIndex);
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
          EquipItem(items, i);
        }
      }
    }

    public void ClosePopUp() {
      gameObject.SetActive(false);
    }

    private void HandlePurchaseAction(AudioClip purchaseSounds) {
      StoreMenu.transform.DOShakePosition(1.0f, strength: new Vector3(50, 50, 0), vibrato: 20, randomness: 180);
      AudioController.Instance.PlayNarrationAtPoint(purchaseSounds);
      SceneManager.Instance.CoinsAnimation();
    }
  }

  [System.Serializable]
  public class NarrationSounds {
    public AudioClip successfulPurchasingSound;
    public AudioClip failPurchasingSound;
  }
}
