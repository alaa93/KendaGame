using KendaGame.Helpers;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using KendaGame.Navigation;


namespace KendaGame.UI {
  public class MapEffect : MonoBehaviour, IEndDragHandler {
    

    public ScrollRect map;
    public Sprite[] MapBackGrounds;
    public Image BackGround;
    public AudioSource MapMusic;
    public AudioClip[] MapMusics;
    public GameObject[] MapParticles;

    private float map1Offset = 0.95f;
    private float map2Offset = 0.45f;
    private float moveDuration = 0.5f;
    private Coroutine moveCoroutine;
    private float[] targetMapPositions = { 1f, 0.5f, 0f };
    Vector2 dragStartPos;
    [SerializeField] NarrationSounds narrationSounds;


    int currentArea =0;
    [SerializeField] UIButtonAnimator area1Button;
    [SerializeField] UIButtonAnimator area2Button;
    [SerializeField] UIButtonAnimator area3Button;


    private void Start() {
      area1Button.AddClickListener(() => GoToArea(2));
      area2Button.AddClickListener(() => GoToArea(1));
      area3Button.AddClickListener(() => GoToArea(0));
      SetMapProperties(0);
      AudioController.Instance.PlayNarrationAtPoint(narrationSounds.mapLoadedClip);
      SceneManager.Instance.SetCoinsObjectState(false);
    }

    public void OnBeginDrag(PointerEventData eventData) {
      dragStartPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData data) {
      Vector2 dragEndPos = data.position;
      //Can we perform a meeting before accepting this code?
      Vector2 dragDirection = dragEndPos - dragStartPos;
      float startPosition = map.horizontalNormalizedPosition;
      int targetMap = 0;
      if (dragDirection.x > 1000) {
        if (startPosition > map1Offset) {
          targetMap = 0;
        } else if (map2Offset <= startPosition && startPosition < map1Offset) {
          targetMap = 1;
        } else if (startPosition < map2Offset) {
          targetMap = 2;
        }
      } else if (dragDirection.x < 1000) {
        if (startPosition > 1-map2Offset) {
          targetMap = 0;
        } else if (1-map1Offset <= startPosition && startPosition < 1-map2Offset) {
          targetMap = 1;
        } else if (startPosition < 1-map1Offset) {
          targetMap = 2;
        }
      }
      GoToArea(targetMap);
    }


    IEnumerator Move(float startPosition, float EndPosition, float duration, int mapNumber) {

      SetMapProperties(mapNumber);

      float elapsedTime = 0;
      while (elapsedTime < duration) {
        float normalizedTime = elapsedTime / duration;
        map.horizontalNormalizedPosition = Mathf.Lerp(startPosition, EndPosition, normalizedTime);

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      map.horizontalNormalizedPosition = EndPosition; // Ensure final value is accurate
    }

    private void SetMapProperties(int mapNumber) {
      BackGround.sprite = MapBackGrounds[mapNumber];
      MapMusic.clip = MapMusics[mapNumber];
      MapMusic.Play();

      for (int i = 0; i < MapParticles.Length; i++) {
        MapParticles[i].SetActive(false);
      }
      MapParticles[mapNumber].SetActive(true);
    }

    public void GoToArea(int areaIndex) {
      if (moveCoroutine != null) {
        StopCoroutine(moveCoroutine);
      }

      if ((areaIndex == 1 && currentArea == 3) || (areaIndex == 3 && currentArea == 1)) {
        moveDuration = 1;
      }
      currentArea = areaIndex;
      float startPosition = map.horizontalNormalizedPosition;
      moveCoroutine = StartCoroutine(Move(startPosition, targetMapPositions[areaIndex], moveDuration, areaIndex));
    }
    [Serializable]
    public class NarrationSounds {
      public AudioClip mapLoadedClip;
    }
  }
}
