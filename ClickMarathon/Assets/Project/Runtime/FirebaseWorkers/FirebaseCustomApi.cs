using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using static FirebaseWorkers.FirebaseServices;
using static ProjectDefaults.ProjectConstants;

namespace FirebaseWorkers
{
     public static class FirebaseCustomApi
     {
          public static DatabaseReference DashboardRef => GetDatabaseService().GetReference(FirebaseDashboardPath);
          public static void LogOut() => GetAuthenticationService().SignOut();

          public static bool TryGetCachedUser(out FirebaseUser user)
          {
               user = GetAuthenticationService().CurrentUser;
               return user.Equals(null);
          }

          public static void PushNewUser()
          {
               TryGetCachedUser(out var user);
               string _userID = user.UserId;
               string _userName = user.DisplayName;
               DashboardRef.Child(_userID).SetRawJsonValueAsync(
                         JsonUtility.ToJson(new JsonUser() { name = _userName, score = 0 }))
                              .ContinueWith(task =>
                              {
                                   Debug.Log($"ex:({task.Exception})");
                                   Debug.Log($"IsCompletedSuccessfully: {task.IsCompletedSuccessfully}");
                              });
          }

          private class JsonUser
          {
               public string name;
               public int score;
          }
     }
}
