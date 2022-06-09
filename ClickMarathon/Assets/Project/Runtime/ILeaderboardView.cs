using FirebaseWorkers;
using System.Collections.Generic;

namespace Runtime
{
     public interface ILeaderboardView
     {
          void SetEntries(List<ScoreEntryModel> entriesToShow);
     }
}