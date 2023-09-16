using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KendaGame.Helpers;
using System;

namespace KendaGame.Navigation {
  public class LoadingController : MonoBehaviour {

    [Header("Clouds")]
    [SerializeField]
    LoadingClouds portraitClouds;
    [SerializeField]
    AudioClip windSfx;
    [SerializeField]
    [Range(0.4f, 3f)]
    float animationDuration = 0.6f;
    bool cloudClosed = true;

    private void Start() {
      UpdateClouds();
    }

    private LoadingClouds GetLoadingClouds() {
      return portraitClouds;
    }

    private void UpdateClouds() {
      portraitClouds.SetCloudsState(false);
      GetLoadingClouds().SetCloudsState(true);
      GetLoadingClouds().inScreenPosition = new Vector3(GetLoadingClouds().leftCloud.transform.position.x,
      GetLoadingClouds().rightCloud.transform.position.x, GetLoadingClouds().upperCloud.transform.position.y);
    }

    public void OpenClouds(Action<bool> onComplete, bool animated = true) {
      if (cloudClosed) {
        GetLoadingClouds().upperCloud.transform.DOMoveY(GetTopScreenEdge().y+5, animated ? animationDuration : 0.0f);
        GetLoadingClouds().leftCloud.transform.DOMoveX(GetLeftScreenEdge().x-5, animated ? animationDuration : 0.0f);
        GetLoadingClouds().rightCloud.transform.DOMoveX(GetRightScreenEdge().x+5, animated ? animationDuration : 0.0f).OnComplete(() => {
          onComplete?.Invoke(animated);
        });
        AudioController.Instance.PlaySfxAtPoint(windSfx, animated ? 0.4f : 0f);
        cloudClosed = false;
      }
    }

    public void CloseClouds(Action<bool> onComplete, bool animated = true) {
      if (!cloudClosed) {
        GetLoadingClouds().upperCloud.transform.DOMoveY(GetLoadingClouds().inScreenPosition.z, animated ? animationDuration : 0.0f);
        GetLoadingClouds().leftCloud.transform.DOMoveX(GetLoadingClouds().inScreenPosition.x, animated ? animationDuration : 0.0f);
        GetLoadingClouds().rightCloud.transform.DOMoveX(GetLoadingClouds().inScreenPosition.y, animated ? animationDuration : 0.0f).OnComplete(() => {
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

    [Serializable]
    class LoadingClouds {
      public Image upperCloud, leftCloud, rightCloud;
      public Vector3 inScreenPosition;
      public void SetCloudsState(bool state) {
        upperCloud.gameObject.SetActive(state);
        leftCloud.gameObject.SetActive(state);
        rightCloud.gameObject.SetActive(state);
      }
    }
  }
}