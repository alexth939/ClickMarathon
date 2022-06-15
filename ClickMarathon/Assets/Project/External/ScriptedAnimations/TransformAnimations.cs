// version 13.6.22

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using External.UnityEngine.UI.Extensions;

namespace External.ScriptedAnimations
{
     public sealed class TransformAnimations
     {
          public static IEnumerator SetTextInHalfRotation(TextAnimationParams parameters)
          {
               var textTransform = parameters.TransformOfContainer;

               float i = 0;
               float fullDuration = parameters.Duration;
               float halfDuration = fullDuration / 2;

               for(; i < halfDuration; i += Time.deltaTime)
               {
                    float t = Mathf.InverseLerp(0, halfDuration, i);
                    float angle = Mathf.Lerp(0, 90.0f, t);
                    textTransform.rotation = Quaternion.Euler(0, angle, 0);
                    yield return new WaitForEndOfFrame();
               }

               parameters.OnHalfRotation?.Invoke();
               parameters.TextComponent.text = parameters.NewText;

               for(; i < fullDuration; i += Time.deltaTime)
               {
                    float t = Mathf.InverseLerp(halfDuration, fullDuration, i);
                    float angle = 90.0f - Mathf.Lerp(0, 90.0f, t);
                    textTransform.rotation = Quaternion.Euler(0, angle, 0);
                    yield return new WaitForEndOfFrame();
               }

               textTransform.rotation = Quaternion.Euler(Vector3.zero);

               parameters.OnFullRotation?.Invoke();
          }

          public static IEnumerator FadeOutAndScale(TransformAnimationParams parameters)
          {
               for(float i = 0; i < parameters.Duration; i += Time.deltaTime)
               {
                    float t = Mathf.InverseLerp(0, parameters.Duration, i);

                    float multiplier = Mathf.Lerp(1, parameters.EndScale, t);

                    parameters.TargetTransform.localScale = Vector3.one * multiplier;
                    parameters.TargetImage.SetAlpha(1 - t);
                    yield return new WaitForEndOfFrame();
               }

               parameters.OnDone?.Invoke();
          }

          public class TextAnimationParams
          {
               public float Duration = 1.0f;
               public Transform TransformOfContainer = null;
               public TextMeshProUGUI TextComponent = null;
               public string NewText = null;
               public Action OnHalfRotation = null;
               public Action OnFullRotation = null;
          }

          public class TransformAnimationParams
          {
               public float Duration = 1.0f;
               public float EndScale = 2.0f;
               public Transform TargetTransform = null;
               public Image TargetImage;
               public Action OnDone = null;
          }
     }
}
