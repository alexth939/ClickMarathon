using UnityEngine.Events;
using Popups;

namespace Runtime.WindowViews
{
     public interface IRegistrationWindowView: IPopupView
     {
          UnityEvent OnGoBackRequest { get; }
          UnityEvent OnRegisterRequest { get; }

          void BlockInteraction();
          void UnblockInteraction();
          string GetNickname();
          string GetEmail();
          string GetPassword();
     }
}