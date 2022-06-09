using UnityEngine.Events;

namespace WindowViews
{
     public interface ILogInWindowView: IWindowView
     {
          string GetEmail();
          string GetPassword();
          void UnlockInteraction();
          void LockInteraction();

          UnityEvent OnLogInButtonClicked { get; }
     }
}