using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ScorpionSteps.ArabicTextFixer
{
    public class SmartText : MonoBehaviour
    {
        public enum TypeOfText { TextMeshPro, Text };
        public TypeOfText typeOfText;
        public enum TypeOfFix { Fix, FixWithLanguageDir };
        public TypeOfFix typeOfFix;

        [SerializeField]
        bool useUpdate;

        [HideInInspector]
        public bool fixAlignment;

        [HideInInspector]
        public bool fixAlignmentBasedOnLanguage;

        Text currentText;
        TextMeshProUGUI currentTextMeshPro;
        string currentTextStr, initText;

        public string OriginalText
        {
            get { return initText; }
        }

        bool isTextFix;
        bool isEnabledBefore;


        private void Update()
        {
            if (useUpdate)
            {
                Fix();
            }
        }
        private void LateUpdate()
        {
            if (!useUpdate)
            {
                Fix();
            }
        }
        private void Fix()
        {
            if (typeOfText == TypeOfText.Text)
            {
                if (currentTextStr != currentText.text && isTextFix)
                {
                    initText = currentText.text;
                    StartCoroutine(FixText());
                }
            }
            else if (typeOfText == TypeOfText.TextMeshPro)
            {
                if (currentTextStr != currentTextMeshPro.text && isTextFix)
                {
                    initText = currentTextMeshPro.text;
                    FixTextMeshPro();
                }
            }
        }

        void OnEnable()
        {
            if (isEnabledBefore)
                return;
            SetTypeOfText();
            isEnabledBefore = true;
            isTextFix = false;
            if (typeOfText == TypeOfText.Text)
            {
                if (currentText == null)
                {
                    currentText = GetComponent<Text>();
                    //currentText.font = SetupFont();
                }

                currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 0);

                initText = currentText.text;
                currentText.text = "";

                if (!string.IsNullOrEmpty(initText))
                {
                    StartCoroutine(FixText());
                }
                else
                {
                    currentTextStr = currentText.text;
                    isTextFix = true;
                }


            }
            else if (typeOfText == TypeOfText.TextMeshPro)
            {
                if (currentTextMeshPro == null)
                {
                    currentTextMeshPro = GetComponent<TextMeshProUGUI>();
                }

                currentTextMeshPro.color = new Color(currentTextMeshPro.color.r, currentTextMeshPro.color.g, currentTextMeshPro.color.b, 0);

                initText = currentTextMeshPro.text;
                currentTextMeshPro.text = "";
                if (!string.IsNullOrEmpty(initText))
                {
                    FixTextMeshPro();
                }
                else
                {
                    currentTextStr = currentTextMeshPro.text;
                    isTextFix = true;
                }

            }
        }

        IEnumerator FixText()
        {
            isTextFix = false;
            currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 0);
            yield return new WaitForEndOfFrame();
            if (typeOfFix == TypeOfFix.Fix)
            {
                SmartTextFixer.Fix(currentText, initText, fixAlignment);
            }
            else if (typeOfFix == TypeOfFix.FixWithLanguageDir)
            {
                SmartTextFixer.FixWithLanguageDir(currentText, initText);
            }

            currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 1);
            currentTextStr = currentText.text;
            isTextFix = true;
        }



        void FixTextMeshPro()
        {
            isTextFix = false;
            currentTextMeshPro.color = new Color(currentTextMeshPro.color.r, currentTextMeshPro.color.g, currentTextMeshPro.color.b, 0);

            if (typeOfFix == TypeOfFix.Fix)
            {
                SmartTextFixer.Fix(currentTextMeshPro, initText, fixAlignment);
            }
            else if (typeOfFix == TypeOfFix.FixWithLanguageDir)
            {
                SmartTextFixer.FixWithLanguageDir(currentTextMeshPro, initText);
            }

            currentTextMeshPro.color = new Color(currentTextMeshPro.color.r, currentTextMeshPro.color.g, currentTextMeshPro.color.b, 1);
            currentTextStr = currentTextMeshPro.text;
            isTextFix = true;
        }


        public void UseFixAlignmentBasedOnLanguage(bool value)
        {
            fixAlignmentBasedOnLanguage = value;
            if (value)
            {
                typeOfFix = TypeOfFix.FixWithLanguageDir;
            }
            else
            {
                typeOfFix = TypeOfFix.Fix;
            }
        }

        public void SetTypeOfText()
        {
            if (GetComponent<Text>() != null)
            {
                typeOfText = TypeOfText.Text;
            }
            else if (GetComponent<TextMeshProUGUI>() != null)
            {
                typeOfText = TypeOfText.TextMeshPro;
            }
        }

        public Font SetupFont()
        {
            Font font;
            if (SmartTextFixer.currentFamilyLanguage == "ar")
            {
                font = Resources.Load("Fonts/DroidKufi-Regular") as Font;
            }
            else
            {
                font = Resources.Load("Fonts/Lato-Regular") as Font;
            }
            return font;
        }
        public bool IsNormalText()
        {
            return GetComponent<Text>() != null;
        }

        public void SetupFontInEditMode(Font f)
        {
            GetComponent<Text>().font = f;
        }
    }
}
