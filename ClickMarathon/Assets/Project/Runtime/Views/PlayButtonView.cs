using System;
using External.Views;
using UnityEngine;
using UnityEngine.Events;
using static External.ScriptedAnimations.TransformAnimations;
using static ProjectDefaults.ProjectConstants;

namespace Runtime.Views
{
     public sealed class PlayButtonView: MonoBehaviour, IPlayButtonView
     {
          [SerializeField] private WholeButton _playButton;
          private long _currentScore;
          private bool _canDisplayScoreText = false;

          public UnityEvent OnClick => _playButton.OnClick;

          public void IncreaseScore()
          {
               _currentScore++;
               if(_canDisplayScoreText)
                    _playButton.Text = _currentScore.ToString();
          }

          public void SwitchToPlayState(long currentScore)
          {
               _currentScore = currentScore;

               StartCoroutine(SetTextInHalfRotation(new TextAnimationParams()
               {
                    Duration = PlayButtonTextSwitchDuration,
                    NewText = currentScore.ToString(),
                    TextComponent = _playButton.TextComponent,
                    TransformOfContainer = _playButton.TextGameObject.transform,
                    OnHalfRotation = () => _canDisplayScoreText = true
               }));
          }

          public void SwitchToIdleState(Action onDone)
          {
               _canDisplayScoreText = false;
               StartCoroutine(SetTextInHalfRotation(new TextAnimationParams()
               {
                    Duration = PlayButtonTextSwitchDuration,
                    NewText = "Start",
                    TextComponent = _playButton.TextComponent,
                    TransformOfContainer = _playButton.TextGameObject.transform,
                    OnFullRotation = onDone
               }));
          }
     }
}
