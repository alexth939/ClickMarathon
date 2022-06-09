using FirebaseWorkers;
using UnityEngine;
using TMPro;

namespace Runtime
{
     public sealed class EntryView: MonoBehaviour
     {
          [SerializeField] TextMeshProUGUI _nameField;
          [SerializeField] TextMeshProUGUI _scoreField;
          [SerializeField] TextMeshProUGUI _placeField;

          public void SetEntry(ScoreEntryModel scoreEntry)
          {
               gameObject.SetActive(true);

               _nameField.text = scoreEntry.Name;
               _scoreField.text = scoreEntry.Score.ToString();
               _placeField.text = "-999";
          }
     }
}