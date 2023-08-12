using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class MapEffect : MonoBehaviour, IEndDragHandler {

    public ScrollRect map;
    public Sprite[] MapBackGrounds;
    public Image BackGround;
    public AudioSource MapMusic;
    public AudioClip[] MapMusics;
    public GameObject[] MapParticles;

    private float map1Offset = 0.75f;
    private float map2Offset = 0.25f;
    private float moveDuration = 0.5f;
    private Coroutine moveCoroutine;
    private float[] targetMapPositions = { 1f, 0.5f, 0f };

    private void Start() {
      SetMapProperties(0);
    }

    public void OnEndDrag(PointerEventData data) {
      float startPosition = map.horizontalNormalizedPosition;
      int targetMap = 0;

      if (startPosition > map1Offset) {
        targetMap = 0;
      } else if (map2Offset <= startPosition && startPosition < map1Offset) {
        targetMap = 1;
      } else if (startPosition < map2Offset) {
        targetMap = 2;
      }

      if (moveCoroutine != null) {
        StopCoroutine(moveCoroutine);
      }

      moveCoroutine = StartCoroutine(Move(startPosition, targetMapPositions[targetMap], moveDuration, targetMap));
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

  }
}
