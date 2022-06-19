using UnityEngine.Events;
using Popups;

namespace Runtime.WindowViews
{
     public interface IAuthorizationWindowView: IPopupView
     {
          string GetEmail();
          string GetPassword();
          void UnblockInteraction();
          void BlockInteraction();

          UnityEvent OnAuthorizeRequest { get; }
     }
}