// version 15.6.2022

using System;
using System.Collections.Generic;

namespace External.Extensions
{
     public static class CommonExtensions
     {
          public static bool NotEquals<T>(this T Object, T OtherObject)
          {
               return !Object.Equals(OtherObject);
          }

          public static int FindUpIndex<T>(this List<T> collection, Predicate<T> match)
          {
               int startIndex = collection.Count - 1;
               return FindUpIndex(collection, startIndex, match);
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
}
