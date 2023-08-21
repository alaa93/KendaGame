using static KendaGame.Models.TracingModels;

namespace KendaGame.Models
{
	public static class UserModels
	{

		[System.Serializable]
		public class User
		{
			public string id;
			public string first_name;
			public string last_name;
			public string email;
			public string password;
			public string sign_in_token;
			public string phone_number;
			public string profile_photo;
			public string gender;
		}

		[System.Serializable]
		public class Profile
		{
			public string gender;
			public string phone;
			public string profile_photo;
		}

		[System.Serializable]
		public class UserSession
		{
			public User user;
			public Profile profile;
			public string token;
		}

		[System.Serializable]
		public class UserProgress
		{
			public string user_id;
			public LetterProgress[] letter_progress;
		}
		[System.Serializable]
		public class PlayerConfigs
		{
			public PlayerConfig appconfig;
		}
		[System.Serializable]
		public class PlayerConfig
		{
			public string app_version;
			public bool freeze_app;
			public bool force_update;
			public bool mute_music;
			public bool mute_sfx;
			public string local_lang;

		}
	}
}