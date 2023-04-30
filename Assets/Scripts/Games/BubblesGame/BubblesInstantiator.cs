using System.Collections;
using UnityEngine;
namespace KendaGame.Games
{
    public class BubblesInstantiator : MonoBehaviour
    {
        [SerializeField]
        bool canInstantiate;
        [SerializeField]
        GameObject bubbleInstance;
        [SerializeField]
        Transform bubbleParent;
        int counter = 0;
        int correctCounter = 0;
        string[] arabicAlphabet = new string[] {
    "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي"
};
        private void Start()
        {
            StartCoroutine(InstantiateBubbles());
        }

        private IEnumerator InstantiateBubbles()
        {
            while (canInstantiate && counter < 100)
            {
                counter++;
                correctCounter++;
                yield return new WaitForSeconds(Random.Range(2f, 4.5f));
                BubbleMovement bubble = Instantiate(bubbleInstance, bubbleParent).GetComponent<BubbleMovement>();
                bubble.gameObject.SetActive(true);
                if (correctCounter >= 3)
                {
                    correctCounter = 0;
                    bubble.targetLetter = 0;
                    bubble.letterText.text = arabicAlphabet[bubble.targetLetter];
                }
                else
                {
                    bubble.targetLetter = Random.Range(0, arabicAlphabet.Length - 1);
                    bubble.letterText.text = arabicAlphabet[bubble.targetLetter];
                }

            }
        }
    }
}