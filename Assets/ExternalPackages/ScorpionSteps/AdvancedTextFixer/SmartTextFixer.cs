using ArabicSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScorpionSteps.ArabicTextFixer
{
    public class SmartTextFixer : MonoBehaviour
    {
        public TextMeshProUGUI txt;
        public Text normalTxt;
        static readonly char[] arabicNumbers = { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
        public static string currentFamilyLanguage = "ar";

        public static string Fix(string str)
        {
            return string.IsNullOrEmpty(str) ? "" : FixNormalWord(str);
        }

        public static void Fix(TextMeshProUGUI TEXT, string str, bool fixAlignment = false)
        {
            if (TEXT == null)
            {
                return;
            }

            TEXT.text = "";
            TEXT.text = str;

            if (fixAlignment)
            {
                FixAlignment(TEXT, str);
            }

            if (!FixMyText(str))
            {
                TEXT.text = str;
            }
            else
            {
                TEXT.text = FixLinesWithoutLink(TEXT, AddSpaceBetweenEmojis(str));
            }
        }

        static bool FixMyText(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            bool fix = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (HasArabicCharacters(str[i]))
                {
                    fix = true;
                    break;
                }
            }

            return fix;
        }

        public static void Fix(Text TEXT, string str, bool fixAlignment = false)
        {
            if (TEXT == null)
            {
                return;
            }

            TEXT.text = "";
            TEXT.text = str;
            if (fixAlignment)
            {
                FixAlignment(TEXT, str);
            }

            TEXT.text = FixLinesWithoutLink(TEXT, str);
        }

        //help functions....
        static void FixAlignment(TextMeshProUGUI TEXT, string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            bool isL2R = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (HasEnglishCharacters(str[i]))
                {
                    isL2R = true;
                    break;
                }
                else if (HasArabicCharacters(str[i]))
                {
                    isL2R = false;
                    break;
                }
            }
            TEXT.alignment = isL2R ? TextAlignmentOptions.Left : TextAlignmentOptions.Right;
        }

        static void FixAlignment(Text TEXT, string str)
        {
            bool isL2R = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (HasEnglishCharacters(str[i]))
                {
                    isL2R = true;
                    break;
                }
                else if (HasArabicCharacters(str[i]))
                {
                    isL2R = false;
                    break;
                }
            }
            TEXT.alignment = isL2R ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
        }

        public static void FixWithLanguageDir(Text TEXT, string str)
        {
            if (TEXT == null)
            {
                return;
            }

            TEXT.text = "";
            TEXT.text = str;
            try
            {
                ForceFixAlignment(TEXT);
                TEXT.text = FixLinesWithoutLink(TEXT, str);
            }
            catch (NullReferenceException e)
            {
                // NullReferenceExceptions will happen when switching between scenes
            }
        }
        public static void FixWithLanguageDir(TextMeshProUGUI TEXT, string str)
        {
            if (TEXT == null)
            {
                return;
            }

            TEXT.text = "";
            TEXT.text = str;
            try
            {
                ForceFixAlignment(TEXT);
                if (!FixMyText(str))
                {
                    TEXT.text = str;
                }
                else
                {
                    TEXT.text = FixLinesWithoutLink(TEXT, AddSpaceBetweenEmojis(str));
                }
            }
            catch (NullReferenceException e)
            {
                // NullReferenceExceptions will happen when switching between scenes
            }
        }

        static void ForceFixAlignment(TextMeshProUGUI TEXT)
        {
            TEXT.alignment = currentFamilyLanguage == "en" ? TextAlignmentOptions.Left : TextAlignmentOptions.Right;
        }
        static void ForceFixAlignment(Text TEXT)
        {
            TEXT.alignment = currentFamilyLanguage == "en" ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
        }

        static bool IsEmojis(in char ch)
        {
            return !char.IsWhiteSpace(ch) && !HasArabicCharacters(ch)
                                          && !HasEnglishCharacters(ch) && !Char.IsControl(ch)
                                          && !Char.IsPunctuation(ch) && !Char.IsSymbol(ch) && !Char.IsWhiteSpace(ch);
        }

        static bool ContainsEmojis(in string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (IsEmojis(str[i]))
                    return true;
            }

            return false;
        }

        static string AddSpaceBetweenEmojis(string str)
        {
            // Take the time to check if we really need to allocate memory
            if (!ContainsEmojis(in str))
            {
                return str;
            }

            var sb = new StringBuilder(str.Length);
            bool lastIndexWasEmoji = false;
            bool lastIndexWasWhiteSpace = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsWhiteSpace(str[i]) && !HasArabicCharacters(str[i])
                && !HasEnglishCharacters(str[i]) && !Char.IsControl(str[i])
                && !Char.IsPunctuation(str[i]) && !Char.IsSymbol(str[i]) && !Char.IsWhiteSpace(str[i]))
                {
                    //it is emoji;
                    if (i == 0 || lastIndexWasEmoji || lastIndexWasWhiteSpace)
                    {
                        sb.Append(str[i]);
                    }
                    else
                    {
                        sb.Append(" ");
                        sb.Append(str[i]);
                    }
                    lastIndexWasEmoji = true;
                }
                else if (char.IsWhiteSpace(str[i]))
                {
                    //it is white space;
                    sb.Append(str[i]);
                    lastIndexWasEmoji = false;
                    lastIndexWasWhiteSpace = true;
                }
                else
                {
                    //any char
                    if (lastIndexWasEmoji)
                    {
                        sb.Append(" ");
                        sb.Append(str[i]);
                    }
                    else
                    {
                        sb.Append(str[i]);
                    }
                    lastIndexWasEmoji = false;
                    lastIndexWasWhiteSpace = false;
                }
            }

            return sb.ToString();
        }
        static string FixLinesWithoutLink(Text TEXT, string originText)
        {
            TextGenerator textGeneration = TEXT.cachedTextGenerator;
            string str = originText;
            string newTEXT = "";
            List<string> allStrings = new List<string>();
            if (textGeneration.lines.Count > 1)
            {
                for (int i = 0; i < textGeneration.lines.Count; i++)
                {
                    string currentLine = "";
                    if (i == textGeneration.lines.Count - 1)
                    {
                        // last line
                        currentLine = str.Substring(textGeneration.lines[i].startCharIdx, str.Length - textGeneration.lines[i].startCharIdx);
                    }
                    else
                    {
                        currentLine = str.Substring(textGeneration.lines[i].startCharIdx, (textGeneration.lines[i + 1].startCharIdx - 1) - textGeneration.lines[i].startCharIdx);
                    }
                    allStrings.Add(currentLine);
                }
            }
            else
            {
                allStrings.Add(str);
            }
            for (int i = 0; i < allStrings.Count; i++)
            {
                newTEXT += FixAtRuntime(allStrings[i]);
                if (i != allStrings.Count - 1)
                    newTEXT += "\n";
            }

            return newTEXT;
        }

        static string FixLinesWithoutLink(TextMeshProUGUI TEXT, string originText)
        {
            TMP_TextInfo textInfo = TEXT.GetTextInfo(TEXT.text);
            string str = originText;
            string newTEXT = "";
            List<string> allStrings = new List<string>();
            if (textInfo.lineCount > 1)
            {
                for (int i = 0; i < textInfo.lineCount; i++)
                {
                    string currentLine = str.Substring(textInfo.lineInfo[i].firstCharacterIndex, (textInfo.lineInfo[i].lastCharacterIndex - textInfo.lineInfo[i].firstCharacterIndex) + 1);
                    allStrings.Add(currentLine);
                }
            }
            else
            {
                allStrings.Add(str);
            }
            for (int i = 0; i < allStrings.Count; i++)
            {
                newTEXT += FixAtRuntime(allStrings[i]);
                if (i != allStrings.Count - 1)
                    newTEXT += "\n";
            }
            return newTEXT;
        }
        static string FixNormalWord(string originText)
        {
            return FixAtRuntime(originText);
        }

        static string FixAtRuntime(string str)
        {
            bool isL2R = true;
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (HasEnglishCharacters(str[i]))
                    {
                        isL2R = true;
                        break;
                    }
                    else if (HasArabicCharacters(str[i]))
                    {
                        isL2R = false;
                        break;
                    }
                }
                return isL2R ? FixL2RTEXT(str) : FixR2LTEXT(str);
            }
            else
            {
                return str;
            }
        }

        static bool HasEnglishCharacters(char c)
        {
            if (IsArabicNumbers(c))
            {
                return true;
            }
            return (c >= 0x0030 && c <= 0x0039) || (c >= 0x0041 && c <= 0x005A) || (c >= 0x0061 && c <= 0x007A);
        }

        static bool HasArabicCharacters(char c)
        {
            if (IsArabicNumbers(c))
            {
                return false;
            }
            return (c >= 0x0600 && c <= 0x06FF) || (c >= 0x0750 && c <= 0x077F) || (c >= 0xFB50 && c <= 0xFDFF) || (c >= 0xFE70 && c <= 0xFEFF);
        }

        static bool IsArabicNumbers(char c)
        {
            for (int i = 0; i < arabicNumbers.Length; i++)
            {
                if (arabicNumbers[i] == c)
                {
                    return true;
                }
            }

            return false;
        }

        static string FixL2RTEXT(string str)
        {
            var arabic = new StringBuilder();
            var english = new StringBuilder();

            var l2r = new StringBuilder(str.Length);

            bool isLastCharArabic = false;
            bool isLastCharEnglish = false;
            bool isLastCharUni = false;

            string characters = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsWhiteSpace(str[i]))
                {
                    if (english.Length > 0)
                    {
                        english.Clear();
                        l2r.Append(" ");
                    }
                    else if (arabic.Length > 0)
                    {
                        isLastCharArabic = true;
                    }
                    else
                    {
                        if (isLastCharUni)
                        {
                            isLastCharUni = false;
                            l2r.Append(characters);
                            characters = "";
                        }

                        l2r.Append(" ");
                    }
                }
                else if (HasArabicCharacters(str[i]))
                {
                    if (isLastCharArabic)
                    {
                        arabic.Append(" ");
                        isLastCharArabic = false;
                    }

                    if (english.Length > 0)
                    {
                        english.Clear();
                    }

                    if (isLastCharUni)
                    {
                        isLastCharUni = false;
                        if (!arabicNumbers.Contains(str[i]))
                        {
                            arabic.Append(characters);
                            arabic.Append(str[i]);
                        }
                        else
                        {
                            arabic.Append(str[i]);
                            arabic.Append(characters);
                        }

                        characters = "";
                    }
                    else
                    {
                        arabic.Append(str[i]);
                    }
                }
                else if (HasEnglishCharacters(str[i]))
                {
                    if (arabic.Length > 0)
                    {
                        if (isLastCharUni)
                        {
                            isLastCharUni = false;
                            l2r.Append(ArabicFixer.Fix(arabic.ToString()));
                            l2r.Append(characters);
                            characters = "";
                        }
                        else
                        {
                            l2r.Append(ArabicFixer.Fix(arabic.ToString()));
                        }

                        arabic.Clear();
                        if (isLastCharArabic)
                        {
                            l2r.Append(" ");
                            isLastCharArabic = false;
                        }
                    }

                    if (isLastCharUni)
                    {
                        isLastCharUni = false;
                        english.Append(characters);
                        english.Append(str[i]);
                        l2r.Append(characters);
                        l2r.Append(str[i]);
                        characters = "";
                    }
                    else
                    {
                        english.Append(str[i]);
                        l2r.Append(str[i]);
                    }
                }
                else
                {
                    if (arabic.Length > 0)
                    {
                        //ARABIC
                        if (isLastCharArabic)
                        {
                            arabic.Append(" ");
                            isLastCharArabic = false;
                        }

                        if (english.Length > 0)
                        {
                            english.Clear();
                        }

                        isLastCharUni = true;
                        characters = str[i] + characters;
                    }
                    else if (english.Length > 0)
                    {
                        //ENGLISH
                        if (arabic.Length > 0)
                        {
                            l2r.Append(ArabicFixer.Fix(arabic.ToString()));
                            arabic.Clear();
                            if (isLastCharArabic)
                            {
                                l2r.Append(" ");
                                isLastCharArabic = false;
                            }
                        }

                        english.Append(str[i]);
                        l2r.Append(str[i]);
                    }
                    else
                    {
                        isLastCharUni = true;
                        characters += str[i];
                    }
                }
            }

            if (isLastCharUni)
            {
                isLastCharUni = false;
                l2r.Append(characters);
                characters = String.Empty;
            }

            if (arabic.Length > 0)
            {
                l2r.Append(ArabicFixer.Fix(arabic.ToString()));
                arabic.Clear();
            }

            return l2r.ToString();
        }

        static string FixR2LTEXT(string str)
        {
            var arabic = new StringBuilder();
            var english = new StringBuilder();

            var r2l = new StringBuilder(str.Length);

            bool isLastCharArabic = false;
            bool isLastCharEnglish = false;
            bool isLastCharUni = false;

            string characters = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsWhiteSpace(str[i]))
                {
                    if (arabic.Length > 0)
                    {
                        r2l.Insert(0, ArabicFixer.Fix(arabic.ToString()));
                        r2l.Insert(0, " ");
                        arabic.Clear();
                    }
                    else if (english.Length > 0)
                    {
                        isLastCharEnglish = true;
                    }
                    else
                    {
                        if (isLastCharUni)
                        {
                            isLastCharUni = false;
                            r2l.Insert(0, characters);
                            characters = String.Empty;
                        }
                        r2l.Insert(0, " ");
                    }
                }
                else if (HasArabicCharacters(str[i]))
                {
                    //Right 2 left - arabic
                    if (isLastCharEnglish)
                    {
                        r2l.Insert(0, english);
                        r2l.Insert(0, " ");
                        english.Clear();
                        isLastCharEnglish = false;
                    }
                    if (english.Length > 0)
                    {
                        r2l.Insert(0, english);
                        english.Clear();
                    }
                    if (isLastCharUni)
                    {
                        isLastCharUni = false;
                        arabic.Append(characters);
                        arabic.Append(str[i]);
                        characters = String.Empty;
                    }
                    else
                    {
                        arabic.Append(str[i]);
                    }
                    // allText = ArabicFixer.Fix(arabic) + r2l;
                }
                else if (HasEnglishCharacters(str[i]))
                {
                    //Right 2 left - english
                    if (isLastCharEnglish)
                    {
                        english.Append(" ");
                        isLastCharEnglish = false;
                    }

                    if (arabic.Length > 0)
                    {
                        r2l.Insert(0, ArabicFixer.Fix(arabic.ToString()));
                        arabic.Clear();
                    }

                    if (isLastCharUni)
                    {
                        isLastCharUni = false;
                        english.Append(characters);
                        english.Append(str[i]);
                        characters = String.Empty;
                    }
                    else
                    {
                        english.Append(str[i]);
                    }
                }
                else
                {
                    if (arabic.Length > 0)
                    {
                        //ARABIC
                        //Right 2 left - arabic
                        if (isLastCharEnglish)
                        {
                            r2l.Insert(0, english);
                            r2l.Insert(0, " ");
                            english.Clear();
                            isLastCharEnglish = false;
                        }
                        if (english.Length > 0)
                        {
                            r2l.Insert(0, english);
                            english.Clear();
                        }

                        arabic.Append(str[i]);
                        r2l.Insert(0, ArabicFixer.Fix(arabic.ToString()));
                        arabic.Clear();
                    }
                    else if (english.Length > 0)
                    {
                        //ENGLISH

                        //Right 2 left - english
                        if (isLastCharEnglish)
                        {
                            english.Append(" ");
                            isLastCharEnglish = false;
                        }

                        if (arabic.Length > 0)
                        {
                            r2l.Insert(0, ArabicFixer.Fix(arabic.ToString()));
                            arabic.Clear();
                        }

                        english.Append(str[i]);
                    }
                    else
                    {
                        isLastCharUni = true;
                        characters += str[i];
                    }
                }
            }

            if (isLastCharUni)
            {
                isLastCharUni = false;
                r2l.Insert(0, characters);
                characters = String.Empty;
            }

            if (english.Length > 0)
            {
                r2l.Insert(0, english);
                english.Clear();
            }

            if (arabic.Length > 0)
            {
                r2l.Insert(0, ArabicFixer.Fix(arabic.ToString()));
                arabic.Clear();
            }

            return r2l.ToString();
        }
    }
}