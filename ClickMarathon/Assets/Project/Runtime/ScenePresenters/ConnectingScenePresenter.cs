using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using Runtime.DependencyContainers;
using FirebaseApi =  FirebaseWorkers.FirebaseCustomApi;

namespace Runtime.ScenePresenters
{
     public sealed class ConnectingScenePresenter: ScenePresenter
     {
          [SerializeField] private ConnectingSceneContainer _dependencies;

          protected override void EnteringScene()
          {
               new StupidFirebaseValidator().CheckIfAvaliable(isAvaliable =>
               {
                    if(isAvaliable)
                    {
                         InitRegistrationScenario();
                         InitAuthorizationScenario();
                    }
                    else
                    {
                         Debug.Log($"Something is wrong with firebase");
                         // todo: maybe inform the user about that?
                    }
               });
          }

          private void InitRegistrationScenario()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var registrationWindow = _dependencies.RegistrationWindow;

               welcomeWindow.OnRegisterButtonClicked.AddListener(() =>
               {
                    welcomeWindow.CleatAllListeners();

                    StartCoroutine(welcomeWindow.Hide(onDone: () =>
                         StartCoroutine(registrationWindow.Show())));
               });
          }

          private void InitAuthorizationScenario()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var authorizationWindow = _dependencies.AuthorizationWindow;

               welcomeWindow.OnSignInButtonClicked.AddListener(() =>
               {
                    welcomeWindow.CleatAllListeners();
                    StartCoroutine(welcomeWindow.Hide(onDone: () =>
                         StartCoroutine(authorizationWindow.Show(onDone: () =>
                              authorizationWindow.OnAuthorizeRequest.AddListener(() =>
                              {
                                   authorizationWindow.BlockInteraction();
                                   new Authorizator().TryAuthorizeWithEmail(args =>
                                   {
                                        args.Email = authorizationWindow.GetEmail();
                                        args.Password = authorizationWindow.GetPassword();
                                        args.OnSucceed = () =>
                                        {
                                             FirebaseApi.SynchronizePlayerInfo(onDone: () =>
                                             {
                                                  SwitchScene(SceneName.PlayingScene);
                                             });
                                        };
                                   });
                              })))));
               });
          }
     }
}
