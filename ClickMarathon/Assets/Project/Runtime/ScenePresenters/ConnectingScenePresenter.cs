using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using Runtime.DependencyContainers;
using Popups;
using FirebaseApi = FirebaseWorkers.FirebaseCustomApi;

namespace Runtime.ScenePresenters
{
     public sealed class ConnectingScenePresenter: ScenePresenter
     {
          [SerializeField] private ConnectingSceneContainer _dependencies;

          protected override void EnteringScene()
          {
               _dependencies.TransitionsView.FadeInAsync(() => Debug.Log($"now visible"));
               Debug.LogWarning($"EnteringScene()");

               new StupidFirebaseValidator().CheckIfAvaliable(isAvaliable =>
               {
                    if(isAvaliable)
                    {
                         InitRegistrationScenario();
                         InitAuthorizationScenario();
                    }
                    else
                    {
                         DialogPopup.ShowDialog("Something is wrong with firebase");
                    }
               });
          }

          private void InitRegistrationScenario()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var registrationWindow = _dependencies.RegistrationWindow;
               var registrator = new UserRegistrator();

               Debug.Log($"assigning OnRegisterButtonClicked");
               welcomeWindow.OnRegisterButtonClicked.AddListener(() =>
               {
                    Debug.Log($"OnRegisterButtonClicked invoked");
                    welcomeWindow.CleatAllListeners();

                    welcomeWindow.Hide(onDone: () =>
                         registrationWindow.Show(onDone: () =>
                              registrationWindow.OnRegisterRequest.AddListener(() =>
                              {
                                   registrationWindow.BlockInteraction();
                                   registrator.TryRegisterEmailAsync(args =>
                                   {
                                        args.Email = registrationWindow.GetEmail();
                                        args.Password = registrationWindow.GetPassword();
                                        args.OnSucceed = () =>
                                        {
                                             Debug.Log($"setting nick");

                                             //test
                                             //new UserAuthorizator().TryAuthorizeEmailAsync(args =>
                                             //{
                                             //     args.Email = registrationWindow.GetEmail();
                                             //     args.Password = registrationWindow.GetPassword();
                                             //     args.OnSucceed = () =>
                                             //          registrator.SetNickname(registrationWindow.GetNickname(), onSucceed: () =>
                                             //                FirebaseApi.SynchronizePlayerInfo(onDone: () =>
                                             //                     SwitchScene(SceneName.PlayingScene)));
                                             //});
                                             //tillHere

                                             registrator
                                                  .SetNickname(registrationWindow.GetNickname(),
                                                       onSucceed: () =>
                                                  FirebaseApi.SynchronizePlayerInfo(onDone: () =>
                                                       SwitchScene(SceneName.PlayingScene)));
                                        };
                                   });
                              }
                         )));
               });
          }

          private void InitAuthorizationScenario()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var authorizationWindow = _dependencies.AuthorizationWindow;

               welcomeWindow.OnSignInButtonClicked.AddListener(() =>
               {
                    welcomeWindow.CleatAllListeners();
                    welcomeWindow.Hide(onDone: () =>
                         authorizationWindow.Show(onDone: () =>
                              authorizationWindow.OnAuthorizeRequest.AddListener(() =>
                              {
                                   authorizationWindow.BlockInteraction();
                                   new UserAuthorizator().TryAuthorizeEmailAsync(args =>
                                   {
                                        args.Email = authorizationWindow.GetEmail();
                                        args.Password = authorizationWindow.GetPassword();
                                        args.OnSucceed = () =>
                                             FirebaseApi.SynchronizePlayerInfo(onDone: () =>
                                                  _dependencies.TransitionsView.FadeOutAsync(onDone: () =>
                                                       SwitchScene(SceneName.PlayingScene)));
                                        args.OnFailed = () =>
                                        {
                                             DialogPopup.ShowDialog("Failed to login.");
                                             authorizationWindow.UnblockInteraction();
                                        };
                                   });
                              })));
               });
          }
     }
}
