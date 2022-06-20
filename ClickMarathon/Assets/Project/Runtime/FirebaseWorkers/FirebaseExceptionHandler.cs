using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;

namespace FirebaseWorkers
{
     public sealed class FirebaseExceptionHandler
     {
          // todo refactor.
          // todo try handle exceptions.
          // todo inform in the user "Message Window".
          public static void CatchAuthorizationAttemptResult(
               Action<AuthorizationAttemptArgs> argumentsSetter)
          {
               var args = new AuthorizationAttemptArgs();
               argumentsSetter.Invoke(args);

               if(args.FinishedTask.IsCanceled)
               {
                    Debug.LogError("Authorization: was canceled.");
                    return;
               }
               if(args.FinishedTask.IsFaulted)
               {
                    Debug.LogError("Authorization: encountered an error: " + args.FinishedTask.Exception);
                    args.OnFailed?.Invoke();
                    return;
               }

               if(args.FinishedTask.IsCompletedSuccessfully && args.FinishedTask.Result != null)
               {
                    Debug.Log("Authorization: successfully");
                    args.OnSucceed.Invoke();
               }
               else
               {
                    Debug.Log("Authorization: Something went wrong.");
               }
          }

          public class AuthorizationAttemptArgs
          {
               public Task<FirebaseUser> FinishedTask;
               public Action OnSucceed;
               public Action OnFailed;
          }
     }
}
