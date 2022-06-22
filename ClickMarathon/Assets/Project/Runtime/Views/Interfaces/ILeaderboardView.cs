using System.Collections.Generic;
using FirebaseWorkers;

namespace Runtime.Views
{
     public interface ILeaderboardView
     {
          void SetEntries(List<ScoreEntryModel> entriesToShow);
     }
}