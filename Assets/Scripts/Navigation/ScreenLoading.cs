using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace KendaGame.Navigation {
  public class ScreenLoading : MonoBehaviour {
    [SerializeField]
    Image upperCloud, leftCloud, rightCloud;
    float leftCloudInScreen, rightCloudInScreen, upperCloudInScreen;

    private void Start() {
      CacheInitailPosition();
    }
    private void OnEnable() {
      Invoke("OpenClouds", 2f);
      Invoke("CloseClouds", 4f);
    }
    private void CacheInitailPosition() {
      leftCloudInScreen = leftCloud.transform.position.x;
      rightCloudInScreen = rightCloud.transform.position.x;
      upperCloudInScreen = upperCloud.transform.position.y;
    }

    public void OpenClouds() {
      upperCloud.transform.DOMoveY(GetTopScreenEdge().y, 0.4f);
      leftCloud.transform.DOMoveX(GetLeftScreenEdge().x, 0.4f);
      rightCloud.transform.DOMoveX(GetRightScreenEdge().x, 0.4f);
    }
    public void CloseClouds() {
      upperCloud.transform.DOMoveY(upperCloudInScreen, 0.4f);
      leftCloud.transform.DOMoveX(leftCloudInScreen, 0.4f);
      rightCloud.transform.DOMoveX(rightCloudInScreen, 0.4f);
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