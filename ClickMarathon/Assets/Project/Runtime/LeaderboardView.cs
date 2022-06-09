using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WindowViews;

namespace Runtime
{
     public class LeaderboardView: MonoBehaviour, ILeaderboardView
     {
          private LeaderboardView _leaderboardView;

          public LeaderboardView(LeaderboardView view)
          {
               _leaderboardView = view;
          }
     }
}
