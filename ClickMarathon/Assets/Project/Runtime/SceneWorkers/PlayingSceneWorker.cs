using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using Runtime.DependencyContainers;
using static ProjectDefaults.ProjectConstants;
using static FirebaseWorkers.FirebaseCustomApi;
using static FirebaseWorkers.FirebaseServices;

namespace Runtime.SceneWorkers
{
     public sealed class PlayingSceneWorker: SceneWorker
     {
          [SerializeField] private PlayingSceneContainer _dependencies;
          private ILeaderboardPresenter _leaderboard;
          private DatabaseListener _databaseListener;

          private bool _isReadyToStart = false;

          protected override void EnteringScene()
          {
               CheckFirebaseStuff();

               _leaderboard = new LeaderboardPresenter(_dependencies.LeaderboardView);
               new MagicPlayButtonPresenter(
                    _dependencies.PlayButtonView,
                    _dependencies.PlayTimerView,
                    newScoreReadyHandler: entry =>
                    {
                         WriteScoreEntryAsync(CurrentUserEntry).ContinueWith(task =>
                         {
                              if(task.IsCompletedSuccessfully)
                                   Debug.Log($"entry writing task completed successfully!");
                              else
                                   Debug.Log($"something whent wrong. ex:{task.Exception}");
                         });
                    });

               StartCoroutine(LetsPlayWhenReady());

               _databaseListener = new DatabaseListener();
               _databaseListener.GrabEntries(
                    onReachedEnd: () =>
                    {
                         _isReadyToStart = true;
                         _databaseListener.ListenToEntryChanged(_leaderboard.HandleEntryChanged);
                         _databaseListener.ListenToEntryRemoved(_leaderboard.HandleEntryRemoved);
                    },
                    childFoundHandler: _leaderboard.HandleEntryFound,
                    playtimeChildAddedHandler: _leaderboard.HandleEntryAdded);
          }

          private IEnumerator LetsPlayWhenReady()
          {
               yield return new WaitForSeconds(MinHaltDurationBeforePlay);
               yield return new WaitUntil(() => _isReadyToStart);

               StartCoroutine(_dependencies.ConnectingWindow.Hide(onDone: () =>
               {
                    _leaderboard.DisplayOnlyActualSegment();

                    StartCoroutine(_dependencies.GameWindowView.Show());
               }));
          }

          private void CheckFirebaseStuff()
          {
               if(TryGetCachedUser(out _) == false)
               {
                    // todo get dumped email and password
                    // todo try to relogin
               }
          }
     }
}
