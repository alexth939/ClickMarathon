using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using static FirebaseWorkers.FirebaseServices;

namespace FirebaseWorkers
{
     public sealed class Authorizator
     {
          public void TryAuthorizeWithEmail(Action<EmailAuthorizationArgs> argumentsSetter)
          {
               Debug.Log($"Try Authorize() invoked");

               var args = new EmailAuthorizationArgs();
               argumentsSetter.Invoke(args);

               GetAuthenticationService()
                    .SignInWithEmailAndPasswordAsync(args.Email, args.Password)
                         .ContinueWithOnMainThread(task =>
                              CatchAuthorizationAttemptResult(
                                   task, args.OnSucceed, args.OnFailed));
          }

          // todo refactor.
          // todo try handle exceptions.
          // todo inform in the user "Message Window".
          private void CatchAuthorizationAttemptResult(
               Task<FirebaseUser> finishedTask,
               Action onLoggedInHandler,
               Action onFailed)
          {
               if(finishedTask.IsCanceled)
               {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
               }
               if(finishedTask.IsFaulted)
               {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + finishedTask.Exception);
                    return;
               }

               if(finishedTask.IsCompletedSuccessfully && finishedTask.Result != null)
               {
                    Debug.Log("User signed in successfully");
                    onLoggedInHandler.Invoke();
               }
               else
               {
                    Debug.Log("Something went wrong.");
               }
          }

          public class EmailAuthorizationArgs
          {
               public string Email;
               public string Password;
               public Action OnSucceed;
               public Action OnFailed = null;
          }
     }
}
