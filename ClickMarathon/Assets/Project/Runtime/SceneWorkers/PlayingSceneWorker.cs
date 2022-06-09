using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirebaseWorkers;
using WindowViews;
using static ProjectDefaults.ProjectConstants;
using static FirebaseWorkers.FirebaseServices;

namespace SceneWorkers
{
     public sealed class PlayingSceneWorker: SceneWorker
     {
          [SerializeField] private PlayingSceneDependencyContainer _dependencyContainer;

          protected override void EnteringScene()
          {
               CheckFirebaseStuff();

               new Runtime.DashboardBuilder().BuildTable();

               StartCoroutine(ShowGameWindowWhenReady());
          }

          IEnumerator ShowGameWindowWhenReady()
          {
               yield return new WaitForSeconds(MinHaltDurationBeforePlay);
          }

          // todo decompose
          private void CheckFirebaseStuff()
          {
               if(FirebaseCustomApi.TryGetCachedUser(out _) == false)
               {
                    // todo get dumped email and password
                    // todo try to relogin
               }
          }

          [Serializable]
          private sealed class PlayingSceneDependencyContainer
          {
               public IConnectingWindowView ConnectingWindow => _connectingWindow;

               [SerializeField] private ConnectingWindowView _connectingWindow;
          }
     }
}
