using System;
using UnityEngine;
using WindowViews;

namespace Runtime.DependencyContainers
{
     [Serializable]
     public class ConnectingSceneContainer
     {
          public IWelcomeWindowView WelcomeWindow => _welcomeWindow;
          public IRegistrationWindowView RegistrationWindow => _registrationWindow;
          public ILogInWindowView LogInWindow => _logInWindow;

          [SerializeField] private WelcomeWindowView _welcomeWindow;
          [SerializeField] private RegistrationWindowView _registrationWindow;
          [SerializeField] private LogInWindowView _logInWindow;
     }
}
