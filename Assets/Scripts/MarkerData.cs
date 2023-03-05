using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System;
using Newtonsoft.Json;

public class MarkerData : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Title;
    [SerializeField]
    public TMP_Text Discription;
    public AnchoreData Marker = new AnchoreData();
    private Text DebugText;
    public string name;



    // Start is called before the first frame update
    void Awake()
    {
        Title.text = name;
        Discription.text = Marker.Description;
    }
    private void Start()
    {
        Title.text = name;
        Discription.text = marker.Description;
    }
    public AnchoreData marker
    {
        get { return this.Marker; }
        set { this.Marker = value; }
    }




    // public IEnumerator getAnchoreDataFromFirebas(Action<DataSnapshot> onCallback)
    // {
    //     yield return FirebaseDatabase.DefaultInstance.
    //     GetReference("anchors").Child(name)
    //     .GetValueAsync().ContinueWith
    //     (task =>
    //     {
    //         if (task.IsFaulted)
    //         {
    //             return;
    //         }
    //         else if (task.IsCompletedSuccessfully)
    //         {
    //             DataSnapshot snapshot = task.Result;
    //             onCallback.Invoke(snapshot);

    //         }
    //     });


    // }


    /// <summary>
    /// Overrides getAnchoreData() method.
    /// </summary>
    /// <returns>get One Anchore Data  </returns>
    // public void getAnchoreData()
    // {

    //     StartCoroutine(getAnchoreDataFromFirebas((DataSnapshot snapshot) =>
    //         {

    //             // ------------------- read jsonfile from Firebas --------------//
    //             IDictionary dictContent = (IDictionary)snapshot.Value;
    //             var json = JsonConvert.SerializeObject(dictContent, Formatting.Indented);

    //             //------------------ Convert json to Marker dataObect-----------//
    //             Marker =
    //             JsonConvert.DeserializeObject<AnchoreData>(json);


    //             //---------------- applay data to UI element -----------------//
    //             Title.text = Marker.Title;
    //             Discription.text = Marker.Description;



    //         }
    //         ));
    // }
    // // Update is called once per frame


    // public AnchoreData getMarker()
    // {

    //     return Marker;
    // }
}
