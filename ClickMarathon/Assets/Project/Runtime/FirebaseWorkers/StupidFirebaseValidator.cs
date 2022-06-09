using System;
using UnityEngine;

namespace FirebaseWorkers
{
     public sealed class StupidFirebaseValidator
     {
          //todo refactor this crap
          public void CheckIfAvaliable(Action<bool> resultCallback = null)
          {
               Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
               {
                    var dependencyStatus = task.Result;
                    if(dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                         // Create and hold a reference to your FirebaseApp,
                         // where app is a Firebase.FirebaseApp property of your application class.
                         //app = Firebase.FirebaseApp.DefaultInstance;

                         // Set a flag here to indicate whether Firebase is ready to use by your app.
                         Debug.Log($"checked firebase, its still alive and avaliable");
                         resultCallback?.Invoke(true);
                    }
                    else
                    {
                         Debug.LogError(String.Format(
                           "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                         // Firebase Unity SDK is not safe to use here.
                         resultCallback?.Invoke(false);
                    }
               });
          }
     }
}
