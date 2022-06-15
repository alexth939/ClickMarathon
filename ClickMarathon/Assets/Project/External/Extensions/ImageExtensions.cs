// version 14.6.22

using UnityEngine;
using UnityEngine.UI;

namespace External.UnityEngine.UI.Extensions
{
     public static class ImageExtensions
     {
          public static void SetAlpha(this Image component, float alpha)
          {
               var originColor = component.color;
               component.color = new Color()
               {
                    r = originColor.r,
                    g = originColor.g,
                    b = originColor.b,
                    a = alpha
               };
          }
     }
}
