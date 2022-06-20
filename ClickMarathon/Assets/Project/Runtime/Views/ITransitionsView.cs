using System;

namespace Runtime.Views
{
     public interface ITransitionsView
     {
          void FadeInAsync(Action onDone = null);
          void FadeOutAsync(Action onDone = null);
     }
}
