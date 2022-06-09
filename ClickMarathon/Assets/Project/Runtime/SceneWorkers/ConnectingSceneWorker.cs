using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using WindowViews;
using FirebaseWorkers;
using static FirebaseWorkers.FirebaseCustomApi;
using static FirebaseWorkers.FirebaseServices;

namespace SceneWorkers
{
     public sealed class ConnectingSceneWorker: SceneWorker
     {
          [SerializeField] private ConnectingSceneDependencyContainer _dependencyContainer;

          protected override void EnteringScene()
          {
               new StupidFirebaseValidator().CheckIfAvaliable(isAvaliable =>
               {
                    if(isAvaliable)
                    {
                         SubscribeRegistrationWindowInitializer();
                         SubscribeLogInWindowInitializer();
                    }
                    else
                    {
                         // todo: maybe inform the user about that?
                    }
               });
          }

          private void SubscribeRegistrationWindowInitializer()
          {
               var welcomeWindow = _dependencyContainer.WelcomeWindow;
               var registrationWindow = _dependencyContainer.RegistrationWindow;

               welcomeWindow.OnRegisterButtonClicked.AddListener(() =>
               {
                    welcomeWindow.OnRegisterButtonClicked.RemoveAllListeners();
                    StartCoroutine(welcomeWindow.Hide(onDone: () =>
                         StartCoroutine(registrationWindow.Show())));
               });
          }

          private void SubscribeLogInWindowInitializer()
          {
               var welcomeWindow = _dependencyContainer.WelcomeWindow;
               var logInWindow = _dependencyContainer.LogInWindow;

               welcomeWindow.OnSignInButtonClicked.AddListener(() =>
               {
                    welcomeWindow.OnRegisterButtonClicked.RemoveAllListeners();
                    StartCoroutine(welcomeWindow.Hide(onDone: () =>
                         StartCoroutine(logInWindow.Show(onDone: () =>
                              new Loginner(logInWindow, onLoggedInSubscriber: () =>
                              {
                                   GetOrCreateUserEntryInfoAsync(
                                        foundHandler: currentUserEntryInfo =>
                                        {
                                             CurrentUserEntry = currentUserEntryInfo;
                                             SwitchScene(SceneName.PlayingScene);
                                        },
                                        taskExceptionHandler: _ => { },
                                        firebaseExceptionHandler: _ => { },
                                        cantFindHandler: () => { });
                              })))));
               });
          }

          [Serializable]
          private sealed class ConnectingSceneDependencyContainer
          {
               public IWelcomeWindowView WelcomeWindow => _welcomeWindow;
               public IRegistrationWindowView RegistrationWindow => _registrationWindow;
               public ILogInWindowView LogInWindow => _logInWindow;

               [SerializeField] private WelcomeWindowView _welcomeWindow;
               [SerializeField] private RegistrationWindowView _registrationWindow;
               [SerializeField] private LogInWindowView _logInWindow;
          }
     }
}
