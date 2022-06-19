using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Popups;

namespace Runtime.WindowViews
{
     [RequireComponent(typeof(CanvasGroup))]
     public sealed class WelcomeWindowView: PopupView, IWelcomeWindowView
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
