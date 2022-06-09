using System.Collections.Generic;
using UnityEngine;
using FirebaseWorkers;

namespace Runtime
{
     public class LeaderboardView: MonoBehaviour, ILeaderboardView
     {
          [SerializeField] private List<EntryView> _entries;

          public void SetEntries(List<ScoreEntryModel> entriesToShow)
          {
               for(int i = 0; i < entriesToShow.Count; i++)
               {
                    _entries[i].SetEntry(entriesToShow[i]);
               }
          }
     }
}
