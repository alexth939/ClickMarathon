using Popups;
using UnityEngine.Events;

namespace Runtime.WindowViews
{
     public interface IWelcomeWindowView: IPopupView
     {
          UnityEvent OnRegisterButtonClicked { get; }
          UnityEvent OnSignInButtonClicked { get; }

          void UnblockInteraction();

          void BlockInteraction();
     }
}
