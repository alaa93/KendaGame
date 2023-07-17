using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using KendaGame.Helpers;
using KendaGame.Navigation;
using TMPro;
public class ShopItem : MonoBehaviour
{
    [Header("Images")]
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    GameObject itemLock;
    [Header("Buttons")]
    [SerializeField]
    UIButtonAnimator buyBtn;
    [SerializeField]
    UIButtonAnimator applyBtn;
    [Header("Price")]
    [SerializeField]
    GameObject itemPrice;
    [SerializeField]
    private int price;
    

    private static event UnityAction ItemBought;

    private void Awake() {
        buyBtn.AddClickListener(OnBuyBtnClicked);
        itemPrice.GetComponent<TextMeshProUGUI>().text = $"{price}";
        //initialize playerprefs value
        if(PlayerPrefs.GetInt(gameObject.name, -1) == -1){
            PlayerPrefs.SetInt(gameObject.name, 0);
        }
        SetLockAndApplyBtn();
    }
    private void SetLockAndApplyBtn(){
        if(PlayerPrefs.GetInt(gameObject.name) == 1){
            ApplyUnlockedConditions();
        }
        else{
            ApplyLockedConditions();
        }

    }
    private void OnBuyBtnClicked(){
        int coins = PlayerPrefs.GetInt("Coins");

        if(coins <= price){
            PlayerPrefs.SetInt(gameObject.name, 1);
            //decrement coins in the database
        }
        else{
            PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("message_generic.error", "message_generic.no_enough_coins_message");
        }
    }
    private void OnApplyBtnClicked(){
        // implement the logic of applying item to habbar
    }
    private void ApplyUnlockedConditions(){
        itemLock.SetActive(false);
        itemPrice.SetActive(false);
        applyBtn.gameObject.SetActive(true);
    }
    private void ApplyLockedConditions(){
        itemLock.SetActive(true);
        itemPrice.SetActive(true);
        applyBtn.gameObject.SetActive(false);
    }
    
}
