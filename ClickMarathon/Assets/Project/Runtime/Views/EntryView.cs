using FirebaseWorkers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ProjectDefaults.ProjectStatics;

namespace Runtime.Views
{
     public sealed class EntryView: MonoBehaviour, IEntryView
     {
          private const string SelectedColorRGB = "#ABFF96";
          private const string UnselectedColorRGB = "#E4E5FF";

          [SerializeField] private TextMeshProUGUI _nameField;
          [SerializeField] private TextMeshProUGUI _scoreField;
          [SerializeField] private TextMeshProUGUI _placeField;
          [SerializeField] private GameObject _visualContentContainer;
          [SerializeField] private Image _backgroundImage;

          public string ID { get; private set; }

          public void Clear()
          {
               _visualContentContainer.SetActive(false);
               ID = null;
               _nameField.text = string.Empty;
               _scoreField.text = string.Empty;
               _placeField.text = string.Empty;
               Unselect();
          }

          public void SetEntry(ScoreEntryModel scoreEntry)
          {
               _visualContentContainer.SetActive(true);

               ID = scoreEntry.ID;
               _nameField.text = scoreEntry.Name;
               _scoreField.text = scoreEntry.Score.ToString();
               _placeField.text = scoreEntry.Position.ToString();

               if(scoreEntry.ID == CachedScoreEntry.ID)
                    Select();
               else
                    Unselect();
          }

          private void Select()
          {
               ColorUtility.TryParseHtmlString(SelectedColorRGB, out var color);
               _backgroundImage.color = color;
          }
          private void Unselect()
          {
               ColorUtility.TryParseHtmlString(UnselectedColorRGB, out var color);
               _backgroundImage.color = color;
          }
     }
}