using System;
using UnityEditor;
using UnityEngine;

public class FixJDKPath//: MonoBehaviour
{
     [RuntimeInitializeOnLoadMethod()]
     private static void FixJDKPathMethod()
     {
          Debug.Log($"fixing JDK path");

          string newJDKPath = EditorApplication.applicationPath.Replace("Unity.exe", "Data/PlaybackEngines/AndroidPlayer/OpenJDK");

          if(Environment.GetEnvironmentVariable("JAVA_HOME") != newJDKPath)
          {
               Environment.SetEnvironmentVariable("JAVA_HOME", newJDKPath);
          }
     }
}
