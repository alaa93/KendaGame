using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KendaGame.Games {
  public class SubmarineController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float turnDelay = 3f;
    public float minRandomDistance = 1f;
    public float maxRandomDistance = 5f;
    public bool manualMode = false;

    private Vector2 targetPosition;
    private float lastTurnTime;
    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;

    private void Start() {
      mainCamera = Camera.main;
      cameraHeight = 2f * mainCamera.orthographicSize;
      cameraWidth = cameraHeight * mainCamera.aspect;
      GenerateRandomTargetPosition();
    }

    private void Update() {
      if (!manualMode) {
        if (Time.time - lastTurnTime >= turnDelay) {
          lastTurnTime = Time.time;
          GenerateRandomTargetPosition();
        }

        Vector2 currentPosition = transform.position;

        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f) {
          GenerateRandomTargetPosition();
        }

        Vector2 direction = (targetPosition - currentPosition).normalized;
        Vector2 newPosition = currentPosition + direction * moveSpeed * Time.deltaTime;
        Vector2 clampedPosition = ClampPositionToCamera(newPosition);
        transform.position = clampedPosition;
      } else {
        if (Input.GetMouseButton(0)) {
          Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
          Vector2 clampedPosition = ClampPositionToCamera(mousePosition);
          transform.position = clampedPosition;
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
      float halfWidth = cameraWidth / 2f;
      float halfHeight = cameraHeight / 2f;
      float clampedX = Mathf.Clamp(position.x, -halfWidth, halfWidth);
      float clampedY = Mathf.Clamp(position.y, -halfHeight, halfHeight);
      return new Vector2(clampedX, clampedY);
    }
  }
}