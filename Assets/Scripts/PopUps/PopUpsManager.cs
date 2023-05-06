using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KendaGame.Navigation {
  public class PopUpsManager : MonoBehaviour {

    public Transform Parent;
    public GameObject RatingLetter;
    public GameObject RatingLevel;

    private static PopUpsManager _instance;
    public static PopUpsManager Instance {
      get {
        if (_instance == null) {
          _instance = GameObject.FindObjectOfType<PopUpsManager>();
        }
        return _instance;
      }
    }

    private void Awake() {
      
    }

    public void LoadRatingLetter() {
      GameObject rating = Instantiate(RatingLetter, Parent,false);
    }

    public void LoadRatingLevel() {
      GameObject rating = Instantiate(RatingLevel, Parent, false);
    }
  }
}
