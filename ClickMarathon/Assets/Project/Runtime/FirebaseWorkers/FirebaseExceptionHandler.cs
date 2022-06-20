using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using External.Signatures;
using External.Extensions;

namespace FirebaseWorkers
{
     public sealed class FirebaseExceptionHandler
     {
          // todo refactor.
          // todo try handle exceptions.
          public static void CatchAuthorizationAttemptResult(
               Action<AuthorizationAttemptArgs> argumentsSetter)
          {
               var args = new AuthorizationAttemptArgs();
               argumentsSetter.Invoke(args);

               if(args.FinishedTask.IsCanceled)
               {
                    Debug.LogError("Authorization: was canceled.");
               }
               else if(args.FinishedTask.IsFaulted)
               {
                    var exception = args.FinishedTask.Exception;
                    var lastInnerException = exception.GetLastInner();

                    Debug.LogError("Authorization: encountered an error: " + exception);
                    args.OnFailed?.Invoke(lastInnerException.Message);
               }
               else if(args.FinishedTask.IsCompletedSuccessfully && args.FinishedTask.Result != null)
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
               public ExceptionCallback OnFailed;
          }
     }
}
