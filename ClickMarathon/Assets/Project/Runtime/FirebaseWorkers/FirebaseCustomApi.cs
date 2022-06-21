using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using External.Signatures;
using static FirebaseWorkers.FirebaseServices;
using static ProjectDefaults.ProjectStatics;
using static ProjectDefaults.ProjectConstants;
using ExceptionHandler = FirebaseWorkers.FirebaseExceptionHandler;

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
                    .ContinueWithOnMainThread(finishedTask =>
                         ExceptionHandler.HandleReadResults(args =>
                         {
                              args.FinishedTask = finishedTask;
                              args.OnSucceed = methodArgs.OnSucceed;
                              args.OnFailed = methodArgs.OnFailed;
                         }));
          }

          public static Task WriteScoreEntryAsync(Action<WriteEntryArgs> argumentsSetter)
          {
               Debug.Log($"write user ()");

               var methodArgs = new WriteEntryArgs();
               argumentsSetter(methodArgs);

               var fields = JsonUtility.ToJson(methodArgs.ScoreEntry.Fields);
               return DashboardRef.Child(methodArgs.ScoreEntry.ID).SetRawJsonValueAsync(fields)
                    .ContinueWithOnMainThread(finishedTask =>
                         ExceptionHandler.HandleWriteResults(args =>
                         {
                              args.FinishedTask = finishedTask;
                              args.OnSucceed = methodArgs.OnSucceed;
                              args.OnFailed = methodArgs.OnFailed;
                         }));
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
