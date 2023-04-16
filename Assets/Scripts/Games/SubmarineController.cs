using KendaGame.Helpers;
using UnityEngine;

namespace KendaGame.Games {
  public class SubmarineController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float autoMoveSpeed = 5f;
    public float turnDelay = 3f;
    public float minRandomDistance = 1f;
    public float maxRandomDistance = 5f;
    public bool continuesMovement;
    public bool autoMove = false;

    private ScrollingBackground scrollingBackground;
    private Vector2 targetPosition;
    private float lastTurnTime;
    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    private bool startMovement;


    Vector2 mousePosition = Vector2.zero;
    Vector2 clampedPosition = Vector2.zero;

    private void Start() {
      mainCamera = Camera.main;
      cameraHeight = 2f * mainCamera.orthographicSize;
      cameraWidth = cameraHeight * mainCamera.aspect;
      GenerateRandomTargetPosition();
      scrollingBackground = FindObjectOfType<ScrollingBackground>();
      if (autoMove) {
        scrollingBackground.StartFollowingSubmarine();
      }
    }

    private void Update() {
      if (autoMove) {
        if (Time.time - lastTurnTime >= turnDelay) {
          lastTurnTime = Time.time;
          GenerateRandomTargetPosition();
        }
        Vector2 currentPosition = transform.position;
        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f) {
          GenerateRandomTargetPosition();
        }
        Vector2 direction = (targetPosition - currentPosition).normalized;
        Vector2 newPosition = currentPosition + direction * autoMoveSpeed * Time.deltaTime;
        Vector2 clampedPosition = ClampPositionToCamera(newPosition);
        transform.position = clampedPosition;
      } else {
        if (Input.GetMouseButtonDown(0)) {
          startMovement = true;
          mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
          clampedPosition = ClampPositionToCamera(mousePosition);
        }
        if (startMovement) {
          // Lerp the submarine towards the mouse position
          Vector2 newPosition = Vector2.Lerp(transform.position, clampedPosition, Time.deltaTime * moveSpeed);
          transform.position = newPosition;

          // Clamp the position of the submarine within the view port bounds
          float halfWidth = cameraWidth / 2f;
          float halfHeight = cameraHeight / 2f;
          float submarineHalfWidth = transform.localScale.x / 2f;
          float submarineHalfHeight = transform.localScale.y / 2f;
          float maxX = halfWidth - submarineHalfWidth;
          float maxY = halfHeight - submarineHalfHeight;
          float minX = -maxX;
          float minY = -maxY;
          float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
          float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
          transform.position = new Vector2(clampedX, clampedY);
          float distance = Vector2.Distance(transform.position, clampedPosition);
          if (distance <= 0.1f && !continuesMovement) {
            scrollingBackground.StopFollowingSubmarine();
            startMovement = false;
          } else {
            scrollingBackground.StartFollowingSubmarine();
          }
        }
      }
    }

    private void GenerateRandomTargetPosition() {
      float randomDistance = Random.Range(minRandomDistance, maxRandomDistance);
      float randomAngle = Random.Range(0f, Mathf.PI * 2f);
      Vector2 offset = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * randomDistance;
      targetPosition = ClampPositionToCamera((Vector2)transform.position + offset);
    }

    private Vector2 ClampPositionToCamera(Vector2 position) {
      float submarineHalfWidth = transform.localScale.x / 2f;
      float submarineHalfHeight = transform.localScale.y / 2f;

      float halfWidth = cameraWidth / 2f - submarineHalfWidth;
      float halfHeight = cameraHeight / 2f - submarineHalfHeight;

      float clampedX = Mathf.Clamp(position.x, -halfWidth, halfWidth);
      float clampedY = Mathf.Clamp(position.y, -halfHeight, halfHeight);
      return new Vector2(clampedX, clampedY);
    }
  }
}