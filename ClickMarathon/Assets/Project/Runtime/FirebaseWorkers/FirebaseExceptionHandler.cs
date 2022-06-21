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

     public sealed class FirebaseExceptionHandler
     {
          public delegate void CommonArgsAction(CommonArgs args);
          public delegate void GenericArgsAction<T>(GenericArgs<T> args);

          public static void HandleAuthorizationResults(GenericArgsAction<FirebaseUser> argumentsSetter)
          {
               HandleAttemptResults(argumentsSetter);
          }

          public static void HandleReadResults(GenericArgsAction<DataSnapshot> argumentsSetter)
          {
               HandleAttemptResults(argumentsSetter);
          }

          public static void HandleWriteResults(CommonArgsAction argumentsSetter)
          {
               HandleAttemptResults(argumentsSetter);
          }

          private static void HandleAttemptResults(CommonArgsAction argumentsSetter)
          {
               var args = new CommonArgs();
               argumentsSetter.Invoke(args);

               Action continuation = args.FinishedTask switch
               {
                    { IsCanceled: true } => () => Debug.LogError("task: was canceled."),

                    { IsFaulted: true } => () => args.OnFailed?.Invoke(args.FinishedTask.Exception.GetLastInner().Message),

                    { IsCompletedSuccessfully: true } => () => args.OnSucceed?.Invoke(),

                    _ => () => Debug.Log("task: Something went wrong.")
               };

               args.FinishedTask.ContinueWithOnMainThread(_ => continuation());
          }

          private static void HandleAttemptResults<T>(GenericArgsAction<T> argumentsSetter)
          {
               var args = new GenericArgs<T>();
               argumentsSetter.Invoke(args);

               Action continuation = args.FinishedTask switch
               {
                    { IsCanceled: true } => () => Debug.LogError("task: was canceled."),

                    { IsFaulted: true } => () => args.OnFailed?.Invoke(args.FinishedTask.Exception.GetLastInner().Message),

                    { IsCompletedSuccessfully: true, Result: not null } => () => args.OnSucceed?.Invoke(args.FinishedTask.Result),

                    _ => () => Debug.Log("task: Something went wrong.")
               };

               args.FinishedTask.ContinueWithOnMainThread(_ => continuation());
          }

          public class CommonArgs
          {
               public Task FinishedTask;
               public Action OnSucceed;
               public ExceptionCallback OnFailed;
          }

          public class GenericArgs<T>
          {
               public Task<T> FinishedTask;
               public Action<T> OnSucceed;
               public ExceptionCallback OnFailed;
          }
     }
}
