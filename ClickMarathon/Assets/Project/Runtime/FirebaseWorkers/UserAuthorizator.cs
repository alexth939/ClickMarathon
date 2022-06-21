using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using External.Signatures;
using static FirebaseWorkers.FirebaseServices;
using ExceptionHandler = FirebaseWorkers.FirebaseExceptionHandler;

namespace FirebaseWorkers
{
     public sealed class UserAuthorizator
     {
          public void TryAuthorizeEmailAsync(Action<EmailAuthorizationArgs> argumentsSetter)
          {
               Debug.Log($"Try Authorize() invoked");

               var methodArgs = new EmailAuthorizationArgs();
               argumentsSetter.Invoke(methodArgs);

               GetAuthenticationService()
                    .SignInWithEmailAndPasswordAsync(methodArgs.Email, methodArgs.Password)
                         .ContinueWithOnMainThread(task =>
                              ExceptionHandler.HandleAuthorizationResults(args =>
                              {
                                   args.FinishedTask = task;
                                   args.OnSucceed = methodArgs.OnSucceed;
                                   args.OnFailed = methodArgs.OnFailed;
                              }));
          }

          public sealed class EmailAuthorizationArgs
          {
               public string Email;
               public string Password;
               public Action<FirebaseUser> OnSucceed;
               public ExceptionCallback OnFailed = null;
          }
     }
}
