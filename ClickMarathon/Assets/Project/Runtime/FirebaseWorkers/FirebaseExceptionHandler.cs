using System;
using System.Threading.Tasks;
using UnityEngine;
using External.Signatures;
using External.Extensions;
using Firebase.Extensions;

namespace FirebaseWorkers
{
     public static class FirebaseExceptionHandler
     {
          public delegate void CommonArgsAction(CommonArgs args);
          public delegate void GenericArgsAction<T>(GenericArgs<T> args);

          public static void ThenHandleTaskResults(this Task taskInProgress, CommonArgsAction argumentsSetter)
          {
               var args = new CommonArgs();
               argumentsSetter.Invoke(args);

               Action generateContinuation() => taskInProgress switch
               {
                    { IsCanceled: true } => () => Debug.LogError("task: was canceled."),

                    { IsFaulted: true } => () => args.OnFailed?.Invoke(taskInProgress.Exception.GetLastInner().Message),

                    { IsCompletedSuccessfully: true } => () => args.OnSucceed?.Invoke(),

                    _ => () => Debug.Log("task: Something went wrong.")
               };

               taskInProgress.ContinueWithOnMainThread(_ => generateContinuation().Invoke());

               // funny moment:
               // await taskInProgress.ContinueWithOnMainThread(_ => generateContinuation()());
          }

          public static void ThenHandleTaskResults<T>(this Task<T> taskInProgress, GenericArgsAction<T> argumentsSetter)
          {
               var args = new GenericArgs<T>();
               argumentsSetter.Invoke(args);

               Action generateContinuation() => taskInProgress switch
               {
                    { IsCanceled: true } => () => Debug.LogError("task: was canceled."),

                    { IsFaulted: true } => () => args.OnFailed?.Invoke(taskInProgress.Exception.GetLastInner().Message),

                    { IsCompletedSuccessfully: true, Result: not null } => () => args.OnSucceed?.Invoke(taskInProgress.Result),

                    _ => () => Debug.Log("task: Something went wrong.")
               };

               taskInProgress.ContinueWithOnMainThread(_ => generateContinuation().Invoke());
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
