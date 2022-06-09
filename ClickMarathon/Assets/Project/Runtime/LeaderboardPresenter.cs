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
          public LeaderboardPresenter(ILeaderboardView view)
          {

          }

          private void ShowUserEntry()
          {
               //CurrentUserEntry
          }

          public void HandleFoundEntry(ScoreEntryModel scoreEntry)
          {
               if(scoreEntry.ID == CurrentUserEntry.ID)
                    return;
               Debug.Log($"LeaderboardPresenter: entry received:{scoreEntry.ToString()}");
          }
     }
}
