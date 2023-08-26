namespace KendaGame.Models {
  public static class TracingModels {

    [System.Serializable]
    public class RatingData {
      public float PredictedScore;
      public string PredictedClass;
      public int StarRating;
      public string cropped_img_base64;
      public string PredictedPoints;
    }

    [System.Serializable]
    public class LetterProgress {
      public string letter_id;
      public int letter_rating;
      public int game_1_completed;
      public int game_2_completed;
      public int game_3_completed;
      public int coloring_completed;
      public int tracing_basic_completed;
      public int tracing_advanced_completed;
    }
  }
}