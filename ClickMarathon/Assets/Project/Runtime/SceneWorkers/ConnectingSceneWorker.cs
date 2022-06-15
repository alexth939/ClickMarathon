using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using Runtime.DependencyContainers;
using static FirebaseWorkers.FirebaseCustomApi;
using static FirebaseWorkers.FirebaseServices;

namespace SceneWorkers
{
     public sealed class ConnectingSceneWorker: SceneWorker
     {
          [SerializeField] private ConnectingSceneContainer _dependencies;

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
                         Debug.Log($"Something is wrong with firebase");
                         // todo: maybe inform the user about that?
                    }
               });
          }

          private void SubscribeRegistrationWindowInitializer()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var registrationWindow = _dependencies.RegistrationWindow;

               welcomeWindow.OnRegisterButtonClicked.AddListener(() =>
               {
                    welcomeWindow.OnRegisterButtonClicked.RemoveAllListeners();
                    StartCoroutine(welcomeWindow.Hide(onDone: () =>
                         StartCoroutine(registrationWindow.Show())));
               });
          }

          private void SubscribeLogInWindowInitializer()
          {
               Debug.Log($"LogIn clicked");

               var welcomeWindow = _dependencies.WelcomeWindow;
               var logInWindow = _dependencies.LogInWindow;

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
                                             Debug.Log($"switching scene");
                                             CurrentUserEntry = currentUserEntryInfo;
                                             SwitchScene(SceneName.PlayingScene);
                                        },
                                        taskExceptionHandler: _ => { },
                                        firebaseExceptionHandler: _ => { },
                                        cantFindHandler: () => { });
                              })))));
               });
          }
     }
}
