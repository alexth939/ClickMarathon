// version 12.6.2022

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace External.Views
{
     [Serializable]
     public struct WholeButton
     {
          public GameObject ButtonGameObject;
          public GameObject TextGameObject;
          public TextMeshProUGUI TextComponent;
          public Button ButtonComponent;

          public string Text
          {
               get => TextComponent.text;
               set => TextComponent.text = value;
          }

          public UnityEvent OnClick => ButtonComponent.onClick;

          public void ClearOnClickListeners() => OnClick.RemoveAllListeners();

          public void Show() => ButtonGameObject.SetActive(true);
          public void Hide() => ButtonGameObject.SetActive(false);
     }
}
