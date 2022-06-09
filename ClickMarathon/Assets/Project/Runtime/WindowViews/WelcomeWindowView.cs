using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WindowViews
{
     [RequireComponent(typeof(CanvasGroup))]
     public class WelcomeWindowView: WindowView, IWelcomeWindowView
     {
          [SerializeField] private Button _registerButton;
          [SerializeField] private Button _signInButton;

          public UnityEvent OnRegisterButtonClicked => _registerButton.onClick;
          public UnityEvent OnSignInButtonClicked => _signInButton.onClick;

          private void OnDestroy()
          {
               OnRegisterButtonClicked.RemoveAllListeners();
               OnSignInButtonClicked.RemoveAllListeners();
          }
     }
}
