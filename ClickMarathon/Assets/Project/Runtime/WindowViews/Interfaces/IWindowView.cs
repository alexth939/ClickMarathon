using System;
using System.Collections;

namespace WindowViews
{
     public interface IWindowView
     {
          IEnumerator Hide(Action onDone = null);
          IEnumerator Show(Action onDone = null);
     }
}