using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using KendaGame.Helpers;
using KendaGame.Tutorial;

namespace KendaGame.Games {
  public class RotateImagePart : MonoBehaviour {

    [SerializeField]
    float rotAmount = 15f;
    [SerializeField]
    Button btn;
    Vector3 targetRot;
    bool canRotate;
    TutorialController tutorialController;
    private void Awake() {
      targetRot = transform.rotation.eulerAngles;
      btn.onClick.AddListener(OnClicked);
    }

    public void SetRandomRotation(bool autoStart) {
      targetRot = new Vector3(targetRot.x, targetRot.y, Random.Range(45f, 300f));
      this.transform.DORotate(targetRot, 1f).SetEase(Ease.OutBack).OnComplete(() => {
        canRotate = autoStart;
      });
    }

    public void SetTutorialPos(TutorialController tutorialController) {
      this.tutorialController = tutorialController;
      this.tutorialController.PlayTutorial(transform, transform, true);
      canRotate = true;
    }

    public void OnClicked() {
      if (tutorialController !=null && tutorialController.IsTutorialPlaying()) {
        RotateImageGameController.Instance.OnTutorialCompleted(tutorialController);
        canRotate = true;
        OnClicked();
      } else if (canRotate) {
        canRotate = false;
        targetRot.z += rotAmount;
        AudioController.Instance.PlaySfxAtPoint(RotateImageGameController.Instance.GetRotateSfx());
        transform.DORotate(targetRot, 0.2f).SetEase(Ease.Linear).OnComplete(() => {
          Debug.Log("Mathf.Abs(targetRot.z) " + Mathf.Abs(targetRot.z));
          if (Mathf.Abs(targetRot.z) <= 10f || (Mathf.Abs(targetRot.z) >= 350f && Mathf.Abs(targetRot.z) < 380f)) {
            targetRot.z = 0f;
            transform.DORotate(targetRot, 0.2f);
            btn.interactable = false;
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
  }
}