using System;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWorkers;
using Runtime.Views;
using External.Extensions;
using static ProjectDefaults.ProjectStatics;
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

               UpdateCachedEntry(scoreEntry, out int changedEntryIndex);

               TryMoveEntryToNextPosition(scoreEntry, changedEntryIndex);

               DisplayOnlyActualSegment();
          }

          private void UpdateCachedEntry(ScoreEntryModel scoreEntry, out int entryIndex)
          {
               entryIndex = _allEntries.FindIndex(entry => entry.ID == scoreEntry.ID);
               _allEntries[entryIndex].Score = scoreEntry.Score;
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
               int indexOfGreater = _allEntries.FindUpIndex(entry =>
               {
                    return entry.Score > scoreEntry.Score;
               });
               int lastIndexTillGreaterScore = indexOfGreater == -1 ? 0 : indexOfGreater + 1;

               Debug.Log($"indexOf new entry:{lastIndexTillGreaterScore}");

               _allEntries.Insert(lastIndexTillGreaterScore, scoreEntry);
          }

          // TryMoveToPrevious position is not needed.
          // but remember, dont downgrade entries manually; or create handler.
          private bool TryMoveEntryToNextPosition(ScoreEntryModel scoreEntry, int changedEntryIndex)
          {
               if(changedEntryIndex == 0)
                    return false;

               int indexOfClosestGreaterScore =
                    _allEntries.FindUpIndex(startIndex: changedEntryIndex, entry =>
                    {
                         return entry.Score > scoreEntry.Score;
                    });

               if(indexOfClosestGreaterScore == changedEntryIndex + 1)
                    return false;

               int newEntryIndex = indexOfClosestGreaterScore == -1 ?
                    0 : indexOfClosestGreaterScore + 1;

               Debug.Log($"reposition! lastIndex:{changedEntryIndex}, new:{newEntryIndex}");

               _allEntries.RemoveAt(changedEntryIndex);
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
