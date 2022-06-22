/// <summary>
/// version 20.6.22
/// </summary>

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
     public sealed class DialogPopup: PopupView
     {
          private const string DialogPopupPrefabName = "DialogPopup";
          private const string EventSystemPrefabName = "EventSystem";

          private static DialogPopup _dialogOnScene;

          [SerializeField] private TextMeshProUGUI _messageField;
          [SerializeField] private Button _okButton;

          private static DialogPopup Dialog
          {
               get
               {
                    if(IsSceneHasEventSystem() == false)
                         SpawnEventSystem();

                    _dialogOnScene ??= Instantiate(Resources.Load<DialogPopup>(DialogPopupPrefabName));
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

          private static bool IsSceneHasEventSystem()
          {
               bool found = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
               return found;
          }

          private static void SpawnEventSystem()
          {
               Instantiate(Resources.Load(EventSystemPrefabName));
          }

          private void OnDestroy()
          {
               _dialogOnScene = null;
          }
     }
}
