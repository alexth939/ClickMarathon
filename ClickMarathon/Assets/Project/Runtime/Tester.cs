using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          Firebase.AppOptions s = new Firebase.AppOptions();
          Debug.Log($"created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
