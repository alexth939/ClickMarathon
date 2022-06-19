using System;

namespace Popups
{
     public interface IPopupView
     {
          void Show(Action onDone = null, float duration = default);
          void Hide(Action onDone = null, float duration = default);
     }
}