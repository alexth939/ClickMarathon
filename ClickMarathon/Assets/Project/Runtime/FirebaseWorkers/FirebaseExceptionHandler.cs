using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using External.Signatures;
using External.Extensions;
using Firebase.Database;
using Firebase.Extensions;

namespace FirebaseWorkers
{

     public static class FirebaseExceptionHandler
     {
          public delegate void CommonArgsAction(CommonArgs args);
          public delegate void GenericArgsAction<T>(GenericArgs<T> args);

          public static async void ThenHandleTaskResults(this Task taskInProgress, CommonArgsAction argumentsSetter)
          {
               await taskInProgress;

               var args = new CommonArgs();
               argumentsSetter.Invoke(args);

               Action continuation = taskInProgress switch
               {
                    { IsCanceled: true } => () => Debug.LogError("task: was canceled."),

                    { IsFaulted: true } => () => args.OnFailed?.Invoke(taskInProgress.Exception.GetLastInner().Message),

                    { IsCompletedSuccessfully: true } => () => args.OnSucceed?.Invoke(),

                    _ => () => Debug.Log("task: Something went wrong.")
               };

               await taskInProgress.ContinueWithOnMainThread(_ => continuation());
          }

          public static async void ThenHandleTaskResults<T>(this Task<T> taskInProgress, GenericArgsAction<T> argumentsSetter)
          {
               await taskInProgress;

               var args = new GenericArgs<T>();
               argumentsSetter.Invoke(args);

               Action continuation = taskInProgress switch
               {
                    { IsCanceled: true } => () => Debug.LogError("task: was canceled."),

                    { IsFaulted: true } => () => args.OnFailed?.Invoke(taskInProgress.Exception.GetLastInner().Message),

                    { IsCompletedSuccessfully: true, Result: not null } => () => args.OnSucceed?.Invoke(taskInProgress.Result),

                    _ => () => Debug.Log("task: Something went wrong.")
               };

               await taskInProgress.ContinueWithOnMainThread(_ => continuation());
          }

          public class CommonArgs
          {
               public Action OnSucceed;
               public ExceptionCallback OnFailed;
          }

          public class GenericArgs<T>
          {
               public Action<T> OnSucceed;
               public ExceptionCallback OnFailed;
          }
     }
}
