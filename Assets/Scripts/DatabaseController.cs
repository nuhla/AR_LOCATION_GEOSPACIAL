using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using Newtonsoft.Json;
using TMPro;

public class DatabaseController : MonoBehaviour
{
    // Start is called before the first frame update

    private DatabaseReference databaseReference;



    private void Awake()
    {

        Firebase.AppOptions options = new Firebase.AppOptions();
        options.ApiKey = "AIzaSyBkGAz-CNWp25cHoYrKK6jWM8CL3POmkzo";
        options.AppId = "539299682712";
        options.MessageSenderId = "539299682712-odeiooigqkku2f98p3219arp6e885ort.apps.googleusercontent.com";
        options.ProjectId = "arpal-377917";
        options.StorageBucket = "arpal-377917.appspot.com";
        // options.DatabaseUrl="https://arpal-377917-default-rtdb.firebaseio.com";


        var app = Firebase.FirebaseApp.Create(options);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
     {
         var dependencyStatus = task.Result;
         if (dependencyStatus == Firebase.DependencyStatus.Available)
         {
             // Create and hold a reference to your FirebaseApp,
             // where app is a Firebase.FirebaseApp property of your application class.
             app = Firebase.FirebaseApp.DefaultInstance;

             // Set a flag here to indicate whether Firebase is ready to use by your app.
         }
         else
         {
             UnityEngine.Debug.LogError(System.String.Format(
               "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
             Debug.Log(" Firebase Unity SDK is not safe to use here.");
         }
     });

        Debug.Log("in firbase start");


    }

    ///////////////////////////
    /// Create an Anchore/////
    //////////////////////////
    private void CreateAnchore(string Title,
     string Description,
      string FullDiscription,
      double Latitude
     , double Longitude,
      double Altitude,
       double Heading,
       float Qua_Z,
       float Qua_Y,
       float Qua_x)
    {

        AnchoreData anchor = new AnchoreData
        (Title, Description, FullDiscription, Latitude
        , Longitude, Altitude, Heading, Qua_x, Qua_Y, Qua_Z);
        string json = JsonUtility.ToJson(anchor);

        databaseReference.Child("anchors").SetRawJsonValueAsync(json);

    }




   

}
