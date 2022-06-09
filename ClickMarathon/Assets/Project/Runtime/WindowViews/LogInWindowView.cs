using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace WindowViews
{
     class LogInWindowView: WindowView, ILogInWindowView
     {
          [SerializeField] private Button _logInButon;
          [SerializeField] private TMP_InputField _emailInputField;
          [SerializeField] private TMP_InputField _passwordInputField;

          public UnityEvent OnLogInButtonClicked => _logInButon.onClick;

#if UNITY_EDITOR
          private void Start()
          {
               _emailInputField.text = "tester@tester.com";
               _passwordInputField.text = "tester";
          }
#endif

          public string GetEmail() => _emailInputField.text;
          public string GetPassword() => _passwordInputField.text;
          public void UnlockInteraction() => _logInButon.interactable = true;
          public void LockInteraction() => _logInButon.interactable = false;

          // todo implement me
          public void PlayConnectingAnimation()
          {
               throw new NotImplementedException();
          }
          public void StopConnectingAnimation()
          {
               throw new NotImplementedException();
          }
     }
}
