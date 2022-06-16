using System;
using Firebase.Database;
using static ProjectDefaults.ProjectConstants;
using FirebaseApi = FirebaseWorkers.FirebaseCustomApi;

namespace FirebaseWorkers
{
     public sealed class DatabaseListener
     {
          private Action<ScoreEntryModel> handleEntryAdded;
          private Action<ScoreEntryModel> handleEntryChanged;
          private Action<ScoreEntryModel> handleEntryRemoved;
          private Action OnReachedEnd;

          /// <summary>
          /// migrates automatically to new handler on last entry found.
          /// </summary>
          /// <param name="playtimeChildAddedHandler">
          /// U need it because [*******] firebase developers,
          /// did't implemented a way to subscribe to childAdded event,
          /// whithout retrieving all object in ref.</param>
          public void GrabEntries(
               Action onReachedEnd,
               Action<ScoreEntryModel> childFoundHandler,
               Action<ScoreEntryModel> playtimeChildAddedHandler)
          {
               OnReachedEnd = onReachedEnd;
               OnReachedEnd += () =>
               {
                    handleEntryAdded = playtimeChildAddedHandler;
               };

               handleEntryAdded = childFoundHandler;

               FirebaseApi.DashboardRef.OrderByChild(DashboardScoreKey)
                    .ChildAdded += (_, args) => TranslateChildAdded(args);
          }

          public void ListenToEntryChanged(Action<ScoreEntryModel> changedEntryHandler)
          {
               handleEntryChanged = changedEntryHandler;
               FirebaseApi.DashboardRef.ChildChanged += (_, args) => TranslateChildChanged(args);
          }

          public void ListenToEntryRemoved(Action<ScoreEntryModel> removedEntryHandler)
          {
               handleEntryRemoved = removedEntryHandler;
               FirebaseApi.DashboardRef.ChildRemoved += (_, args) => TranslateChildRemoved(args);
          }

          private void TranslateChildAdded(ChildChangedEventArgs e)
          {
               var score = e.Snapshot.Child(DashboardScoreKey).Value;

               UnityEngine.Debug.Log($"added key:{e.Snapshot.Key}");

               if((long)score == 999_999_999)
               {
                    OnReachedEnd.Invoke();
                    return;
               }

               handleEntryAdded((ScoreEntryModel)e.Snapshot);
          }

          private void TranslateChildChanged(ChildChangedEventArgs e)
          {
               handleEntryChanged((ScoreEntryModel)e.Snapshot);
          }

          private void TranslateChildRemoved(ChildChangedEventArgs e)
          {
               handleEntryRemoved((ScoreEntryModel)e.Snapshot);
          }
     }
}