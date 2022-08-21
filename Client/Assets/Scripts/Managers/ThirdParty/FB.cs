using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class FB : MonoBehaviour
{
    public static Firebase.FirebaseApp App;

    public static Action<Firebase.FirebaseApp> onReadyFireBase;

    private void Start()
    {
        StartFirebaseInit();
    }

    public static void StartFirebaseInit()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                App = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                onReadyFireBase?.Invoke(Firebase.FirebaseApp.DefaultInstance);

                //Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventAppOpen);
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

}
