using KendaGame.Helpers;
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
      BoxesTreasureBox treasureBox = collision.GetComponent<BoxesTreasureBox>();
      if (treasureBox != null) {
        if (isDragging && letterId == LettersHelper.GetActiveLetter() && treasureBox.letterId == LettersHelper.GetActiveLetter() && !completed) {
          completed = true;
          GameObject.FindObjectOfType<BoxesLettersController>().OnCorrectLetter();
          treasureBox.OnTriggered(this.gameObject);
        } else {
          Jump();
        }
      }
    }

    private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e) {
      Invoke(nameof(ResetBoxAnim), 2f);
    }

    private void ResetBoxAnim() {
      targetbox.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "close", false);
    }

    public void Jump() {
      if (isGrounded) {
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

    public void SetHoldingAnimation(bool state) {
      isDragging = state;
      skeletonAnimation.AnimationState.SetAnimation(0, state ? "Fighting" : "Jump", state ? true : false);
      rb.gravityScale = 0.5f;
    }
  }
}