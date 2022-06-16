using UnityEngine.Events;

namespace WindowViews
{
     public interface IRegistrationWindowView: IWindowView
     {
          UnityEvent OnRegisterRequest { get; }

          void BlockInteraction();
          void UnblockInteraction();
          string GetNickname();
          string GetEmail();
          string GetPassword();
     }
}