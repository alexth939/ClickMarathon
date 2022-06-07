using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CommonExtensions
{
     public static bool NotEquals<T>(this T Object, T OtherObject)
     {
          return !Object.Equals(OtherObject);
     }
}
