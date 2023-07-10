using UnityEngine;

namespace KendaGame.Games {
  public class StairsGameCamera : MonoBehaviour {

    [SerializeField]
    Transform target;
    [SerializeField]
    public float smoothSpeed = 0.5f;
    public Vector3 offset = new Vector3(0f, 1f, -10);
    bool follow;
    private Camera cam;
    bool endingOffset;

    private void Awake() {
      cam = Camera.main;
    }

    public void StartFollowing(Transform target) {
      this.target = target;
      follow = true;
    }

    public void StopFollowing() {
      follow = false;
    }

    public void SetEndingOffset() {
      offset = new Vector3(3f, 3f, -10f);
      endingOffset = true;
    }

    private void LateUpdate() {
      if (!follow) {
        return;
      }
      if (cam != null && target != null) {
        Vector3 targetViewportPos = cam.WorldToViewportPoint(target.position);

        // Check if the target is nearly out of the viewport view
        if (targetViewportPos.x < 0.3f || targetViewportPos.x > 0.6f ||
            targetViewportPos.y < 0.3f || targetViewportPos.y > 0.7f || endingOffset) {
          // Calculate the desired camera position based on the target and offset
          Vector3 desiredPos = target.position + offset;

          //Move the camera towards the desired position
          Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
          transform.position = smoothedPos;
        }
      }
    }

  }
}
