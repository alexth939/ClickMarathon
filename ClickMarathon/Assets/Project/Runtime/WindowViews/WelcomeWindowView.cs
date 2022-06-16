using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WindowViews
{
     [RequireComponent(typeof(CanvasGroup))]
     public sealed class WelcomeWindowView: WindowView, IWelcomeWindowView
     {
          [SerializeField] private Button _registerButton;
          [SerializeField] private Button _signInButton;

          public UnityEvent OnRegisterButtonClicked => _registerButton.onClick;
          public UnityEvent OnSignInButtonClicked => _signInButton.onClick;

          public void CleatAllListeners()
          {
               OnRegisterButtonClicked.RemoveAllListeners();
               OnSignInButtonClicked.RemoveAllListeners();
          }

          private void OnDestroy()
          {
               OnRegisterButtonClicked.RemoveAllListeners();
               OnSignInButtonClicked.RemoveAllListeners();
          }
     }
}
