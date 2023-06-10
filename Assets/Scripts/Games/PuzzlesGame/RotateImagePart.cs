using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using KendaGame.Helpers;
using KendaGame.Tutorial;
using UnityEngine.EventSystems;

namespace KendaGame.Games {
  public class RotateImagePart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField]
    float rotAmount = 15f;
    [SerializeField]
    Button btn;
    Vector3 targetRot;
    bool canRotate = true;
    TutorialController tutorialController;
    private Vector3 lastMousePosition;
    public float rotationSpeed = 1f;
    bool mouseDown;

    private void Awake() {
      targetRot = transform.rotation.eulerAngles;
      //btn.onClick.AddListener(OnClicked);
    }

    public void SetRandomRotation(bool autoStart) {
      targetRot = new Vector3(targetRot.x, targetRot.y, Random.Range(45f, 300f));
      this.transform.DORotate(targetRot, 1f).SetEase(Ease.OutBack).OnComplete(() => {
        canRotate = autoStart;
      });
    }

    public void SetTutorialPos(TutorialController tutorialController) {
      this.tutorialController = tutorialController;
      this.tutorialController.PlayTutorial(transform, transform, swipeTutorial: true);
      canRotate = true;
    }

    public void OnClicked() {
      if (tutorialController != null && !RotateImageGameController.Instance.IsTutorialPlayed()) {
        RotateImageGameController.Instance.SetTutorialPlayed();
        RotateImageGameController.Instance.OnTutorialCompleted();
        canRotate = true;
        OnClicked();
      } else if (canRotate) {
        canRotate = false;
        targetRot.z += rotAmount;
        AudioController.Instance.PlaySfxAtPoint(RotateImageGameController.Instance.GetRotateSfx());
        transform.DORotate(targetRot, 0.2f).SetEase(Ease.Linear).OnComplete(() => {
          if (Mathf.Abs(targetRot.z) <= 10f || (Mathf.Abs(targetRot.z) >= 350f && Mathf.Abs(targetRot.z) < 380f)) {
            targetRot.z = 0f;
            transform.DORotate(targetRot, 0.2f);
            RotateImageGameController.Instance.OnCorrect();
          } else {
            canRotate = true;
          }
        });
      }
    }

    public void SetClickState(bool state) {
      canRotate = state;
    }

    public void OnPointerDown(PointerEventData eventData) {
      Debug.Log("pointer down");
      mouseDown = true;
      RotateImageGameController.Instance.OnTutorialCompleted();
    }

    public void OnPointerUp(PointerEventData eventData) {
      mouseDown = false;
    }

    private void Update() {
      if (mouseDown && canRotate) {
        Vector2 currentMousePosition = Input.mousePosition;

        float movementDirection = currentMousePosition.x - lastMousePosition.x;
        float movementDirectionY = currentMousePosition.y - lastMousePosition.y;

        float rotation = movementDirection * rotationSpeed;
        float rotationY = movementDirectionY * rotationSpeed;

        if (rotation != 0) {
          transform.Rotate(0f, 0f, -rotation);
        }

        if (rotationY != 0) {
          transform.Rotate(0f, 0f, rotationY);
        }
        lastMousePosition = currentMousePosition;

        if (Mathf.Abs(transform.rotation.eulerAngles.z) <= 10f || (Mathf.Abs(transform.eulerAngles.z) >= 350f && Mathf.Abs(transform.eulerAngles.z) < 380f)) {
          mouseDown = false;
          canRotate = false;
          targetRot.z = 0f;
          transform.DORotate(targetRot, 0.2f);
          RotateImageGameController.Instance.OnCorrect();
        }
      }
    }
  }
}