/// <summary>
/// version 17.6.22
/// </summary>

using System;

namespace External.Extensions
{
     public static class ExceptionExtensions
     {
          public static Exception GetLastInner(this Exception topNode)
          {
               if(topNode.InnerException == null)
                    return topNode;

               return GetLastInner(topNode.InnerException);
          }
     }
}
