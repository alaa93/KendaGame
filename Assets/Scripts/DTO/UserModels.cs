namespace KendaGame.Models {
  public static class UserModels {

    [System.Serializable]
    public class User {
      public string user_id;
      public string name;
      public string email;
      public string password;
      public string sign_in_token;
      public string phone_number;
      public string profile_photo;
    }
  }
}