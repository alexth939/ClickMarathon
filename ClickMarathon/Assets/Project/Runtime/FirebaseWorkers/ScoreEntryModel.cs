using System;
using Firebase.Database;
using UnityEngine;
using static ProjectDefaults.ProjectConstants;
using FirebaseApi = FirebaseWorkers.FirebaseCustomApi;

namespace FirebaseWorkers
{
     public sealed class ScoreEntryModel
     {
          public string ID;
          public string Name;
          public long Score;
          public int Position;

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

          public static ScoreEntryModel GenerateDefault()
          {
               Debug.Log($"generate default()");

               FirebaseApi.TryGetCachedUser(out var user);

               return new ScoreEntryModel()
               {
                    ID = user.UserId,
                    Name = user.DisplayName,
                    Score = 0
               };
          }

          public override string ToString()
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
