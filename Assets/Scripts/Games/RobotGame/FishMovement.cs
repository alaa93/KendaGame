using TMPro;
using UnityEngine;

namespace KendaGame.Games
{
    public class FishMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float leftLimit = -5f;
        [SerializeField] private float rightLimit = 5f;

        [SerializeField] private float amplitude = 0.5f;  // The amplitude of the floating motion
        [SerializeField] private float frequency = 1f;   // The frequency of the floating motion
        [SerializeField] private bool applyRandomSpeed;
        [SerializeField] public bool isBubble;
        [SerializeField] public bool isCorrectBubble;
        string[] arabicAlphabet = new string[] {
    "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي"
};

        private bool movingRight = true;
        private Vector2 initialPosition;

        private SpriteRenderer spriteRenderer;
        private float startY;  // The initial Y-position of the sprite

        private void Start()
        {
            if (applyRandomSpeed)
            {
                moveSpeed = Random.Range(2.5f,6f);
            }
            if (isBubble)
            {
                this.GetComponentInChildren<TextMeshPro>().text = isCorrectBubble ? arabicAlphabet[0] : arabicAlphabet[Random.Range(1, arabicAlphabet.Length - 1)];
            }
            // Store the initial position of the fish
            initialPosition = transform.position;

            // Get the sprite renderer component
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Calculate the vertical position range of the camera's viewport
            float viewportHeight = Camera.main.orthographicSize * 2f;
            float minY = Camera.main.transform.position.y - (viewportHeight / 2f);
            float maxY = Camera.main.transform.position.y + (viewportHeight / 2f);

            // Generate a random Y-position within the viewport height range
            float randomY = Random.Range(minY, maxY);

            // Clamp the random Y-position to be within the viewport height range
            startY = Mathf.Clamp(randomY, minY, maxY);

            // Set the initial position of the fish at the random Y-position and the left limit
            transform.position = new Vector2(transform.position.x, startY);

        }

        private void Update()
        {
            // Calculate the horizontal movement direction based on whether the fish is moving left or right
            float moveDirection = movingRight ? 1f : -1f;

            // Combine the horizontal movement direction into a vector
            Vector2 moveVector = new Vector2(moveDirection, 0f);

            // Move the fish by the combined direction vector
            transform.Translate(moveVector * moveSpeed * Time.deltaTime);

            // Check if the fish has reached the left or right limit
            if (transform.position.x >= rightLimit)
            {
                movingRight = false;

                // Flip the sprite renderer on the X-axis
                spriteRenderer.flipX = true;
                if (applyRandomSpeed)
                {
                    moveSpeed = Random.Range(2.5f, 6f);
                }
            }
            else if (transform.position.x <= leftLimit)
            {
                movingRight = true;

                // Flip the sprite renderer on the X-axis
                spriteRenderer.flipX = false;
                if (applyRandomSpeed)
                {
                    moveSpeed = Random.Range(2.5f, 6f);
                }
            }

            // Calculate the new Y-position of the sprite using a cosine random value
            float newY = startY + amplitude * Mathf.Cos(frequency * Time.time);

            // Update the sprite's position with the new Y-position
            transform.position = new Vector2(transform.position.x, newY);
        }



        private void OnBecameInvisible()
        {
            // Reset the position of the fish when it goes off-screen
            transform.position = initialPosition;
        }
    }

}