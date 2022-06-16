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
               return user.Equals(null);
          }

          public static Task<DataSnapshot> ReadScoreEntryAsync()
          {
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
               GetOrCreatePlayerInfoAsync(
                    foundHandler: currentUserEntryInfo =>
                    {
                         CurrentUserEntry = currentUserEntryInfo;
                         onDone.Invoke();
                    },
                    taskExceptionHandler: _ => { },
                    firebaseExceptionHandler: _ => { },
                    cantFindHandler: () => { });
          }

          // todo refactor or burn it.
          public static void GetOrCreatePlayerInfoAsync(Action<ScoreEntryModel> foundHandler,
               Action<AggregateException> taskExceptionHandler,
               Action<DataSnapshot> firebaseExceptionHandler,
               Action cantFindHandler)
          {
               TryGetCachedUser(out var user);
               try
               {
                    Debug.Log($"my firebase user: {user.UserId}");
               }
               catch(Exception ex)
               {
                    Debug.LogError($"ex:{ex.Message}");
               }

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
