using System;
using UnityEngine;
using Firebase.Database;
using Runtime;
using static FirebaseWorkers.FirebaseCustomApi;
using static ProjectDefaults.ProjectConstants;

namespace FirebaseWorkers
{
     public sealed class DatabaseGrabber
     {
          public DatabaseGrabber(Action<ScoreEntryModel> pulledEntryHandler)
          {
               HandleEntry = pulledEntryHandler;
          }

          private Action<ScoreEntryModel> HandleEntry;
          private Action SendSignalToStart;

          public void IterateEntries(Action readyToStartSignal)
          {
               SendSignalToStart = readyToStartSignal;
               DashboardRef.OrderByChild(DashboardScoreKey).ChildAdded += OnChildAdded;
          }

          private void UnsubscribeOnLastEntry()
          {
               DashboardRef.ChildAdded -= OnChildAdded;
               HandleEntry = entry =>
                    Debug.LogWarning($"U R tryin to call null Action, with this key:({entry.ID})");
               SendSignalToStart();
               //DashboardRef.KeepSynced(false);
          }

          private void OnChildAdded(object sender, ChildChangedEventArgs e)
          {
               //Debug.Log($"err:({e.DatabaseError}). found:({e.Snapshot.Key})");

               var name = e.Snapshot.Child("name").Value;
               var score = e.Snapshot.Child("score").Value;

               //Debug.Log($"({name.GetType()}){name}");
               //Debug.Log($"({score.GetType()}){score}");

               if((long)score == 999_999_999)
               {
                    UnsubscribeOnLastEntry();
                    return;
               }

               HandleEntry((ScoreEntryModel)e.Snapshot);
          }
     }
}
