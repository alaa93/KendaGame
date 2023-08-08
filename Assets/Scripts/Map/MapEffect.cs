using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEffect : MonoBehaviour, IDragHandler, IEndDragHandler {

  public ScrollRect map;

  public void OnDrag(PointerEventData data) {
    Debug.Log(map.horizontalNormalizedPosition);
  }

  public void OnEndDrag(PointerEventData data) {
    float startPosition = map.horizontalNormalizedPosition;
    if (startPosition > 0.75f) {
      StartCoroutine(Move(startPosition, 1, 0.5f));
    } else if (0.25f <= map.horizontalNormalizedPosition && map.horizontalNormalizedPosition < 0.75f) {
      StartCoroutine(Move(startPosition, 0.5f, 0.5f));
    } else if (map.horizontalNormalizedPosition < 0.25f) {
      StartCoroutine(Move(startPosition, 0, 0.5f));
    }
  }
  IEnumerator Move(float startPosition, float EndPosition, float duration) {
    float elapsedTime = 0;
    while (elapsedTime < duration) {
      float normalizedTime = elapsedTime / duration;
      map.horizontalNormalizedPosition = Mathf.Lerp(startPosition, EndPosition, normalizedTime);

      elapsedTime += Time.deltaTime;
      yield return null;
    }
    map.horizontalNormalizedPosition = EndPosition; // Ensure final value is accurate
  }
}
