using System;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWorkers;
using static FirebaseWorkers.FirebaseServices;
using static ProjectDefaults.ProjectConstants;

namespace Runtime
{
     public sealed class LeaderboardPresenter: ILeaderboardPresenter
     {
          private ILeaderboardView _leaderboardView;
          private List<ScoreEntryModel> _allEntries;

          public LeaderboardPresenter(ILeaderboardView view)
          {
               _leaderboardView = view;
               _allEntries = new List<ScoreEntryModel>();
          }

          public void DisplayOnlyActualSegment()
          {
               int userIndexInList = _allEntries.FindIndex(
                    entry => entry.ID == CurrentUserEntry.ID);
               int indexToStartTaking = Math.Max(
                    userIndexInList - LeaderboardMaxEntriesBeforeUser, 0);

               int entriesCountAfterUserEntry = _allEntries.Count - userIndexInList - 1;
               int entriesCountToTake = Math.Min(
                    _allEntries.Count - indexToStartTaking, LeaderboardMaxEntriesToShow);

               var entriesToShow = _allEntries.GetRange(indexToStartTaking, entriesCountToTake);
               int positionOfFirstElement = indexToStartTaking + 1;

               InjectPositions(ref entriesToShow, startWithPosition: positionOfFirstElement);
               _leaderboardView.SetEntries(entriesToShow);
          }

          public void HandleEntryFound(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"LeaderboardPresenter: entry found handled:{scoreEntry.ToString()}");

               _allEntries.Insert(0, scoreEntry);
          }

          public void HandleEntryChanged(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"LeaderboardPresenter: entry changed:{scoreEntry.ToString()}");

               if(TryMoveEntryToNextPosition(scoreEntry))
                    DisplayOnlyActualSegment();
          }

          public void HandleEntryAdded(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"LeaderboardPresenter: entry added:{scoreEntry.ToString()}");

               InsertNewEntry(scoreEntry);
               DisplayOnlyActualSegment();
          }

          public void HandleEntryRemoved(ScoreEntryModel scoreEntry)
          {
               Debug.Log($"LeaderboardPresenter: entry removed handled:{scoreEntry.ToString()}");

               int indexOfRemoved = _allEntries.FindIndex(entry => entry.ID == scoreEntry.ID);
               _allEntries.RemoveAt(indexOfRemoved);
               DisplayOnlyActualSegment();
          }

          private void InsertNewEntry(ScoreEntryModel scoreEntry)
          {
               int lastIndexOfList = _allEntries.Count - 1;
               int indexOfBigger = _allEntries.FindUpIndex(lastIndexOfList, entry =>
               {
                    return entry.Score > scoreEntry.Score;
               });
               int lastIndexTillBiggerScore = indexOfBigger == -1 ? 0 : indexOfBigger + 1;

               Debug.Log($"indexOf new entry:{lastIndexTillBiggerScore}");

               _allEntries.Insert(lastIndexTillBiggerScore, scoreEntry);
          }

          // TryMoveToPrevious position is not needed.
          // but remember, dont downgrade entries manually; or create handler.
          private bool TryMoveEntryToNextPosition(ScoreEntryModel scoreEntry)
          {
               int changedEntryOldIndex = _allEntries.FindIndex(entry => entry.ID == scoreEntry.ID);

               if(changedEntryOldIndex == 0)
                    return false;

               int indexOfClosestBiggerScore =
                    _allEntries.FindUpIndex(startIndex: changedEntryOldIndex, entry =>
                    {
                         return entry.Score > scoreEntry.Score;
                    });

               if(indexOfClosestBiggerScore == changedEntryOldIndex + 1)
                    return false;

               int newEntryIndex = indexOfClosestBiggerScore == -1 ?
                    0 : indexOfClosestBiggerScore + 1;

               Debug.Log($"reposition! lastIndex:{changedEntryOldIndex}, new:{newEntryIndex}");

               _allEntries.RemoveAt(changedEntryOldIndex);
               _allEntries.Insert(newEntryIndex, scoreEntry);
               return true;
          }

          private void InjectPositions(ref List<ScoreEntryModel> entries, int startWithPosition)
          {
               foreach(var entry in entries)
                    entry.Position = startWithPosition++;
          }
     }
}
