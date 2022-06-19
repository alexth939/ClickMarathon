using System;
using UnityEngine;
using Runtime.Views;
using Runtime.WindowViews;

namespace Runtime.DependencyContainers
{
     [Serializable]
     public sealed class ConnectingSceneContainer
     {
          public TransitionsView TransitionsView;
          public IWelcomeWindowView WelcomeWindow => _welcomeWindow;
          public IRegistrationWindowView RegistrationWindow => _registrationWindow;
          public IAuthorizationWindowView AuthorizationWindow => _authorizationWindow;

          [SerializeField] private WelcomeWindowView _welcomeWindow;
          [SerializeField] private RegistrationWindowView _registrationWindow;
          [SerializeField] private AuthorizationWindowView _authorizationWindow;
     }
}
