using System;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWorkers;
using static ProjectDefaults.ProjectConstants;

namespace Runtime
{
     public sealed class LeaderboardView: MonoBehaviour, ILeaderboardView
     {
          [SerializeField] private EntryView _entryPrefab;
          [SerializeField] private Transform _entriesContainer;

          private List<IEntryView> _entries;

          public void SetEntries(List<ScoreEntryModel> entriesToShow)
          {
               InitViewsIfNull();

               Debug.Log($"count:{entriesToShow.Count}");
               Debug.Log($"count:{_entries.Count}");

               int i;
               for(i = 0; i < entriesToShow.Count; i++)
               {
                    _entries[i].SetEntry(entriesToShow[i]);
               }
               for(; i < LeaderboardMaxEntriesToShow; i++)
               {
                    _entries[i].Clear();
               }
          }

          private void InitViewsIfNull()
          {
               if(_entries != null)
                    return;

               _entries = new List<IEntryView>(LeaderboardMaxEntriesToShow);

               for(int i = 0; i < LeaderboardMaxEntriesToShow; i++)
                    _entries.Add(Instantiate(_entryPrefab, _entriesContainer));
          }
     }
}
