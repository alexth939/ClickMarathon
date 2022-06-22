using System;
using Runtime.Views;
using Runtime.WindowViews;
using UnityEngine;

namespace Runtime.DependencyContainers
{
     [Serializable]
     public sealed class ConnectingSceneContainer
     {
          public ITransitionsView TransitionsView => _transitionsView;
          public IWelcomeWindowView WelcomeWindow => _welcomeWindow;
          public IRegistrationWindowView RegistrationWindow => _registrationWindow;
          public IAuthorizationWindowView AuthorizationWindow => _authorizationWindow;

          [SerializeField] private WelcomeWindowView _welcomeWindow;
          [SerializeField] private RegistrationWindowView _registrationWindow;
          [SerializeField] private AuthorizationWindowView _authorizationWindow;
          [SerializeField] private TransitionsView _transitionsView;
     }
}
