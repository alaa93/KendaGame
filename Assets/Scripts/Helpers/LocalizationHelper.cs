using I2.Loc;

namespace KendaGame.Helpers {
  public static class LocalizationHelper {

    public static string GetCurrentLocale() {
      return LocalizationManager.CurrentLanguageCode;
    }

    public static bool IsRTL() {
      return LocalizationManager.IsRTL(GetCurrentLocale());
    }

  }
}
