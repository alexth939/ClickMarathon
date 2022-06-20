using UnityEngine;
using static ProjectDefaults.ProjectConstants;

namespace Runtime
{
     public sealed class CredentialsSaver
     {
          public static void RememberMe(string email, string password)
          {
               PlayerPrefs.SetString(LastUsedEmailKey, email);
               PlayerPrefs.SetString(LastUsedPasswordKey, password);
          }
     }
}
