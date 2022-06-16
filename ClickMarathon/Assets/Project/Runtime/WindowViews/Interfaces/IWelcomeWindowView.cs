﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace WindowViews
{
     public interface IWelcomeWindowView: IWindowView
     {
          UnityEvent OnRegisterButtonClicked { get; }
          UnityEvent OnSignInButtonClicked { get; }

          void CleatAllListeners();
     }
}
