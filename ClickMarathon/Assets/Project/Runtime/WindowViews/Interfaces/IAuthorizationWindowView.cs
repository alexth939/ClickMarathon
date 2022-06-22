using Popups;
using UnityEngine.Events;

namespace Runtime.WindowViews
{
     public interface IAuthorizationWindowView: IPopupView
     {
          string GetEmail();

          string GetPassword();

          void UnblockInteraction();

          void BlockInteraction();

          UnityEvent OnGoBackRequest { get; }
          UnityEvent OnAuthorizeRequest { get; }
     }
}