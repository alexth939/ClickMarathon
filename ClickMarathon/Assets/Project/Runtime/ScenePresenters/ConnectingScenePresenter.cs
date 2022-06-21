using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using Runtime.DependencyContainers;
using Popups;
using FirebaseApi = FirebaseWorkers.FirebaseCustomApi;
using ProjectDefaults;

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
               var transitionsView = _dependencies.TransitionsView;

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
                                   registrator.TryRegisterEmailAsync(registerArgs =>
                                   {
                                        registerArgs.Email = registrationWindow.GetEmail();
                                        registerArgs.Password = registrationWindow.GetPassword();
                                        registerArgs.OnFailed = message =>
                                             DialogPopup.ShowDialog(message, onOK: () =>
                                                   registrationWindow.UnblockInteraction());
                                        registerArgs.OnSucceed = myUser =>
                                        {
                                             CredentialsSaver.RememberMe(registerArgs.Email, registerArgs.Password);
                                             FirebaseApi.WriteScoreEntryAsync(writeArgs =>
                                             {
                                                  writeArgs.ScoreEntry = new ScoreEntryModel(myUser.UserId, registrationWindow.GetNickname());
                                                  writeArgs.OnFailed = message =>
                                                       DialogPopup.ShowDialog(message, onOK: () =>
                                                            registrationWindow.UnblockInteraction());
                                                  writeArgs.OnSucceed = () =>
                                                  {
                                                       ProjectStatics.CachedScoreEntry = writeArgs.ScoreEntry;
                                                       transitionsView.FadeOutAsync(onDone: () =>
                                                            SwitchScene(SceneName.PlayingScene));
                                                  };
                                             });
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
               var transitionsView = _dependencies.TransitionsView;

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
                                   new UserAuthorizator().TryAuthorizeEmailAsync(loginArgs =>
                                   {
                                        loginArgs.Email = authorizationWindow.GetEmail();
                                        loginArgs.Password = authorizationWindow.GetPassword();
                                        loginArgs.OnFailed = message =>
                                             DialogPopup.ShowDialog(message, onOK: () =>
                                                   authorizationWindow.UnblockInteraction());
                                        loginArgs.OnSucceed = myUser =>
                                        {
                                             CredentialsSaver.RememberMe(loginArgs.Email, loginArgs.Password);
                                             FirebaseApi.ReadScoreEntryAsync(readArgs =>
                                             {
                                                  readArgs.WithID = myUser.UserId;
                                                  readArgs.OnFailed = message =>
                                                       DialogPopup.ShowDialog(message, onOK: () =>
                                                            authorizationWindow.UnblockInteraction());
                                                  readArgs.OnSucceed = snapshot =>
                                                  {
                                                       ProjectStatics.CachedScoreEntry = (ScoreEntryModel)snapshot;
                                                       transitionsView.FadeOutAsync(onDone: () =>
                                                            SwitchScene(SceneName.PlayingScene));
                                                  };
                                             });
                                        };
                                   });
                              });
                         }));
               });
          }
     }
}
