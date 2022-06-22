using FirebaseWorkers;

namespace Runtime.Views
{
     public interface IEntryView
     {
          string ID { get; }

          void SetEntry(ScoreEntryModel scoreEntry);

          void Clear();
     }
}