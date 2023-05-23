using UnityEngine;

namespace KendaGame.Games {
  public class StairsCharacter : MonoBehaviour {
    public Transform platformA;  // Reference to Platform A
    public Transform platformB;  // Reference to Platform B

    public AnimationCurve jumpCurve;  // Animation curve controlling the jump physics

    public float jumpDuration = 1f;  // Duration of the jump
    public float jumpHeight = 5f;    // Height of the jump

    public bool shouldJump = false;  // Flag to determine if the character should jump
    private float jumpTimer = 0f;     // Timer to track the progress of the jump

    private Rigidbody2D rb;

    private void Awake() {
      rb = GetComponent<Rigidbody2D>();
    }

    public void PerformJump(GameObject targetRock = null) {
      GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
      // Calculate the direction from Platform A to Platform B
      Vector2 direction = StairsLevelManager.Instance.GetNextRock().transform.position - StairsLevelManager.Instance.GetActiveRock().transform.position;
      direction.y += 3f;
      // Normalize the direction vector
      direction.Normalize();

      // Calculate the jump force based on the animation curve
      float normalizedTime = jumpTimer / jumpDuration;
      float jumpForce = jumpCurve.Evaluate(normalizedTime) * jumpHeight;

      // Apply the jump force in the calculated direction
      rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);

      // Start the jump timer
      jumpTimer = 0f;
    }

    private void FixedUpdate() {
      if (jumpTimer <= jumpDuration) {
        jumpTimer += Time.fixedDeltaTime;
      }
    }

    // This method can be called externally to trigger the jump
    public void SetJump(bool value) {
      shouldJump = value;
    }
  }
}