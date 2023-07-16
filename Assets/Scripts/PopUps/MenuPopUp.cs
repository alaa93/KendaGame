using UnityEngine;
using KendaGame.Helpers;
using UnityEngine.Events;
using TMPro;
public class MenuPopUp : MonoBehaviour
{
    [SerializeField]
    UIButtonAnimator exitButton;
    [SerializeField]
    TextMeshProUGUI starsEarned;

    [SerializeField]
    TMP_InputField player_name;

    private static event UnityAction OnSaveBtnPressed;
    private void OnEnable() {
        OnSaveBtnPressed+=ChangeName;
    }

    private void OnDisable() {
        OnSaveBtnPressed-=ChangeName;
    }
    private void Start() {
        InitializeButtons();
        SetProgressBar();
        SetStarsEarned();
    }
    private void InitializeButtons(){
        exitButton.AddClickListener(OnExitButtonPressed);
    }
    public void ChangeName(){
        //is gonna be implemented after database is all set and done
        //name in the database = player_name.text
    }
    private void SetStarsEarned(){
        //retrieve stars earned from the database
        int query = 0;
        starsEarned.text = query.ToString();
    }
    private void SetProgressBar(){
        //is gonna be implemented after database implementation
    }
    private void OnExitButtonPressed(){
        gameObject.SetActive(false);
    }
    public void ShowPopUp(){
        gameObject.SetActive(true);
    }
}
