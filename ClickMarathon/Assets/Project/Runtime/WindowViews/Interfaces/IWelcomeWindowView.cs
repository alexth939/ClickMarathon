using UnityEngine.Events;
using Popups;

namespace Runtime.WindowViews
{
     public interface IWelcomeWindowView: IPopupView
     {
          UnityEvent OnRegisterButtonClicked { get; }
          UnityEvent OnSignInButtonClicked { get; }

          void CleatAllListeners();
     }
}
