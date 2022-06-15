using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using External.UnityEngine.UI.Extensions;
using static ProjectDefaults.ProjectConstants;
using static External.ScriptedAnimations.TransformAnimations;

namespace Runtime.Views
{
     public sealed class PlayTimerView: MonoBehaviour, IPlayTimerView
     {
          [SerializeField] GameObject _timerGameObject;
          [SerializeField] Image _timerImage;
          private bool _isTimerRunning = false;

          public void PlayCooldown(Action onDone)
          {
               var timerTransform = _timerGameObject.transform;
               _timerImage.fillAmount = 1.0f;
               _timerGameObject.SetActive(true);

               StartCoroutine(FadeOutAndScale(new TransformAnimationParams()
               {
                    Duration = 2.0f,
                    EndScale = 3.0f,
                    TargetTransform = timerTransform,
                    TargetImage = _timerImage,
                    OnDone = () =>
                    {
                         _timerGameObject.SetActive(false);
                         _timerImage.SetAlpha(1.0f);
                         timerTransform.localScale = Vector3.one;
                         onDone.Invoke();
                    }
               }));
          }

          public void PlayCountdown(Action onDone)
          {
               if(_isTimerRunning)
               {
                    Debug.LogWarning($"U tryin to start timer while it's already running!");
                    return;
               }

               _timerGameObject.gameObject.SetActive(true);
               StartCoroutine(StartTimer(onDone));
          }

          private IEnumerator StartTimer(Action onDone)
          {
               _isTimerRunning = true;
               float duration = InteractionDurationOnPlay;

               for(float i = duration; i > 0; i -= Time.deltaTime)
               {
                    float t = Mathf.InverseLerp(0, duration, i);
                    _timerImage.fillAmount = t;
                    yield return new WaitForEndOfFrame();
               }

               _timerImage.fillAmount = 0;
               _isTimerRunning = false;
               _timerGameObject.gameObject.SetActive(false);
               onDone.Invoke();
          }
     }
}
