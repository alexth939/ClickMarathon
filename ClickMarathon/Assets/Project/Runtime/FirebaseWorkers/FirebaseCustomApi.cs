using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using static FirebaseWorkers.FirebaseServices;
using static ProjectDefaults.ProjectStatics;
using static ProjectDefaults.ProjectConstants;

namespace FirebaseWorkers
{
     public static class FirebaseCustomApi
     {
          private static Lazy<DatabaseReference> LazyDashboardRef => new Lazy<DatabaseReference>(() => GetDatabaseService().GetReference(FirebaseDashboardPath));
          public static DatabaseReference DashboardRef => LazyDashboardRef.Value;

          public static void LogOut() => GetAuthenticationService().SignOut();

          public static bool TryGetCachedUser(out FirebaseUser user)
          {
               Debug.Log($"GetCachedUser()");
               user = GetAuthenticationService().CurrentUser;
               Debug.Log($"it equals:{(user == null ? "NULL" : "not NULL, ID:" + user.UserId)}");
               return user.Equals(null);
          }

          public static Task<DataSnapshot> ReadScoreEntryAsync()
          {
               Debug.Log($"read score()");
               TryGetCachedUser(out var user);
               var val = DashboardRef.Child(user.UserId).GetValueAsync();
               return val;
          }

          public static Task WriteScoreEntryAsync(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"write user ()");
               TryGetCachedUser(out var user);
               var fields = JsonUtility.ToJson(scoreEntry.Fields);
               return DashboardRef.Child(user.UserId).SetRawJsonValueAsync(fields);
          }

          public static void SynchronizePlayerInfo(Action onDone)
          {
               Debug.Log($"sync player info()");
               GetOrCreatePlayerInfoAsync(
                    foundHandler: currentUserEntryInfo =>
                    {
                         Debug.Log($"assigning currentUser");
                         CurrentUserEntry = currentUserEntryInfo;
                         onDone.Invoke();
                    },
                    taskExceptionHandler: _ => fuck(),
                    firebaseExceptionHandler: _ => fuck(),
                    cantFindHandler: () => fuck());

               void fuck() => Debug.Log($"fuck");
          }

          // todo refactor or burn it.
          public static void GetOrCreatePlayerInfoAsync(Action<ScoreEntryModel> foundHandler,
               Action<AggregateException> taskExceptionHandler,
               Action<DataSnapshot> firebaseExceptionHandler,
               Action cantFindHandler)
          {
               Debug.Log($"get or create entry()");

               ReadScoreEntryAsync().ContinueWithOnMainThread(task =>
               {
                    if(task.Exception != null)
                    {
                         Debug.Log($"task.Exception:{task.Exception}");
                    }
                    else if(task.Result.Exists)
                    {
                         Debug.Log($"found");
                         var entry = (ScoreEntryModel)task.Result;
                         foundHandler.Invoke(entry);
                    }
                    else
                    {
                         Debug.Log($"score entry not found");
                         WriteScoreEntryAsync(ScoreEntryModel.GenerateDefault())
                              .ContinueWithOnMainThread(task =>
                              {
                                   if(task.Exception != null)
                                   {
                                        Debug.Log($"task.Exception:{task.Exception}");
                                   }
                                   else
                                   {
                                        ReadScoreEntryAsync().ContinueWithOnMainThread(task =>
                                        {
                                             if(task.Exception != null)
                                             {
                                                  Debug.Log($"task.Exception:{task.Exception}");
                                             }
                                             else if(task.Result.Exists)
                                             {
                                                  Debug.Log($"got it");
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
     }
}
