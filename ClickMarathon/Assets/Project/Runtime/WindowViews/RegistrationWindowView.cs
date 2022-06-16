using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace WindowViews
{
     public sealed class RegistrationWindowView: WindowView, IRegistrationWindowView
     {
          [SerializeField] private TMP_InputField _nickNameInputField;
          [SerializeField] private TMP_InputField _emailInputField;
          [SerializeField] private TMP_InputField _passwordInputField;
          [SerializeField] private Button _registerButton;

          public UnityEvent OnRegisterRequest => _registerButton.onClick;

          public string GetEmail() => _emailInputField.text;
          public string GetPassword() => _passwordInputField.text;
          public void UnlockInteraction() => _registerButton.interactable = true;
          public void LockInteraction() => _registerButton.interactable = false;
     }
}
