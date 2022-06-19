using UnityEngine;
using UnityEditor;
using System;

public class FixJDKPath//: MonoBehaviour
{
     [RuntimeInitializeOnLoadMethod()]
     static void FixJDKPathMethod()
     {
          Debug.Log($"fixing JDK path");

          string newJDKPath = EditorApplication.applicationPath.Replace("Unity.exe", "Data/PlaybackEngines/AndroidPlayer/OpenJDK");

          if(Environment.GetEnvironmentVariable("JAVA_HOME") != newJDKPath)
          {
               Environment.SetEnvironmentVariable("JAVA_HOME", newJDKPath);
          }
     }
}
