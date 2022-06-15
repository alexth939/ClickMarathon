using System;
using UnityEngine.Events;

namespace Runtime.Views
{
     public interface IPlayButtonView
     {
          UnityEvent OnClick { get; }
          public void IncreaseScore();
          void SwitchToPlayState(long currentScore);
          void SwitchToIdleState(Action onDone);
     }
}
