using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runtime.Views;
using UnityEngine;
using FirebaseWorkers;
using static FirebaseWorkers.FirebaseServices;

namespace Runtime
{
     public sealed class MagicPlayButtonPresenter
     {
          private IPlayButtonView _playButton;
          private IPlayTimerView _playTimer;
          private ushort _sessionClickCount;

          public MagicPlayButtonPresenter(
               IPlayButtonView buttonView,
               IPlayTimerView timerView,
               Action<ScoreEntryModel> newScoreReadyHandler)
          {
               _playButton = buttonView;
               _playTimer = timerView;
               CommitScore = newScoreReadyHandler;

               _playButton.OnClick.AddListener(StartButtonClickHandler);
          }

          private Action<ScoreEntryModel> CommitScore;

          private void StartPlaying()
          {
               _sessionClickCount = 0;
               SwitchToPlayState();

               _playTimer.PlayCountdown(onDone: () =>
               {
                    _playButton.OnClick.RemoveListener(ScoreButtonClickHandler);
                    CurrentUserEntry.Score += _sessionClickCount;
                    CommitScore(CurrentUserEntry);
                    _playTimer.PlayCooldown(onDone: () =>
                    {

                         SwitchToIdleState();
                    });
               });
          }

          private void SwitchToPlayState()
          {
               _playButton.OnClick.RemoveListener(StartButtonClickHandler);
               _playButton.OnClick.AddListener(ScoreButtonClickHandler);
               _playButton.SwitchToPlayState(currentScore: CurrentUserEntry.Score);
          }

          private void SwitchToIdleState()
          {
               _playButton.SwitchToIdleState(onDone: () =>
                    _playButton.OnClick.AddListener(StartButtonClickHandler));
          }

          private void StartButtonClickHandler() => StartPlaying();
          private void ScoreButtonClickHandler()
          {
               _sessionClickCount++;
               _playButton.IncreaseScore();
          }
     }
}
