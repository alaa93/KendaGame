using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEffect : MonoBehaviour, IEndDragHandler {

  public ScrollRect map;

  public Sprite[] MapBackGrounds;
  public Image BackGround;

  public AudioSource MapMusic;
  public AudioClip [] MapMusics;

  public GameObject[] MapParticles;

  private void Start() {
    BackGround.sprite = MapBackGrounds[0];
    MapMusic.clip = MapMusics[0];
    MapMusic.Play();
   
    MapParticles[0].SetActive(true);
  }

  public void OnEndDrag(PointerEventData data) {
    float startPosition = map.horizontalNormalizedPosition;
    if (startPosition > 0.75f) {
      StartCoroutine(Move(startPosition, 1, 0.5f,0));
    } else if (0.25f <= map.horizontalNormalizedPosition && map.horizontalNormalizedPosition < 0.75f) {
      StartCoroutine(Move(startPosition, 0.5f, 0.5f,1));
    } else if (map.horizontalNormalizedPosition < 0.25f) {
      StartCoroutine(Move(startPosition, 0, 0.5f,2));
    }
  }
  IEnumerator Move(float startPosition, float EndPosition, float duration,int mapNumber) {

    BackGround.sprite = MapBackGrounds[mapNumber];
    MapMusic.clip = MapMusics[mapNumber];
    MapMusic.Play();

    for (int i = 0; i < MapParticles.Length; i++) {
      MapParticles[i].SetActive(false);
    }
    MapParticles[mapNumber].SetActive(true);

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
