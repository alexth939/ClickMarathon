namespace ProjectDefaults
{
     public static class ProjectConstants
     {
          public const float almostOne = 0.99f;
          public const float almostZero = 0.01f;

          public const float WindowFadeInDuration = 0.5f;
          public const float WindowFadeOutDuration = 0.5f;
          public const float MinHaltDurationBeforePlay = 1.0f;

          public const string FirebaseDashboardPath = "dashboard";
          public const string DashboardNameKey = "name";
          public const string DashboardScoreKey = "score";

          public const string WindowBusyMessage = "U tryin to Hide() busy window";
          public const string WindowNotRevealedMessage = "U tryin to Hide() not completely revealed window";
          public const string WindowNotHiddenMessage = "U tryin to Show() not completely hidden window";
     }
}
