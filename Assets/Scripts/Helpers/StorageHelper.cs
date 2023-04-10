using UnityEngine;

namespace KendaGame.Helpers {
  public static class StorageHelper {

    public static string CurrentLocale = "ar";

    public static void WriteStorage(string key, string value) {
      PlayerPrefs.SetString(key, value);
      PlayerPrefs.Save();
    }

    public static void DeleteStorage(string key) {
      PlayerPrefs.DeleteKey(key);
    }

    public static string ReadStorage(string key) {
      string value = PlayerPrefs.GetString(key);
      return value;
    }

    public static string ReadStorage(string key, string def) {
      string res = ReadStorage(key);
      return string.IsNullOrEmpty(res) ? def : res;
    }
  }
}
