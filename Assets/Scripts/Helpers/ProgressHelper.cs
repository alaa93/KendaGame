using KendaGame.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KendaGame.Models.TracingModels;

namespace KendaGame.Helpers {
  public static class ProgressHelper {
    public static LetterProgress FetchProgress() {
      LetterProgress letterProgress = new LetterProgress();
      letterProgress.letter_id = LettersHelper.GetActiveLetterClass();
      letterProgress.letter_rating = 3;
      letterProgress.game_1_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Game1_" + LettersHelper.GetActiveLetterIndex(), "0"));
      letterProgress.game_2_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Game2_" + LettersHelper.GetActiveLetterIndex(),"0"));
      letterProgress.game_3_completed = 3;
      letterProgress.coloring_completed = 3;
      letterProgress.tracing_basic_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Basic_" + LettersHelper.GetActiveLetterIndex()));
      letterProgress.tracing_advanced_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Advance_" + LettersHelper.GetActiveLetterIndex()));
      return letterProgress;
    }
    public static LetterProgress FetchProgress(int letterId) {
      LetterProgress letterProgress = new LetterProgress();
      letterProgress.letter_id = $"{letterId}";
      letterProgress.game_1_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Game1_" + letterId,"0"));
      letterProgress.game_2_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Game2_" + letterId,"0"));
      letterProgress.game_3_completed = 3;
      letterProgress.coloring_completed = 3;
      letterProgress.tracing_basic_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Basic_" + letterId,"0"));
      letterProgress.tracing_advanced_completed = int.Parse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_Advance_" + letterId,"0"));
      return letterProgress;
    }
  }
}