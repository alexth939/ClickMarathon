using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace WindowViews
{
     public sealed class AuthorizationWindowView: WindowView, IAuthorizationWindowView
     {
          [SerializeField] private TMP_InputField _emailInputField;
          [SerializeField] private TMP_InputField _passwordInputField;
          [SerializeField] private Button _signInButon;

          public UnityEvent OnAuthorizeRequest => _signInButon.onClick;

#if UNITY_EDITOR
          private void Start()
          {
               _emailInputField.text = "tester@tester.com";
               _passwordInputField.text = "tester";
          }
#endif

          public string GetEmail() => _emailInputField.text;
          public string GetPassword() => _passwordInputField.text;
          public void UnblockInteraction() => _signInButon.interactable = true;
          public void BlockInteraction() => _signInButon.interactable = false;

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
