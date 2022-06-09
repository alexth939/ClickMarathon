using System.Collections.Generic;
using Firebase.Database;
using static FirebaseWorkers.FirebaseCustomApi;
using static ProjectDefaults.ProjectConstants;

namespace Runtime
{
     public sealed class DashboardBuilder
     {
          private List<PlayerEntry> _allEntries;
          private PlayerEntry _userEntry;
          private string _userID;
          private string _userName;
          private int _userEntryIndex = -1;

          public DashboardBuilder()
          {
               _allEntries = new List<PlayerEntry>();

               TryGetCachedUser(out var user);
               _userID = user.UserId;
               _userName = user.DisplayName;
          }

          public List<PlayerEntry> BuildTable()
          {
               Query getAll;
               (getAll = DashboardRef.OrderByChild(DashboardScoreKey)).ChildAdded += OnChildAdded;
               getAll.ChildAdded -= OnChildAdded;
               return _allEntries;
          }

          private void OnChildAdded(object sender, ChildChangedEventArgs e)
          {
               _allEntries.Add(new PlayerEntry()
               {
                    ID = e.Snapshot.Key,
                    name = (string)e.Snapshot.Child(DashboardNameKey).Value,
                    score = (int)e.Snapshot.Child(DashboardScoreKey).Value
               });
          }

          public sealed class PlayerEntry
          {
               public string ID;
               public string name;
               public int score;
          }
     }
}
