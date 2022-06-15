using FirebaseWorkers;

namespace Runtime
{
     public interface IEntryView
     {
          string ID { get; }

          void SetEntry(ScoreEntryModel scoreEntry);
          void Clear();
     }
}