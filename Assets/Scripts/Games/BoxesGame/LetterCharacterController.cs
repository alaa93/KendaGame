using KendaGame.Helpers;
using KendaGame.Navigation;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class LetterCharacterController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [SerializeField]
    SkeletonAnimation skeletonAnimation;
    private bool canRun;
    private Rigidbody2D rb;
    public bool isGrounded;
    [SerializeField]
    string letterId = "أ";
    Vector2 initialPos;
    int runningDirection = -1;
    float spriteflipDir = 1f;
    bool isDragging = false;
    GameObject targetbox;
    bool completed;
    bool freeze=true;

    private void Awake() {
      initialPos = this.transform.position;
      spriteflipDir = this.transform.localScale.x;
    }

    private void Start() {
      rb = GetComponent<Rigidbody2D>();
      moveSpeed = Random.Range(3f, 5.5f);
    }

    private void Update() {
      if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }

      if (isGrounded && canRun) {
        rb.velocity = new Vector2(runningDirection == 1 ? moveSpeed : -moveSpeed, rb.velocity.y);
        if (skeletonAnimation.AnimationName != "runinng" && !isDragging) {
          skeletonAnimation.AnimationState.SetAnimation(0, "runinng", true);
        }
      }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "TreasureBox") {
        isGrounded = true;
        rb.gravityScale = 1f;
      }
    }

    private void OnCollisionExit2D(Collision2D collision) {
      if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "TreasureBox") {
        isGrounded = false;
        if (!isDragging) {
          skeletonAnimation.AnimationState.SetAnimation(0, "Jump", false);
        }
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (BoxesLettersController.Instance.gameCompleted) {
        return;
      }
      BoxesTreasureBoxTrigger boxTrigger = collision.GetComponent<BoxesTreasureBoxTrigger>();
      BoxesTreasureBox treasureBox = collision.GetComponentInParent<BoxesTreasureBox>();
      if (boxTrigger != null) {
        if (isDragging) {
          if (!completed) {
            if (letterId == LettersHelper.GetActiveLetter()) {
              if (boxTrigger.solved) {
                return;
              }
              boxTrigger.solved = true;
              completed = true;
              if (treasureBox != null) {
                treasureBox.OnSolved(this.gameObject);
                BoxesLettersController.Instance.OnCorrectLetter(treasureBox);
              }
            } else {
              Jump(true);
              GetComponent<LetterCharacterDraggable>().mouseDown = false;
              SetHoldingAnimation(false);
              SetRunState(true);
              BoxesLettersController.Instance.OnInCorrectLetter();
            }
          }
        }
      }

      if (treasureBox != null) {
        if (!isDragging) {
          Jump();
        }
      }

      if (collision.gameObject.tag == "Tutorial") {
        CheckPlayTutorial();
      }
    }

    private void CheckPlayTutorial() {
      BoxesLettersController.Instance.CheckPlayTutorial(this.transform, letterId);
    }

    public void Jump(bool forceJump = false) {
      if (isGrounded || forceJump) {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }
    }

    private void EnableRunLocally() {
      canRun = true;
    }

    public bool CanRun() {
      return canRun;
    }

    public void SetRunState(bool run) {
      canRun = run;
    }

    public void ReverseDirection(int direction) {
      runningDirection = direction;
      this.transform.localScale = new Vector3(direction == 1 ? -spriteflipDir : spriteflipDir, transform.localScale.y, transform.localScale.z);
      canRun = false;
      moveSpeed = Random.Range(3f, 5.5f);
      Invoke(nameof(EnableRunLocally), Random.Range(2, 4));
    }

    public void SetInitalPos(Vector3 pos) {
      initialPos = pos;
      this.transform.position = initialPos;
    }

    public void ResetPosition() {
      this.transform.position = initialPos;
    }

    public void SetHoldingAnimation(bool state) {
      isDragging = state;
      skeletonAnimation.AnimationState.SetAnimation(0, state ? "Fighting" : "Jump", state ? true : false);
      rb.gravityScale = 0.5f;
    }

    public void SetAnimation(bool state) {
      isDragging = false;
      skeletonAnimation.AnimationState.SetAnimation(0, state? "runinng" : "Idle", true);
    }

    public void SetDancingAnimation() {
      isDragging = false;
      skeletonAnimation.AnimationState.SetAnimation(0, "Dancing", true);
    }

    public void SetFreezeState(bool freeze) {
      this.freeze = freeze;
    }

    public bool IsFreezed() {
      return freeze;
    }

    public string GetLetterId() {
      return letterId;
    }
  }
}