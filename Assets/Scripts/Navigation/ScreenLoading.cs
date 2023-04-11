using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KendaGame.Helpers;
using System;

namespace KendaGame.Navigation {
  public class ScreenLoading : MonoBehaviour {
    [SerializeField]
    Image upperCloud, leftCloud, rightCloud;
    [SerializeField]
    AudioClip windSfx;
    [SerializeField][Range(0.4f,3f)]
    float animationDuration = 0.6f;

    float leftCloudInScreen, rightCloudInScreen, upperCloudInScreen;
    bool cloudClosed = true;

    private void Start() {
      CacheInitailPosition();
    }

    private void CacheInitailPosition() {
      leftCloudInScreen = leftCloud.transform.position.x;
      rightCloudInScreen = rightCloud.transform.position.x;
      upperCloudInScreen = upperCloud.transform.position.y;
    }

    public void OpenClouds(Action<bool> onComplete, bool animated = true) {
      if (cloudClosed) {
        upperCloud.transform.DOMoveY(GetTopScreenEdge().y, animated ? animationDuration : 0.0f);
        leftCloud.transform.DOMoveX(GetLeftScreenEdge().x, animated ? animationDuration : 0.0f);
        rightCloud.transform.DOMoveX(GetRightScreenEdge().x, animated ? animationDuration : 0.0f).OnComplete(() => {
          onComplete?.Invoke(animated);
        });
        AudioController.Instance.PlaySfxAtPoint(windSfx, animated ? 0.4f : 0f);
        cloudClosed = false;
      }
    }

    public void CloseClouds(Action<bool> onComplete, bool animated = true) {
      if (!cloudClosed) {
        upperCloud.transform.DOMoveY(upperCloudInScreen, animated ? animationDuration : 0.0f);
        leftCloud.transform.DOMoveX(leftCloudInScreen, animated ? animationDuration : 0.0f);
        rightCloud.transform.DOMoveX(rightCloudInScreen, animated ? animationDuration : 0.0f).OnComplete(()=> {
          onComplete?.Invoke(animated);
        });
        AudioController.Instance.PlaySfxAtPoint(windSfx, animated ? 0.4f : 0f);
        cloudClosed = true;
      }
    }

    private Vector3 GetRightScreenEdge() {
      Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0));
      mPos.z = 0;
      return mPos;
    }
    private Vector3 GetLeftScreenEdge() {
      Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f, 0));
      mPos.z = 0;
      return mPos;
    }
    private Vector3 GetTopScreenEdge() {
      Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0));
      mPos.z = 0;
      return mPos;
    }
  }
}