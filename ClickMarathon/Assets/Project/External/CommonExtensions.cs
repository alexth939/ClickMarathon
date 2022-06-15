// version 6.4.2022

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

     public static int FindUpIndex<T>(this List<T> collection, int startIndex, Predicate<T> match)
     {
          for(int i = startIndex; i >= 0; i--)
          {
               if(match(collection[i]))
               {
                    return i;
               }
          }
          return -1;
     }
}
