using System;

namespace Runtime.Views
{
     public interface IPlayTimerView
     {
          public void PlayCountdown(Action onDone);

          void PlayCooldown(Action onDone);
     }
}
