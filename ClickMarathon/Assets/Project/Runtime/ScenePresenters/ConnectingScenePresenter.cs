using System;
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
               _dependencies.TransitionsView.FadeInAsync();

               new StupidFirebaseValidator().CheckIfAvaliable(isAvaliable =>
               {
                    if(isAvaliable)
                    {
                         InitBackFromRegistrationScenario();
                         InitBackFromAuthorizationScenario();
                         InitRegistrationScenario();
                         InitAuthorizationScenario();
                    }
                    else
                    {
                         DialogPopup.ShowDialog("Something is wrong with firebase");
                    }
               });
          }

          private void BackToWelcomeWindow(IPopupView from)
          {
               var welcomeWindow = _dependencies.WelcomeWindow;

               from.Hide(onDone: () =>
                    welcomeWindow.Show(onDone: () =>
                         welcomeWindow.UnblockInteraction()));
          }

          private void InitBackFromRegistrationScenario()
          {
               var registrationWindow = _dependencies.RegistrationWindow;

               registrationWindow.OnGoBackRequest.AddListener(() =>
               {
                    registrationWindow.BlockInteraction();
                    BackToWelcomeWindow(from: registrationWindow);
               });
          }

          private void InitBackFromAuthorizationScenario()
          {
               var authorizationWindow = _dependencies.AuthorizationWindow;

               authorizationWindow.OnGoBackRequest.AddListener(() =>
               {
                    authorizationWindow.BlockInteraction();
                    BackToWelcomeWindow(from: authorizationWindow);
               });
          }

          private void InitRegistrationScenario()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var registrationWindow = _dependencies.RegistrationWindow;
               var registrator = new UserRegistrator();

               welcomeWindow.OnRegisterButtonClicked.AddListener(() =>
               {
                    welcomeWindow.BlockInteraction();
                    welcomeWindow.Hide(onDone: () =>
                         registrationWindow.Show(onDone: () =>
                         {
                              registrationWindow.UnblockInteraction();
                              registrationWindow.OnRegisterRequest.AddListener(() =>
                              {
                                   registrationWindow.BlockInteraction();
                                   registrator.TryRegisterEmailAsync(args =>
                                   {
                                        args.Email = registrationWindow.GetEmail();
                                        args.Password = registrationWindow.GetPassword();
                                        args.OnSucceed = () =>
                                        {
                                             registrator.SetNickname(registrationWindow.GetNickname(),
                                                  onSucceed: () =>
                                                  {
                                                       // todo refactor
                                                       FirebaseServices.GetAuthenticationService().SignOut();
                                                       new UserAuthorizator().TryAuthorizeEmailAsync(args =>
                                                       {
                                                            args.Email = registrationWindow.GetEmail();
                                                            args.Password = registrationWindow.GetPassword();
                                                            args.OnSucceed = () =>
                                                            {
                                                                 CredentialsSaver.RememberMe(args.Email, args.Password);
                                                                 FirebaseApi.SynchronizePlayerEntry(onDone: () =>
                                                                      _dependencies.TransitionsView.FadeOutAsync(onDone: () =>
                                                                           SwitchScene(SceneName.PlayingScene)));
                                                            };
                                                            args.OnFailed = message =>
                                                            {
                                                                 DialogPopup.ShowDialog(message, onOK: () =>
                                                                       registrationWindow.UnblockInteraction());
                                                            };
                                                       });
                                                  });
                                        };
                                        args.OnFailed = message =>
                                        {
                                             DialogPopup.ShowDialog(message, onOK: () =>
                                                   registrationWindow.UnblockInteraction());
                                        };
                                   });
                              });
                         }));
               });
          }

          private void InitAuthorizationScenario()
          {
               var welcomeWindow = _dependencies.WelcomeWindow;
               var authorizationWindow = _dependencies.AuthorizationWindow;

               welcomeWindow.OnSignInButtonClicked.AddListener(() =>
               {
                    welcomeWindow.BlockInteraction();
                    welcomeWindow.Hide(onDone: () =>
                         authorizationWindow.Show(onDone: () =>
                         {
                              authorizationWindow.UnblockInteraction();
                              authorizationWindow.OnAuthorizeRequest.AddListener(() =>
                              {
                                   authorizationWindow.BlockInteraction();
                                   new UserAuthorizator().TryAuthorizeEmailAsync(args =>
                                   {
                                        args.Email = authorizationWindow.GetEmail();
                                        args.Password = authorizationWindow.GetPassword();
                                        args.OnSucceed = () =>
                                        {
                                             CredentialsSaver.RememberMe(args.Email, args.Password);
                                             FirebaseApi.SynchronizePlayerEntry(onDone: () =>
                                                  _dependencies.TransitionsView.FadeOutAsync(onDone: () =>
                                                       SwitchScene(SceneName.PlayingScene)));
                                        };
                                        args.OnFailed = message =>
                                        {
                                             DialogPopup.ShowDialog(message, onOK: () =>
                                                   authorizationWindow.UnblockInteraction());
                                        };
                                   });
                              });
                         }));
               });
          }
     }
}
