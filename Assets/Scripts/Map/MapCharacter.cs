using System.Collections;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;
using KendaGame.Helpers;
using KendaGame.UI;

public class MapCharacter : MonoBehaviour {
  [SerializeField] private Vector3[] letterPosition;
  [SerializeField] private SkeletonGraphic character;
  [SerializeField] private MapEffect mapEffect;

  private void Start() {
    StartCoroutine(MoveCharacterRoutine());
  }

  private IEnumerator MoveCharacterRoutine() {
    int lastLetterOpend = LettersHelper.GetLastLetterOpendIndex();
    if(lastLetterOpend == -1) {
      lastLetterOpend = 0;
    }
    int lastActiveLetter = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted,"0"));
    DetermineArea(lastActiveLetter);

    transform.localPosition = letterPosition[lastActiveLetter];
    PlayCharacterAnimation("Happy-Hi");
    yield return new WaitForSeconds(1f);
    PlayCharacterAnimation("_Idle");

    if ((lastLetterOpend + 1) == 29) {
      ResetCharacterPosition();
    } else {
      MoveCharacterToNextLetter(lastLetterOpend);
    }
  }

  private void DetermineArea(int activeLetter) {
    if (activeLetter + 1 < 8 || activeLetter == 28) {
      mapEffect.GoToArea(0);
    } else if (activeLetter + 1 >= 8 && activeLetter + 1 < 18) {
      mapEffect.GoToArea(1);
    } else {
      mapEffect.GoToArea(2);
    }
  }

  private void MoveCharacterToNextLetter(int activeLetter) {
    if (activeLetter + 1 < letterPosition.Length) {
      transform.DOLocalMove(letterPosition[activeLetter + 1], 1f).OnComplete(()=> { PlayCharacterAnimation("Happy clap"); });
    }
  }

  private void PlayCharacterAnimation(string animationName) {
    character.AnimationState.SetAnimation(0, animationName, true);
  }

  private void ResetCharacterPosition() {
    transform.localPosition = letterPosition[0];
    LettersHelper.SetActiveLetter(0);
  }
}
