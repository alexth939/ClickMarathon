using System.Collections;
using FirebaseWorkers;
using Runtime.DependencyContainers;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ProjectDefaults.ProjectConstants;
using static ProjectDefaults.ProjectStatics;
using FirebaseApi = FirebaseWorkers.FirebaseCustomApi;

namespace Runtime.ScenePresenters
{
     public sealed class PlayingScenePresenter: ScenePresenter
     {
          [SerializeField] private PlayingSceneContainer _dependencies;
          private ILeaderboardPresenter _leaderboard;
          private DatabaseListener _databaseListener;
          private bool _isReadyToStart = false;

          protected override void EnteringScene()
          {
               _dependencies.TransitionsView.FadeInAsync(onDone: () =>
                     StartCoroutine(TransitToLeaderboardWhenReady()));

               CheckFirebaseStuff();
               InitMagicPlayButtonPresenter();
               InitLeaderboard();
          }

          private void InitMagicPlayButtonPresenter()
          {
               new MagicPlayButtonPresenter(
                    _dependencies.PlayButtonView,
                    _dependencies.PlayTimerView,
                    newScoreAchievedHandler: entry =>
                    {
                         FirebaseApi.WriteScoreEntryAsync(args =>
                         {
                              args.ScoreEntry = CachedScoreEntry;
                              args.OnSucceed = () => Debug.Log($"entry writing task completed successfully!");
                              args.OnFailed = message => Debug.Log($"failed to write new score. errorMsg:{message}");
                         });
                    });
          }

          private void InitLeaderboard()
          {
               _leaderboard = new LeaderboardPresenter(_dependencies.LeaderboardView);

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

          private IEnumerator TransitToLeaderboardWhenReady()
          {
               yield return new WaitForSeconds(MinHaltDurationBeforePlay);
               yield return new WaitUntil(() => _isReadyToStart);

               _dependencies.ConnectingWindow.Hide(onDone: () =>
               {
                    _leaderboard.DisplayOnlyActualSegment();

                    _dependencies.GameWindowView.Show();
               });
          }

          private void CheckFirebaseStuff()
          {
               if(FirebaseApi.TryGetCachedUser(out _) == false)
               {
                    // todo get dumped email and password
                    // todo try to relogin
               }
          }
     }
}
