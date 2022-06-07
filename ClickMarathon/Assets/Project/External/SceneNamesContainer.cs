using System;

namespace UnityEngine.SceneManagement
{
     public enum SceneName
     {
          ConnectingScene,
          PlayingScene
     }

     internal static class SceneManagementExtensions
     {
          public static string AsString(this SceneName name)
          {
               return Enum.GetName(typeof(SceneName), name);
          }
     }
}
