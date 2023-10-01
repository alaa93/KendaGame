using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static KendaGame.Models.TracingModels;

namespace KendaGame.Helpers {
  public static class LettersHelper {
    static string[] arabicAlphabet = new string[] {
    "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي"
};
    static string[] arabicAlphabetClass = new string[] { "alef", "ba", "ta", "tha", "jem", "7ah", "kha", "dal", "thal", "ra", "zen", "sen", "shen", "sad", "thad", "taah", "thaaah", "aen", "gen", "fa", "qaf", "kaf", "lam", "mem", "non", "haa", "wow", "yah" };
    static Dictionary<string, LetterStatus> lettersStatuses;

    static string lastLetterOpen;
    public static void SetActiveLetter(string letter) {
      for (int x = 0; x < arabicAlphabet.Length; x++) {
        if (letter == arabicAlphabet[x]) {
          PlayerPrefs.SetInt("ActiveLetterIndex", x);
          break;
        }
      }
    }

    public static int GetLastLetterOpendIndex() {
      return LettersHelper.GetLetterClassIndex(lastLetterOpen);
    }

    public static void PreparingLettersStatuses(Action<bool> onComplete) {
      HTTPRequestHandler.Instance.GetStatusOfALLlettersInterface((success, letterStatuses, lastLetterOpened,error) => {
        if (success) {
          lettersStatuses = letterStatuses;
          lastLetterOpen = lastLetterOpened;
          //Map logic goes here
          onComplete?.Invoke(true);
        } else {
          Debug.Log("PreparingLettersStatuses "+success);
          onComplete?.Invoke(false);
        }
      });
    }

    public static string CheckIfLetterOpen(string letterName) {
      if (lettersStatuses.ContainsKey(letterName)) {
        LetterStatus letterStatus = lettersStatuses[letterName];
        return letterStatus.status;
      } else {
        return "Closed";
      }
    }

    public static void SetActiveLetter(int letterIndex) {
      PlayerPrefs.SetInt("ActiveLetterIndex", letterIndex);
    }

   

    public static int GetActiveLetterIndex() {
      return PlayerPrefs.GetInt("ActiveLetterIndex", 0);
    }

    public static string GetActiveLetter() {
      return arabicAlphabet[PlayerPrefs.GetInt("ActiveLetterIndex", 0)];
    }

    public static string GetActiveLetterClass() {
      return arabicAlphabetClass[GetActiveLetterIndex()];
    }

    public static string GetLetterByIndexClass(int index) {
      return arabicAlphabetClass[index];
    }
    public static string GetLetterByIndex(int index) {
      return arabicAlphabet[index];
    }

    public static int GetLetterIndex(string letter) {
      for (int x = 0; x < arabicAlphabet.Length; x++) {
        if (arabicAlphabet[x] == letter)
          return x;
      }
      return -1;
    }

    public static int GetLetterClassIndex(string letter) {
      for (int x = 0; x < arabicAlphabetClass.Length; x++) {
        if (arabicAlphabetClass[x] == letter)
          return x;
      }
      return -1;
    }

    public static void SetLetterProgress(string letter, int progress) {
      PlayerPrefs.SetInt("LetterProgress_" + letter, progress);
    }

    public static int GetLetterProgress(string letter) {
      return PlayerPrefs.GetInt("LetterProgress_" + letter, 0);
    }

    public static void SetLetterRating(string letter, int rating) {
      PlayerPrefs.SetInt("LetterRating_" + letter, rating);
    }

    public static int GetLetterRating(string letter) {
      return PlayerPrefs.GetInt("LetterRating_" + letter, -1);
    }

    public static int GetLettersCount() {
      return arabicAlphabet.Length;
    }

    public static string[] GetInCorrectLetters() {
      List<string> otherLetters = new List<string>();
      for (int x = 0; x < GetLettersCount(); x++) {
        if (arabicAlphabet[x] != GetActiveLetter()) {
          otherLetters.Add(arabicAlphabet[x]);
        }
      }
      return otherLetters.ToArray();
    }

    public static int GetIslandId(int activeLetter) {
      if (activeLetter < 8) {
        return 0;
      } else if (activeLetter < 18) {
        return 1;
      } else {
        return 2;
      }
    }
  }
}