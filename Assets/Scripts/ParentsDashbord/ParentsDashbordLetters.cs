using KendaGame.Helpers;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParentsDashbordLetters : MonoBehaviour
{
  [SerializeField]
  UIButtonAnimator btn;
  [SerializeField]
  [Range(0, 28)]
  int id;
  [SerializeField]
  ParentsDashboardPopUp parentsDashboardPopUp;
  [SerializeField]
  Sprite [] letterBackGrounds;
  [SerializeField]
  Image letterBackGround;
  private void Start() {
    InitBtn();
  }

  private void InitBtn() {
    btn.AddClickListener(() => parentsDashboardPopUp.ChangeCurrentLetterRating(id));
    if (parentsDashboardPopUp.CheckIfLetterPlayed(id)) {
      letterBackGround.sprite = letterBackGrounds[1];
    } else {
      letterBackGround.sprite = letterBackGrounds[0];
    }
  }
}
