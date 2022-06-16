using System;
using UnityEngine;
using Firebase.Extensions;
using static FirebaseWorkers.FirebaseServices;
using ExceptionHandler = FirebaseWorkers.FirebaseExceptionHandler;
using EmailRegistrationArgs = FirebaseWorkers.UserAuthorizator.EmailAuthorizationArgs;

namespace FirebaseWorkers
{
     public sealed class UserRegistrator
     {
          public void TryRegisterEmailAsync(Action<EmailRegistrationArgs> argumentsSetter)
          {
               Debug.Log($"Try Register() invoked");

               var methodSrgs = new EmailRegistrationArgs();
               argumentsSetter.Invoke(methodSrgs);

               GetAuthenticationService()
                    .CreateUserWithEmailAndPasswordAsync(methodSrgs.Email, methodSrgs.Password)
                    .ContinueWithOnMainThread(task =>
                    {
                         ExceptionHandler.CatchAuthorizationAttemptResult(args =>
                         {
                              args.FinishedTask = task;
                              args.OnSucceed = methodSrgs.OnSucceed;
                              args.OnFailed = methodSrgs.OnFailed;
                         });
                    });
          }

          public void SetNickname(string nickname, Action onSucceed)
          {
               GetAuthenticationService().CurrentUser.UpdateUserProfileAsync(
                    new Firebase.Auth.UserProfile()
                    {
                         DisplayName = nickname
                    }).ContinueWithOnMainThread(task =>
                    {
                         onSucceed.Invoke();
                    });
          }
     }
}
