using KendaGame.Helpers;
using KendaGame.Models;
using UnityEngine;

namespace KendaGame.UI {
  public class GenderPopUp : MonoBehaviour {
    [SerializeField]
    UIButtonAnimator maleBtn, femaleBtn;

    private void Start() {
      InitBtns();
    }

    private void InitBtns() {
      maleBtn.AddClickListener(() => {
        OnGenderSelected(0);
      }, true);
      femaleBtn.AddClickListener(() => {
        OnGenderSelected(1);
      }, true);
    }

    public void ShowPopUp() {
      gameObject.SetActive(true);
    }

    private void OnGenderSelected(int selectedGender) {
      Debug.Log("geneder selected: " + (selectedGender == 0 ? "Male" : "Female"));
      gameObject.SetActive(false);
    }
  }
}
