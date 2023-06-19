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
      public bool game_1_completed;
      public bool game_2_completed;
      public bool game_3_completed;
      public bool coloring_completed;
      public bool tracing_basic_completed;
      public bool tracing_advanced_completed;
    }
  }
}