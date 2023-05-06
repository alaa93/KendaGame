using UnityEngine;
using DG.Tweening;
using KendaGame.Helpers;

namespace KendaGame.Games {
  public class ShipSharkController : MonoBehaviour {
    [SerializeField]
    float releaseSpeed = 1.3f;
    [SerializeField]
    float endPoint = -15f;
    [SerializeField]
    Ease releaseEase = Ease.Linear;
    [SerializeField]
    ShipWheelController shipWheelController;

    private bool released;
    private Vector3 initialPosition;

    private void Awake() {
      initialPosition = this.transform.position;
    }
    private void OnMouseDown() {
      if (!released) {
        ReleaseShark();
      }
    }

    private void ReleaseShark() {
      released = true;
      this.transform.DOMoveX(endPoint, releaseSpeed).SetEase(releaseEase);
    }

    public void ResetShark() {
      DOTween.Pause(this.transform);
      DOTween.Kill(this.transform);
      released = false;
      this.transform.position = initialPosition;
      this.GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      ShipWheelBubble wheelBubble = collision.gameObject.GetComponent<ShipWheelBubble>();
      if (wheelBubble != null) {
        this.GetComponent<Collider2D>().enabled = false;
        shipWheelController.OnBubbleHit(wheelBubble, (gameCompleted) => {
          if (!gameCompleted) {
            ResetShark();
          }
        });
      }
    }
  }
}