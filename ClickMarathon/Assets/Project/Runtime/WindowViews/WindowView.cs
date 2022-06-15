using System;
using System.Collections;
using UnityEngine;
using static ProjectDefaults.ProjectStrings;
using static ProjectDefaults.ProjectConstants;

namespace WindowViews
{
     [RequireComponent(typeof(CanvasGroup))]
     public abstract class WindowView: MonoBehaviour, IWindowView
     {
          [SerializeField] private CanvasGroup _alphaLerper;
          private bool _isTransitting = false;

          public IEnumerator Show(Action onDone = null)
          {
               #region Exception handlers
               if(_isTransitting)
               {
#if UNITY_EDITOR
                    UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                    Debug.LogError(WindowBusyMessage);
                    yield break;
               }

               if(IsNotHidden())
               {
#if UNITY_EDITOR
                    UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                    Debug.LogError(WindowNotHiddenMessage);
                    yield break;
               }
               #endregion

               float duration = WindowFadeInDuration;
               PrepareToShow();

               for(float i = 0; i < duration; i += Time.deltaTime)
               {
                    yield return new WaitForEndOfFrame();
                    float t = Mathf.InverseLerp(0, duration, i);
                    _alphaLerper.alpha = t;
               }

               FinalizeShow();

               #region Nested methods
               bool IsNotHidden() => gameObject.activeSelf;
               void PrepareToShow()
               {
                    _alphaLerper.alpha = 0;
                    gameObject.SetActive(true);
                    _isTransitting = true;
               }

               void FinalizeShow()
               {
                    _alphaLerper.alpha = 1;
                    _isTransitting = false;
                    onDone?.Invoke();
               }
               #endregion
          }

          public IEnumerator Hide(Action onDone = null)
          {
               #region Exception handlers
               if(_isTransitting)
               {
#if UNITY_EDITOR
                    UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                    Debug.LogError(WindowBusyMessage);
                    yield break;
               }

               if(IsNotCompletelyRevealed())
               {
#if UNITY_EDITOR
                    UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                    Debug.LogError(WindowNotRevealedMessage);
                    yield break;
               }
               #endregion

               float duration = WindowFadeOutDuration;
               PrepareHide();

               for(float i = duration; i > 0; i -= Time.deltaTime)
               {
                    yield return new WaitForEndOfFrame();
                    float t = Mathf.InverseLerp(0, duration, i);
                    _alphaLerper.alpha = t;
               }

               FinalizeHide();

               #region Nested methods
               bool IsNotCompletelyRevealed() =>
                    gameObject.activeSelf == false
                    || _alphaLerper.alpha < AlmostOne;

               void PrepareHide()
               {
                    _alphaLerper.alpha = 1;
                    _isTransitting = true;
               }

               void FinalizeHide()
               {
                    _alphaLerper.alpha = 0;
                    gameObject.SetActive(false);
                    _isTransitting = false;
                    onDone?.Invoke();
               }
               #endregion
          }
     }
}
