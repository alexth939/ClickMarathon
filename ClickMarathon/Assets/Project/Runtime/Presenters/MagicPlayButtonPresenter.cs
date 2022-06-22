using System;
using FirebaseWorkers;
using Runtime.Views;
using static ProjectDefaults.ProjectStatics;

namespace Runtime
{
     public sealed class MagicPlayButtonPresenter
     {
          private readonly IPlayButtonView _playButton;
          private readonly IPlayTimerView _playTimer;
          private ushort _sessionClickCount;

          public MagicPlayButtonPresenter(
               IPlayButtonView buttonView,
               IPlayTimerView timerView,
               Action<ScoreEntryModel> newScoreAchievedHandler)
          {
               _playButton = buttonView;
               _playTimer = timerView;
               CommitScore = newScoreAchievedHandler;

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
                    CachedScoreEntry.Score += _sessionClickCount;
                    CommitScore(CachedScoreEntry);
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
               _playButton.SwitchToPlayState(currentScore: CachedScoreEntry.Score);
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
