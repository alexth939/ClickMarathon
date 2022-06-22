using Popups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.WindowViews
{
     [RequireComponent(typeof(CanvasGroup))]
     public sealed class WelcomeWindowView: PopupView, IWelcomeWindowView
     {
          [SerializeField] private Button _registerButton;
          [SerializeField] private Button _signInButton;

          public UnityEvent OnRegisterButtonClicked => _registerButton.onClick;
          public UnityEvent OnSignInButtonClicked => _signInButton.onClick;

          public void UnblockInteraction()
          {
               _registerButton.interactable =
                    _signInButton.interactable = true;
          }

          public void BlockInteraction()
          {
               _registerButton.interactable =
                    _signInButton.interactable = false;
          }

          private void OnDestroy()
          {
               OnRegisterButtonClicked.RemoveAllListeners();
               OnSignInButtonClicked.RemoveAllListeners();
          }
     }
}
