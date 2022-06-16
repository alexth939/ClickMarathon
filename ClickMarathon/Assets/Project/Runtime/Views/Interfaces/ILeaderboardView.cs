using FirebaseWorkers;
using System.Collections.Generic;

namespace Runtime.Views
{
     public interface ILeaderboardView
     {
          void SetEntries(List<ScoreEntryModel> entriesToShow);
     }
}