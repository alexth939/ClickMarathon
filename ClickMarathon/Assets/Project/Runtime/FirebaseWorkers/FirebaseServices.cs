using Firebase.Auth;
using Firebase.Database;

namespace FirebaseWorkers
{
     public static class FirebaseServices
     {
          public static FirebaseAuth GetAuthenticationService() => FirebaseAuth.DefaultInstance;

          public static FirebaseDatabase GetDatabaseService() => FirebaseDatabase.DefaultInstance;
     }
}
