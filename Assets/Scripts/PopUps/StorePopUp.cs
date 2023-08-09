using KendaGame.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class StorePopUp : MonoBehaviour {
  public Image[] hats;
  public Image[] glasses;
  public Image[] mustache;

  private int coins;
   
  private void OnEnable () 
  {
    coins=int.Parse(StorageHelper.ReadStorage(StorageHelper.CoinsAmount,"0"));
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

  private void SaveCoins() {
    StorageHelper.WriteStorage(StorageHelper.CoinsAmount, $"{coins}");
  }

  public void BuyHat(int hatIndex) {
    int hatCost = 100;
    if (StorageHelper.ReadStorage(StorageHelper.Hat + "_" + hatIndex, "0") == "0") {//Check if he already owned it
      if (coins >= hatCost) {
        coins -= hatCost;
        hats[hatIndex].enabled = false;
        StorageHelper.WriteStorage(StorageHelper.Hat + "_" + hatIndex, "1"); // Mark the Hat as purchased in StorageHelper
        SpineAttachmentUI.instance.changeHats(hatIndex);
        SaveCoins();
      }
    } else {
      SpineAttachmentUI.instance.changeHats(hatIndex);
    }
    StorageHelper.WriteStorage(StorageHelper.UsedHat, $"{hatIndex}");
  }

  public void BuyGlasses(int glassesIndex) {
    int glassesCost = 100;
    if (StorageHelper.ReadStorage(StorageHelper.Glasses + "_" + glassesIndex, "0") == "0") {//Check if he already owned it
      if (coins >= glassesCost) {
        coins -= glassesCost;
        glasses[glassesIndex].enabled = false;
        StorageHelper.WriteStorage(StorageHelper.Glasses + "_" + glassesIndex, "1"); // Mark the glasses as purchased in StorageHelper
        SpineAttachmentUI.instance.changeGlasses(glassesIndex);
        SaveCoins() ;
      }
    }
    else {
      SpineAttachmentUI.instance.changeGlasses(glassesIndex);
    }
    StorageHelper.WriteStorage(StorageHelper.UsedGlasses, $"{glassesIndex}");
  }

  public void BuyMustache(int mustacheIndex) {
    int mustacheCost = 100;
    if (StorageHelper.ReadStorage(StorageHelper.Mustache + "_" + mustacheIndex, "0") == "0") { //Check if he already owned it
      if (coins >= mustacheCost) {
        coins -= mustacheCost;
        mustache[mustacheIndex].enabled = false;
        StorageHelper.WriteStorage(StorageHelper.Mustache + "_" + mustacheIndex, "1"); // Mark the musache as purchased in StorageHelper
        SpineAttachmentUI.instance.changeMustache(mustacheIndex);
        SaveCoins();
      }
    }
    else{
      SpineAttachmentUI.instance.changeMustache(mustacheIndex);
    }
    StorageHelper.WriteStorage(StorageHelper.UsedMustache,$"{mustacheIndex}");
  }
  public void ShowPopUp() {
    gameObject.SetActive(true);
  }
}
