using System;
using UnityEngine;
using WindowViews;

namespace Runtime.DependencyContainers
{
     [Serializable]
     public sealed class ConnectingSceneContainer
     {
          public IWelcomeWindowView WelcomeWindow => _welcomeWindow;
          public IRegistrationWindowView RegistrationWindow => _registrationWindow;
          public IAuthorizationWindowView AuthorizationWindow => _authorizationWindow;

          [SerializeField] private WelcomeWindowView _welcomeWindow;
          [SerializeField] private RegistrationWindowView _registrationWindow;
          [SerializeField] private AuthorizationWindowView _authorizationWindow;
     }
}
