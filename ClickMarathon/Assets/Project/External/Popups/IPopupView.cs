/// <summary>
/// version 20.6.22
/// </summary>

using System;

namespace Popups
{
     public interface IPopupView
     {
          void Show(float? duration = null, Action onDone = null);

          void Hide(float? duration = null, Action onDone = null);
     }
}