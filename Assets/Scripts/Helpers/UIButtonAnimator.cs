using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;


namespace KendaGame.Helpers
{
    public class UIButtonAnimator : MonoBehaviour
    {
        public Vector3 startVal = Vector3.one * 0.8f;
        public Vector3 finalVal = Vector3.one;
        public bool isScaleAnimation = true;
        public bool prewarm = true;
        public bool autoPlay = false;
        public float autoPlayDelay = 0f;
        public float sfxPlayDelay = 0f;
        [Range(0.1f, 3f)]
        public float animationSpeed = 0.4f;
        [Space]
        [Space]
        public UnityEvent callBackFn;

        [Space]
        public UnityEvent onStartFn;

        public Ease ease = Ease.InOutBack;
        public bool isAnchoredUI = false;
        public AudioClip audioClip;

        private bool isAnimating = false;
        private IEnumerator playSfxCoroutine;

        private void OnEnable()
        {
            isAnimating = false;

            if (prewarm)
            {
                if (isScaleAnimation)
                    this.transform.localScale = startVal;
                else
                    this.transform.localPosition = startVal;
            }
                

           // if (isScaleAnimation)
             //   this.transform.localScale = finalVal;
           // else
               // this.transform.localPosition = finalVal;
        }

        private void Start()
        {
            if (autoPlay)
                StartCoroutine(PlayAnimOnly(autoPlayDelay));
        }

    public virtual void OnClick() {
      if (isAnimating)
        return;

      isAnimating = true;

      onStartFn?.Invoke();

      if (isScaleAnimation)
        this.transform.DOScale(startVal, animationSpeed).SetEase(Ease.OutBack).OnComplete(() => {
          this.transform.DOScale(finalVal, animationSpeed).SetEase(Ease.OutBack).OnComplete(() => {
          });
        });
      else
        this.transform.DOLocalMove(startVal, animationSpeed).SetEase(Ease.OutBack).OnComplete(() => {
          this.transform.DOLocalMove(finalVal, animationSpeed).SetEase(Ease.OutBack);
        });

      PlaySfx(audioClip, sfxPlayDelay);
      StartCoroutine(OnClickProcess(animationSpeed + 0.2f));
    }

        private IEnumerator PlayAnimOnly(float delay)
        {

            yield return new WaitForSeconds(delay);

            isAnimating = true;

            onStartFn?.Invoke();

            if (isScaleAnimation)
                this.transform.DOScale(startVal, 0.1f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.transform.DOScale(finalVal, animationSpeed).SetEase(ease);
                });
            else
                this.transform.DOLocalMove(startVal, 0.1f).SetEase(Ease.InBack).OnComplete(() =>
                {

                    this.transform.DOLocalMove(finalVal, animationSpeed).SetEase(ease);

                });

            yield return new WaitForSeconds(animationSpeed);

            isAnimating = false;
        }
        public virtual IEnumerator OnClickProcess(float timer)
        {
            yield return new WaitForSeconds(timer);
            callBackFn?.Invoke();
            isAnimating = false;
        }

    public void PlaySfx(AudioClip audioClip, float delay = 0f) {
      if (AudioController.Instance && AudioController.Instance.GetSfxState()) {
        if (audioClip) {
          if (playSfxCoroutine != null)
            StopCoroutine(playSfxCoroutine);
          playSfxCoroutine = PlaySfxProcess(audioClip, delay);
          StartCoroutine(playSfxCoroutine);
        } else {
          AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetClickableSfx());
        }

      }
    }

        private IEnumerator PlaySfxProcess(AudioClip audioClip,float delay) 
        {
            yield return new WaitForSeconds(delay);
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, 1f);
        }

        public void SetClickState(bool clickState)
        {
            this.GetComponent<Collider2D>().enabled = clickState;
        }  
    }
}