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


    public MapboxRoute route;

    public GameObject Charc;
    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>
    public AREarthManager EarthManager;
    private PlaceAtLocation placeAtLocation;





    private void Awake()
    {


    }

    private void Start()
    {
        if (route != null)
        {
            /// <summary>
            /// Debugging
            /// </summary>


            lat = double.Parse(PlayerPrefs.GetString("Latitude"));
            lang = double.Parse(PlayerPrefs.GetString("Longitude"));
            alt = double.Parse(PlayerPrefs.GetString("altitud"));

            Debug.Log(lat + "," + lang + "," + alt);
            try
            {
                // placeAtLocation = Charc.GetComponent<PlaceAtLocation>();
                // placeAtLocation.LocationOptions = new PlaceAtLocation.LocationSettingsData();
                // placeAtLocation.LocationOptions.LocationInput.LocationInputType = LocationPropertyData.LocationPropertyType.Location;
                // placeAtLocation.Location = new ARLocation.Location(lat, lang, alt);
                // Charc.gameObject.SetActive(false);
                // Debug.Log("88888888888888888888" + placeAtLocation.Location.Altitude + "," + placeAtLocation.Location.Latitude + "," + placeAtLocation.Location.Longitude);
            }
            catch (Exception ex)
            {
                Debug.Log("exp" + ex);
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

        if (EarthManager.EarthTrackingState == TrackingState.Tracking && !_CheckIsItDone)
        {
            GeospatialPose POS = EarthManager.CameraGeospatialPose;
            GeoCoordinate UserPlace = new GeoCoordinate(POS.Latitude, POS.Longitude, POS.Altitude);
            GeoCoordinate PlaceLocation = new GeoCoordinate(lat, lang, alt);

            double distance = PlaceLocation.GetDistanceTo(UserPlace);
            if (distance <= 2)
            {

                Debug.Log("you Reach The Point");
                _CheckIsItDone = true;
                Done.gameObject.SetActive(true);


            }
        }



    }



}
