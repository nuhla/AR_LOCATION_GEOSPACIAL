using System.Collections;
using System.Collections.Generic;
using ARLocation.MapboxRoutes;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine;
using Mapbox.Unity.Location;
using ARLocation;
using static ARLocation.MapboxRoutes.MapboxRoute;
using System;
using static Mercator;
using TMPro;

public enum LineType
{
    Route,
    NextTarget
}
public class RoutOrgnizer : MonoBehaviour
{

    private double lat;
    private double lang;
    private double alt;

    private bool _CheckIsItDone = false;
    private SettingsData settings = new SettingsData();
    public GameObject Done;

    public TMP_Text Distance;
    private float CurrentDistance = 0.0f;

    private float lastDistan = 0.05f;
    public MapboxRoute route;

    public GameObject Charc;
    public bool _isDoneTrue = false;
    public bool _isCharTru = false;
    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>
    public AREarthManager EarthManager;
    public bool _Permission_IsGrandet;
    private int count = 0;
    private int count2 = 0;
    private PlaceAtLocation placeAtLocation;


    private void Start()
    {

        if (!Input.location.isEnabledByUser)
        {
            _Permission_IsGrandet = true;
        }
        else
        {
            _Permission_IsGrandet = false;
        }
        if (route != null)
        {
            /// <summary>
            /// Debugging
            /// </summary>


            lat = double.Parse(PlayerPrefs.GetString("Latitude"));
            lang = double.Parse(PlayerPrefs.GetString("Longitude"));
            alt = double.Parse(PlayerPrefs.GetString("altitud"));

            Debug.Log(lat + "," + lang + "," + alt);


            placeAtLocation = Charc.GetComponent<PlaceAtLocation>();
            if (placeAtLocation)
            {
                Debug.Log("---------------- Found---------");
            }

            try
            {
                placeAtLocation.LocationOptions = new PlaceAtLocation.LocationSettingsData();
                placeAtLocation.LocationOptions.LocationInput.Location.AltitudeMode = AltitudeMode.GroundRelative;
                placeAtLocation.LocationOptions.LocationInput.LocationInputType = LocationPropertyData.LocationPropertyType.Location;
                placeAtLocation.LocationOptions.LocationInput = new LocationPropertyData();
                placeAtLocation.LocationOptions.LocationInput.Location = new ARLocation.Location(lat, lang, alt);
             
                Debug.Log(placeAtLocation.LocationOptions.LocationInput.Location.Latitude + "-" + placeAtLocation.LocationOptions.LocationInput.Location.Altitude + "-" + placeAtLocation.LocationOptions.LocationInput.Location.Longitude);
                Charc.gameObject.SetActive(true);
            }
            catch (Exception EX)
            {
                Debug.Log(EX);

            }



            settings.RouteSettings = new RouteSettings();
            settings.RouteSettings.To = new RouteWaypoint { Type = RouteWaypointType.Location };

            settings.RouteSettings.To.Location = new ARLocation.Location(lat, lang, alt);
            settings.RouteSettings.To.Location.AltitudeMode = AltitudeMode.GroundRelative;

            route.Settings.RouteSettings.To = settings.RouteSettings.To;

            route.gameObject.SetActive(true);
        }
    }



    private void Update()
    {

        Debug.Log("------------------ Update ---------");

        //------------ Convert The Location To GeoCoordinate ----------//

        GeoCoordinate UserPlace = new GeoCoordinate(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.altitude);
        GeoCoordinate PlaceLocation = new GeoCoordinate(lat, lang, alt);

        //------------ Check The Distance  ----------//
        double distance = PlaceLocation.GetDistanceTo(UserPlace);
        CurrentDistance = (float)distance;

        Distance.text = CurrentDistance.ToString();
        //------------ Set The Remaining Distance to Point  ----------//
        if (!Mathf.Approximately(CurrentDistance, lastDistan))
        {
            Distance.text = distance.ToString();
        }

        lastDistan = (float)CurrentDistance;

        //------------ Activate Done Menu  ----------//
        if (CurrentDistance <= 5)
        {

            _isDoneTrue = true;
            Debug.Log("you Reach The Point" + count + "count");
        }
        if (_isDoneTrue && !Done.gameObject.active)
        {
            Debug.Log("you Reach The Point");
            _CheckIsItDone = true;
            Done.gameObject.SetActive(true);
            _isDoneTrue = false;
        }


        //------------ Activate Done Charachter  ----------//
        if (CurrentDistance <= 10)
        {
            _isCharTru = true;


        }
        if (_isCharTru && !_isCharTru)
        {
            Debug.Log("you Reach The Point");
            Charc.gameObject.SetActive(true);

        }

    }

}






