using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.Helpers {
  public class ScrollingBackground : MonoBehaviour {
    [SerializeField] private RawImage _img;
    [SerializeField] private float _scrollSpeed = 1f;
    [SerializeField] private Transform _submarine;
    [SerializeField] private float edgeInnerMargin;
    private bool _followSubmarine = false;

    private Camera _mainCamera;
    private float _cameraHeight;
    private float _cameraWidth;

    private void Start() {
      _mainCamera = Camera.main;
      _cameraHeight = 2f * _mainCamera.orthographicSize;
      _cameraWidth = _cameraHeight * _mainCamera.aspect;
    }

    void Update() {
      if (_followSubmarine) {
        Vector2 scrollDirection = new Vector2(_submarine.position.x / 2, _submarine.position.y / 2);
        if (IsSubmarineAtEdge()) {
          scrollDirection *= 0.5f;
          _img.uvRect = new Rect(_img.uvRect.position + scrollDirection * _scrollSpeed * Time.deltaTime, _img.uvRect.size);
        }
      }
    }

    public void StartFollowingSubmarine() {
      _followSubmarine = true;
    }

    public void StopFollowingSubmarine() {
      _followSubmarine = false;
    }

    private bool IsSubmarineAtEdge() {
      Vector3 submarinePosition = _submarine.position;
      float halfWidth = _cameraWidth / 2f;
      float halfHeight = _cameraHeight / 2f;
      return submarinePosition.x < -halfWidth + edgeInnerMargin || submarinePosition.x > halfWidth - edgeInnerMargin || submarinePosition.y < -halfHeight + edgeInnerMargin || submarinePosition.y > halfHeight - edgeInnerMargin;
    }
  }
}