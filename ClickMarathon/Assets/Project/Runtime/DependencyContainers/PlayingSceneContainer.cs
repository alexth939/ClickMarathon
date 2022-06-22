using System;
using Runtime.Views;
using Runtime.WindowViews;
using UnityEngine;

namespace Runtime.DependencyContainers
{
     [Serializable]
     public sealed class PlayingSceneContainer
     {
          public IConnectingWindowView ConnectingWindow => _connectingWindow;
          public IGameWindowView GameWindowView => _gameWindowView;
          public ILeaderboardView LeaderboardView => _leaderboardView;

          public IPlayButtonView PlayButtonView => _playButtonView;
          public IPlayTimerView PlayTimerView => _playTimerView;
          public ITransitionsView TransitionsView => _transitionsView;

          [SerializeField] private ConnectingWindowView _connectingWindow;
          [SerializeField] private GameWindowView _gameWindowView;
          [SerializeField] private LeaderboardView _leaderboardView;

          [SerializeField] private PlayButtonView _playButtonView;
          [SerializeField] private PlayTimerView _playTimerView;
          [SerializeField] private TransitionsView _transitionsView;
     }
}
