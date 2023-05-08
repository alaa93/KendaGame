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
    private bool isGrounded;
    [SerializeField]
    string letterId = "أ";
    Vector2 initialPos;

    private void Awake() {
      initialPos = this.transform.position;
    }
    private void Start() {
      rb = GetComponent<Rigidbody2D>();
      moveSpeed = Random.Range(5,8);
    }

    private void Update() {
      if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }

      if (isGrounded && canRun) {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        if (skeletonAnimation.AnimationName != "runinng") {
          skeletonAnimation.AnimationState.SetAnimation(0, "runinng", true);
        }
      }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Ground") {
        isGrounded = true;
      }
    }

    private void OnCollisionExit2D(Collision2D collision) {
      if (collision.gameObject.tag == "Ground") {
        isGrounded = false;
        skeletonAnimation.AnimationState.SetAnimation(0, "Jump", false);
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision != null && collision.tag == "TreasureBox") {
        Jump();
      }
    }

    private void Jump() {
      if (isGrounded) {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }
    }

    public bool CanRun() {
      return canRun;
    }

    public void SetRunState(bool run) {
      canRun = run;
    }

    public void ResetPos() {
      this.transform.position = initialPos;
      canRun = false;
      moveSpeed = Random.Range(5, 8);
      Invoke(nameof(EnableRunLocally), Random.Range(2, 4));
    }

    public void SetInitalPos(Vector3 pos) {
      initialPos = pos;
    }

    private void EnableRunLocally() {
      canRun = true;
    }
  }
}