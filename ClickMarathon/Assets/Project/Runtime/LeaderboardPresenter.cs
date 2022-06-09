using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseWorkers;
using static FirebaseWorkers.FirebaseServices;
using UnityEngine;

namespace Runtime
{
     public sealed class LeaderboardPresenter
     {
          private const int MaxEntriesBeforeUser = 3;
          private const int EntryCountOnView = 8;

          private ILeaderboardView _leaderboardView;
          private List<ScoreEntryModel> _allEntries;

          public LeaderboardPresenter(ILeaderboardView view)
          {
               _leaderboardView = view;
               _allEntries = new List<ScoreEntryModel>();
          }

          public void DisplayFilteredResults()
          {
               int userIndexInList = _allEntries.FindIndex(
                    entry => entry.ID == CurrentUserEntry.ID);
               int indexToStartTaking = Math.Max(userIndexInList - MaxEntriesBeforeUser, 0);

               int entriesCountAfterUserEntry = _allEntries.Count - userIndexInList - 1;
               int entriesCountToTake = Math.Min(
                    _allEntries.Count - indexToStartTaking, EntryCountOnView);

               var entriesToShow = _allEntries.GetRange(indexToStartTaking, entriesCountToTake);
               _leaderboardView.SetEntries(entriesToShow);

               _allEntries = null;
          }

          public void HandleFoundEntry(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"LeaderboardPresenter: entry received:{scoreEntry.ToString()}");

               _allEntries.Insert(0,scoreEntry);
          }
     }
}
