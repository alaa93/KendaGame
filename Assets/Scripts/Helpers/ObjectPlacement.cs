using ScorpionSteps.Utils;
using UnityEngine;

namespace KendaGame.Helpers {

  public class ObjectPlacement : MonoBehaviour {

    public float positionOffet;

    void Awake() {
      SetScreenPosition();
    }

    private void SetScreenPosition() {
      this.transform.position = new Vector3(CameraUtils.GetLeftScreenEdge().x + positionOffet, this.transform.position.y, 0f);
    }
  }
}