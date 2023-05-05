using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using KendaGame.Helpers;

namespace KendaGame.Games {
    public class BubbleMovement : MonoBehaviour
    {
        public TextMeshPro letterText;
        public Vector2 startPosition;      // Starting position of the bubble
        public Vector2 endPosition;        // Ending position of the bubble
        public float speed = 1f;           // Speed of the bubble movement
        public float floatRange = 0.5f;    // Maximum floating range of the bubble
        public float bubbleRadius = 0.5f;      // Radius of the bubble
        public float bubbleBounceForce = 1f;   // Force with which bubbles will bounce off each other
        public int targetLetter;
        private Vector2 currentTarget;     // Current target position of the bubble
        private Vector2 floatingOffset;    // Current floating offset of the bubble
        private Rigidbody2D rb;            // Rigidbody2D component of the bubble
        bool clicked;
        private void Start()
        {
            //endPosition.y = Random.Range(-6f, 6f);
            startPosition.y = Random.Range(-6f, 6f);
            speed = Random.Range(8, 12f);
            currentTarget = endPosition;
            rb = GetComponent<Rigidbody2D>();
            floatRange = Random.Range(5f, 15f);

            Vector2 targetPosition = currentTarget;

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                currentTarget = currentTarget == endPosition ? startPosition : endPosition;
            }

            // floatingOffset = new Vector2(
            //  Random.Range(-floatRange, floatRange),
            //  Mathf.PingPong(Time.time, floatRange)
            // );

            Vector2 direction = (targetPosition - rb.position).normalized;
            direction.y = Random.Range(-1f, 2f);
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            BubbleMovement bubble = collision.gameObject.GetComponent<BubbleMovement>();
            if (bubble != null)
            {
                // Calculate the direction of the collision
                Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;
                collisionDirection.Normalize();

                // Add a force to the Rigidbody2D of the collided bubble to bounce it off the current bubble
                Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
                otherRb.AddForce(collisionDirection * bubbleBounceForce, ForceMode2D.Impulse);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "TreasureBox" && clicked && targetLetter == 0)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                GetComponent<Collider2D>().isTrigger = true;
                this.transform.DOMove(new Vector3(-6.38f,-2.76f,0f), 0.3f);
                this.transform.DOScale(0f, 1f);
            }
        }

        private void OnMouseDown()
        {
            clicked = true;

            if (targetLetter == 0)
            {
                //Correct
                speed = 100f;
                Vector2 direction = (currentTarget - rb.position).normalized;
                rb.AddForce(direction * speed, ForceMode2D.Impulse);
                this.GetComponent<SpriteRenderer>().color = Color.green;
                GetComponent<Collider2D>().isTrigger = true;
                AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
                GetComponentInParent<BubblesInstantiator>().OnCorrect();
            }
            else
            {
                //not correct try again
                rb.AddForce(new Vector2(Random.Range(-3f, 9f), Random.Range(-4f, 4f)) * (25f), ForceMode2D.Impulse);
                GetComponent<Collider2D>().isTrigger = true;
                AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
                StartCoroutine(FlashEffect());
            }
        }

        private IEnumerator FlashEffect()
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
