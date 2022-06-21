using System;
using Firebase.Database;
using UnityEngine;
using static ProjectDefaults.ProjectConstants;

namespace FirebaseWorkers
{
     public sealed class ScoreEntryModel
     {
          private const long DefaultScore = 0;
          private const int DefaultPosition = -1;

          private ScoreEntryModel() { }

          public ScoreEntryModel(string id, string name)
          {
               this.ID = id;
               this.Name = name;
               this.Score = DefaultScore;
               this.Position = DefaultPosition;
          }

          // change to {get,init}, when it will be avaliable.
          public string ID { get; private set; }
          public string Name { get; private set; }
          public long Score { get; set; }
          public int Position { get; set; }

          public ScoreEntryFields Fields => new ScoreEntryFields()
          {
               Name = this.Name,
               Score = this.Score
          };

          public static explicit operator ScoreEntryModel(DataSnapshot entrySnapshot)
          {
               try
               {
                    return new ScoreEntryModel()
                    {
                         ID = entrySnapshot.Key,
                         Name = (string)entrySnapshot.Child(DashboardNameKey).Value,
                         Score = (long)entrySnapshot.Child(DashboardScoreKey).Value
                    };
               }
               catch(Exception ex)
               {
                    Debug.LogError($"{ex.Message}");
                    return null;
               }
          }

          public new string ToString()
          {
               return $"(ID:{ID}),(Name:{Name}),(Score:{Score})";
          }

          [Serializable]
          public class ScoreEntryFields
          {
               public string Name;
               public long Score;
          }
     }
}
