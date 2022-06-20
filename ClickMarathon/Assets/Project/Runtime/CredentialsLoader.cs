using UnityEngine;
using static ProjectDefaults.ProjectConstants;

namespace Runtime
{
     public sealed class CredentialsLoader
     {
          public void Deconstruct(out string email, out string password)
          {
               email = TryLoadLastEmail();
               password = TryLoadLastPassword();
          }

          private string TryLoadLastEmail()
          {
               return PlayerPrefs.GetString(LastUsedEmailKey, defaultValue: string.Empty);
          }

          private string TryLoadLastPassword()
          {
               return PlayerPrefs.GetString(LastUsedPasswordKey, defaultValue: string.Empty);
          }
     }
}
