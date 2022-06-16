using UnityEngine.Events;

namespace WindowViews
{
     public interface IAuthorizationWindowView: IWindowView
     {
          string GetEmail();
          string GetPassword();
          void UnblockInteraction();
          void BlockInteraction();

          UnityEvent OnAuthorizeRequest { get; }
     }
}