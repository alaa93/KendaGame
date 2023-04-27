using UnityEngine;

namespace KendaGame.Navigation {
  public class TransitionController : MonoBehaviour {
    [SerializeField][Range(0.1f,3f)]
    float transitionSpeed;
    [SerializeField]
    GameObject leftClouds,rightClouds,upClouds,bottomClouds;

    public void SlideIn() {
      //TODO implement the slide in animation for clouds 
    }

    public void SlideOut() {
      //TODO implement the slide out animation for clouds 
    }
  }
}
