using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;

namespace KendaGame.UI {
  public class HomeScreenController : MonoBehaviour {

    [SerializeField]
    UIButtonAnimator playBtn;

    private void Awake() {
      playBtn.AddClickListener(OnPlayBtnClicked);
    }

    private void OnPlayBtnClicked() {
      SceneManager.Instance.MoveTo(SceneManager.Scenes.Map.ToString());
    }
  }
}
