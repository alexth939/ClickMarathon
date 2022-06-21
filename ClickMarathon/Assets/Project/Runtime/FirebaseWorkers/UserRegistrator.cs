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
                         .ThenHandleTaskResults(args =>
                         {
                              args.OnSucceed = methodSrgs.OnSucceed;
                              args.OnFailed = methodSrgs.OnFailed;
                         });
          }
     }
}
