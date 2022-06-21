/// <summary>
/// version 20.6.22
/// </summary>

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Popups
{
     public sealed class DialogPopup: PopupView
     {
          private const string PrefabName = "DialogPopup";

          private static DialogPopup _dialogOnScene;

          [SerializeField] private TextMeshProUGUI _messageField;
          [SerializeField] private Button _okButton;

          private static DialogPopup Dialog
          {
               get
               {
                    _dialogOnScene ??= Instantiate(Resources.Load<DialogPopup>(PrefabName));
                    return _dialogOnScene;
               }
          }

          public static void ShowDialog(string message, Action onOK = null)
          {
               Dialog._messageField.text = message;
               Dialog.Show(onDone: () =>
               {
                    Dialog._okButton.onClick.AddListener(() =>
                    {
                         Dialog._okButton.onClick.RemoveAllListeners();
                         onOK?.Invoke();
                         Dialog.Hide();
                    });
               });
          }

          private void OnDestroy()
          {
               _dialogOnScene = null;
          }
     }
}
