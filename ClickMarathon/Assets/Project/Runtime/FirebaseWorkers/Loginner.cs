using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using WindowViews;
using static FirebaseWorkers.FirebaseServices;

namespace FirebaseWorkers
{
     public sealed class Loginner
     {
          public Loginner(ILogInWindowView loginWindowView, Action onLoggedInSubscriber)
          {
               SubscribeOnLogInClick(loginWindowView);
               loginWindowView.UnlockInteraction();

               GetEmail = loginWindowView.GetEmail;
               GetPassword = loginWindowView.GetPassword;
               OnLoggedIn += onLoggedInSubscriber;
          }

          private Func<string> GetEmail;
          private Func<string> GetPassword;
          private event Action OnLoggedIn;

          private void SubscribeOnLogInClick(ILogInWindowView loginWindowView)
          {
               loginWindowView.OnLogInButtonClicked.AddListener(() =>
                    loginWindowView.LockInteraction());

               loginWindowView.OnLogInButtonClicked.AddListener(TryLogin);
          }

          private void TryLogin()
          {
               Debug.Log($"TryLogin() invoked");
               string email = GetEmail();
               string password = GetPassword();

               GetAuthenticationService()
                    .SignInWithEmailAndPasswordAsync(email, password)
                         .ContinueWithOnMainThread(CatchLoginAttemptResult);
          }

          // todo refactor.
          // todo try handle exceptions.
          // todo inform in the user "Message Window".
          private void CatchLoginAttemptResult(Task<FirebaseUser> finishedTask)
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
                    OnLoggedIn?.Invoke();
               }
               else
               {
                    Debug.Log("Something went wrong.");
               }

               //FirebaseUser newUser = finishedTask.Result;
          }
     }
}
