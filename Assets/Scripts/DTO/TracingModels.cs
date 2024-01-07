using KendaGame.Helpers;
using System.Collections.Generic;

namespace KendaGame.Models {
  public static class TracingModels {
    
    [System.Serializable]
    public class DashBordData {
      public List<LetterProgress> data;
      public int last_seven_days_time;
    }


    public class RatingData {
      public float PredictedScore;
      public string PredictedClass;
      public int StarRating;
      public string cropped_img_base64;
      public string PredictedPoints;
    }

    public class LetterProgress {
      public string letter_id;
      public int letter_rating;
      public int game_1_completed;
      public int game_2_completed;
      public int game_3_completed;
      public int coloring_completed;
      public int tracing_basic_coloring_rating;
      public int tracing_advanced_completed;
    }
  }
}