/// <summary>
/// version 20.6.22
/// </summary>

using System;
using System.Collections;
using UnityEngine;

namespace Popups
{
     [RequireComponent(typeof(CanvasGroup))]
     public abstract class PopupView: MonoBehaviour, IPopupView
     {
          private const float DefaultShowDuration = 0.5f;
          private const float DefaultHideDuration = 0.5f;
          private const float AlmostOne = 0.99f;

          private const string WindowBusyMessage = "U tryin to use busy window.";
          private const string WindowNotRevealedMessage = "U tryin to Hide() not completely revealed window.";
          private const string WindowNotHiddenMessage = "U tryin to Show() not completely hidden window.";

          [SerializeField] private CanvasGroup _alphaLerper;
          private bool _isTransitting = false;

          private bool IsNotReadyToShow
          {
               get
               {
                    if(_isTransitting)
                    {
#if UNITY_EDITOR
                         UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                         Debug.LogError(WindowBusyMessage);
                         return true;
                    }

                    if(IsNotCompletelyHidden)
                    {
#if UNITY_EDITOR
                         UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                         Debug.LogError(WindowNotHiddenMessage);
                         return true;
                    }

                    return false;
               }
          }

          private bool IsNotReadyToHide
          {
               get
               {
                    if(_isTransitting)
                    {
#if UNITY_EDITOR
                         UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                         Debug.LogError(WindowBusyMessage);
                         return true;
                    }

                    if(IsNotCompletelyRevealed)
                    {
#if UNITY_EDITOR
                         UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                         Debug.LogError(WindowNotRevealedMessage);
                         return true;
                    }

                    return false;
               }
          }

          private bool IsNotCompletelyHidden => gameObject.activeSelf;

          private bool IsNotCompletelyRevealed =>
               gameObject.activeSelf == false || _alphaLerper.alpha < AlmostOne;

          public void Show(float? duration = null, Action onDone = null)
          {
               if(IsNotReadyToShow)
                    return;

               duration ??= DefaultShowDuration;

               SetupObjectToShow();
               StartCoroutine(ShowWindow(duration.Value, onDone: () =>
               {
                    FinalizeShow();
                    onDone?.Invoke();
               }));
          }

          public void Hide(float? duration = null, Action onDone = null)
          {
               if(IsNotReadyToHide)
                    return;

               duration ??= DefaultHideDuration;

               SetupObjectToHide();
               StartCoroutine(HideWindow(duration.Value, onDone: () =>
               {
                    FinalizeHide();
                    onDone?.Invoke();
               }));
          }

          private void SetupObjectToShow()
          {
               _alphaLerper.alpha = 0;
               gameObject.SetActive(true);
               _isTransitting = true;
          }

          private void SetupObjectToHide()
          {
               _alphaLerper.alpha = 1;
               _isTransitting = true;
          }

          private void FinalizeShow()
          {
               _alphaLerper.alpha = 1;
               _isTransitting = false;
          }

          private void FinalizeHide()
          {
               _alphaLerper.alpha = 0;
               gameObject.SetActive(false);
               _isTransitting = false;
          }

          private IEnumerator ShowWindow(float duration, Action onDone)
          {
               for(float i = 0; i < duration; i += Time.deltaTime)
               {
                    yield return new WaitForEndOfFrame();
                    float t = Mathf.InverseLerp(0, duration, i);
                    _alphaLerper.alpha = t;
               }

               onDone.Invoke();
          }

          private IEnumerator HideWindow(float duration, Action onDone)
          {
               for(float i = duration; i > 0; i -= Time.deltaTime)
               {
                    yield return new WaitForEndOfFrame();
                    float t = Mathf.InverseLerp(0, duration, i);
                    _alphaLerper.alpha = t;
               }

               onDone.Invoke();
          }
     }
}
