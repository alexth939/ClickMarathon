using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using WindowViews;
using Runtime;
using static ProjectDefaults.ProjectConstants;
using static FirebaseWorkers.FirebaseServices;
using static FirebaseWorkers.FirebaseCustomApi;

namespace SceneWorkers
{
     public sealed class PlayingSceneWorker: SceneWorker
     {
          [SerializeField] private PlayingSceneDependencyContainer _dependencyContainer;

          private bool _isReadyToStart = false;

          protected override void EnteringScene()
          {
               CheckFirebaseStuff();
               var leaderboardPresenter =
                    new LeaderboardPresenter(_dependencyContainer.LeaderboardView);

               new DatabaseGrabber(leaderboardPresenter.HandleFoundEntry)
                    .IterateEntries(() =>
                    {
                         _isReadyToStart = true;
                         leaderboardPresenter.DisplayFilteredResults();
                    });

               StartCoroutine(ShowGameWindowWhenReady());
          }

          private IEnumerator ShowGameWindowWhenReady()
          {
               yield return new WaitForSeconds(MinHaltDurationBeforePlay);
               yield return new WaitUntil(() => _isReadyToStart);

               StartCoroutine(_dependencyContainer.ConnectingWindow.Hide(onDone: () =>
                    StartCoroutine(_dependencyContainer.GameWindowView.Show())));
          }

          private void CheckFirebaseStuff()
          {
               if(TryGetCachedUser(out _) == false)
               {
                    // todo get dumped email and password
                    // todo try to relogin
               }
          }

          [Serializable]
          private sealed class PlayingSceneDependencyContainer
          {
               public IConnectingWindowView ConnectingWindow => _connectingWindow;
               public IGameWindowView GameWindowView => _gameWindowView;
               public ILeaderboardView LeaderboardView => _leaderboardView;

               [SerializeField] private ConnectingWindowView _connectingWindow;
               [SerializeField] private GameWindowView _gameWindowView;
               [SerializeField] private LeaderboardView _leaderboardView;
          }
     }
}
