using FirebaseWorkers;

namespace Runtime
{
     public interface ILeaderboardPresenter
     {
          void DisplayOnlyActualSegment();
          void HandleEntryAdded(ScoreEntryModel scoreEntry);
          void HandleEntryChanged(ScoreEntryModel scoreEntry);
          void HandleEntryFound(ScoreEntryModel scoreEntry);
          void HandleEntryRemoved(ScoreEntryModel scoreEntry);
     }
}