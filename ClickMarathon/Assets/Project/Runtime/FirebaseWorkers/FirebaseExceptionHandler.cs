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
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
               }
               if(args.FinishedTask.IsFaulted)
               {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + args.FinishedTask.Exception);
                    return;
               }

               if(args.FinishedTask.IsCompletedSuccessfully && args.FinishedTask.Result != null)
               {
                    Debug.Log("User signed in successfully");
                    args.OnSucceed.Invoke();
               }
               else
               {
                    Debug.Log("Something went wrong.");
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
