using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using static FirebaseWorkers.FirebaseServices;
using static ProjectDefaults.ProjectConstants;
using Firebase.Extensions;

namespace FirebaseWorkers
{
     public static class FirebaseCustomApi
     {
          private static Lazy<DatabaseReference> LazyDashboardRef => new Lazy<DatabaseReference>(() => GetDatabaseService().GetReference(FirebaseDashboardPath));
          public static DatabaseReference DashboardRef => LazyDashboardRef.Value;
          public static void LogOut() => GetAuthenticationService().SignOut();

          public static bool TryGetCachedUser(out FirebaseUser user)
          {
               user = GetAuthenticationService().CurrentUser;
               return user.Equals(null);
          }

          public static Task<DataSnapshot> ReadScoreEntryAsync()
          {
               Debug.Log($"Read()");

               TryGetCachedUser(out var user);
               return DashboardRef.Child(user.UserId).GetValueAsync();
          }

          public static Task WriteScoreEntryAsync(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"Write()");

               TryGetCachedUser(out var user);
               var fields = JsonUtility.ToJson(scoreEntry.Fields);
               return DashboardRef.Child(user.UserId).SetRawJsonValueAsync(fields);
          }

          // todo refactor or burn it.
          public static void GetOrCreateUserEntryInfoAsync(Action<ScoreEntryModel> foundHandler,
               Action<AggregateException> taskExceptionHandler,
               Action<DataSnapshot> firebaseExceptionHandler,
               Action cantFindHandler)
          {
               ReadScoreEntryAsync().ContinueWithOnMainThread(task =>
               {
                    if(task.Exception != null)
                    {
                         Debug.Log($"task.Exception:{task.Exception}");
                    }
                    else if(task.Result.Exists)
                    {
                         Debug.Log($"found");
                         foundHandler.Invoke((ScoreEntryModel)task.Result);
                    }
                    else
                    {
                         Debug.Log($"not found");
                         WriteScoreEntryAsync(ScoreEntryModel.GenerateDefault())
                              .ContinueWithOnMainThread(task =>
                              {
                                   if(task.Exception != null)
                                   {
                                        Debug.Log($"task.Exception:{task.Exception}");
                                   }
                                   else
                                   {
                                        Debug.Log($"writed");
                                        ReadScoreEntryAsync().ContinueWithOnMainThread(task =>
                                        {
                                             if(task.Exception != null)
                                             {
                                                  Debug.Log($"task.Exception:{task.Exception}");
                                             }
                                             else if(task.Result.Exists)
                                             {
                                                  Debug.Log($"found");
                                                  foundHandler.Invoke((ScoreEntryModel)task.Result);
                                             }
                                             else
                                             {
                                                  Debug.Log($"cant find or create.");
                                             }
                                        });
                                   }
                              });
                    }
               });
          }

          private class JsonUser
          {
               public string name;
               public int score;
          }
     }
}
