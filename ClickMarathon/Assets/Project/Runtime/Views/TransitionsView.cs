using System;
using UnityEditor;
using UnityEngine;

namespace Runtime.Views
{
     [RequireComponent(typeof(Animator))]
     public sealed class TransitionsView: MonoBehaviour, ITransitionsView
     {
          private const string AnimatorFadeInKey = "Entering";
          private const string AnimatorFadeOutKey = "Exiting";

          [SerializeField] private Animator _animator;
          private ContentState _currentState = default;

          private event Action OnFadeInDone;

          private event Action OnFadeOutDone;

          private enum ContentState
          {
               Hidden,
               Visible
          }

          private bool IsHidden
          {
               get
               {
                    if(_currentState == ContentState.Hidden)
                    {
#if UNITY_EDITOR
                         EditorGUIUtility.PingObject(this);
#endif
                         Debug.LogError($"Already hidden.");
                         return true;
                    }

                    return false;
               }
          }

          private bool IsVisible
          {
               get
               {
                    if(_currentState == ContentState.Visible)
                    {
#if UNITY_EDITOR
                         EditorGUIUtility.PingObject(this);
#endif
                         Debug.LogError($"Already visible.");
                         return true;
                    }

                    return false;
               }
          }

          /// <summary>
          /// Don't forget to add an animation event key:"CompleteFadeIn()" to your animation.
          /// </summary>
          public void FadeInAsync(Action onDone = null)
          {
               if(IsVisible)
                    return;

               OnFadeInDone = () =>
               {
                    onDone?.Invoke();
                    OnFadeInDone = null;
               };

               _animator.SetBool(AnimatorFadeInKey, true);
          }

          /// <summary>
          /// Don't forget to add an animation event key:"CompleteFadeOut()" to your animation.
          /// </summary>
          public void FadeOutAsync(Action onDone = null)
          {
               if(IsHidden)
                    return;

               OnFadeOutDone = () =>
               {
                    onDone?.Invoke();
                    OnFadeOutDone = null;
               };

               _animator.SetBool(AnimatorFadeOutKey, true);
          }

          public void CompleteFadeIn()
          {
               if(IsVisible)
                    return;

               _currentState = ContentState.Visible;
               OnFadeInDone?.Invoke();
          }

          public void CompleteFadeOut()
          {
               if(IsHidden)
                    return;

               _currentState = ContentState.Hidden;
               OnFadeOutDone?.Invoke();
          }
     }
}
