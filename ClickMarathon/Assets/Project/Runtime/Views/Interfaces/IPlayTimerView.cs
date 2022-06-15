using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime.Views
{
     public interface IPlayTimerView
     {
          public void PlayCountdown(Action onDone);
          void PlayCooldown(Action onDone);
     }
}
