using UnityEngine;

namespace KendaGame.Helpers
{
	public static class StorageHelper
	{

		public static string CurrentLocale = "ar";
		public static string MapLetterProgress = "MapLetterProgress";
		public static string CoinsAmount = "CoinsAmount";
		public static string UserSession = "UserSession";
		public static string LastLetterCompleted = "LastLetterCompleted";
		public static string CanWatchIntro = "CanWatchIntro";
		public static string CanWatchOutro = "CanWatchOutro";
		public static string Hat = "Hat";
		public static string Glasses = "Glasses";
		public static string Mustache = "Mustache";
		public static string UsedHat = "UsedHat";
		public static string UsedGlasses = "UsedGlasses";
		public static string UsedMustache = "UsedMustache";
		public static string ISMUSICMUTED = "ISMUSICMUTE";
		public static string ISSFXMUTED = "ISSFXMUTE";
		public static string ISNARRATIONMUTED = "IsNarrationMuted";




		public static void WriteStorage(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
			PlayerPrefs.Save();
		}

		public static void DeleteStorage(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		public static string ReadStorage(string key)
		{
			string value = PlayerPrefs.GetString(key);
			return value;
		}

		public static string ReadStorage(string key, string def)
		{
			string res = ReadStorage(key);
			return string.IsNullOrEmpty(res) ? def : res;
		}
	}
}