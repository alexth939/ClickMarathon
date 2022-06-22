using System;
using External.Signatures;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using static FirebaseWorkers.FirebaseServices;
using static ProjectDefaults.ProjectConstants;

namespace FirebaseWorkers
{
     public static class FirebaseCustomApi
     {
          private static Lazy<DatabaseReference> LazyDashboardRef =>
               new Lazy<DatabaseReference>(() => GetDatabaseService().GetReference(FirebaseDashboardPath));

          public static DatabaseReference DashboardRef => LazyDashboardRef.Value;

          public static void LogOut() => GetAuthenticationService().SignOut();

          public static bool TryGetCachedUser(out FirebaseUser user)
          {
               Debug.Log($"GetCachedUser()");
               user = GetAuthenticationService().CurrentUser;
               Debug.Log($"it equals:{(user == null ? "NULL" : "not NULL, ID:" + user.UserId)}" +
                    $" Nick:{user.DisplayName}");
               return user.Equals(null);
          }

          public static void ReadScoreEntryAsync(Action<ReadEntryArgs> argumentsSetter)
          {
               Debug.Log($"read score()");

               var methodArgs = new ReadEntryArgs();
               argumentsSetter(methodArgs);

               DashboardRef.Child(methodArgs.WithID).GetValueAsync()
                    .ThenHandleTaskResults(args =>
                    {
                         args.OnSucceed = methodArgs.OnSucceed;
                         args.OnFailed = methodArgs.OnFailed;
                    });
          }

          public static void WriteScoreEntryAsync(Action<WriteEntryArgs> argumentsSetter)
          {
               Debug.Log($"write user ()");

               var methodArgs = new WriteEntryArgs();
               argumentsSetter(methodArgs);

               var fields = JsonUtility.ToJson(methodArgs.ScoreEntry.Fields);
               DashboardRef.Child(methodArgs.ScoreEntry.ID).SetRawJsonValueAsync(fields)
                    .ThenHandleTaskResults(args =>
                    {
                         args.OnSucceed = methodArgs.OnSucceed;
                         args.OnFailed = methodArgs.OnFailed;
                    });
          }

          public sealed class ReadEntryArgs
          {
               public string WithID;
               public Action<DataSnapshot> OnSucceed;
               public ExceptionCallback OnFailed;
          }

          public sealed class WriteEntryArgs
          {
               public ScoreEntryModel ScoreEntry;
               public Action OnSucceed;
               public ExceptionCallback OnFailed;
          }
     }
}
