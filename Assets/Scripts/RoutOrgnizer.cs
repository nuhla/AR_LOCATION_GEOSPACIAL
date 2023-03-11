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

public enum LineType
{
    Route,
    NextTarget
}
public class RoutOrgnizer : MonoBehaviour
{



    private SettingsData settings = new SettingsData();



    public MapboxRoute route;
    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>
    public AREarthManager EarthManager;






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


            var lat = double.Parse(PlayerPrefs.GetString("Latitude"));
            var lang = double.Parse(PlayerPrefs.GetString("Longitude"));
            var alt = double.Parse(PlayerPrefs.GetString("altitud"));

            Debug.Log(lat + "," + lang + "," + alt);

            settings.RouteSettings = new RouteSettings();
            settings.RouteSettings.To = new RouteWaypoint { Type = RouteWaypointType.Location };

            settings.RouteSettings.To.Location = new ARLocation.Location(lat, lang, alt);
            settings.RouteSettings.To.Location.AltitudeMode = AltitudeMode.GroundRelative;

            route.Settings.RouteSettings.To = settings.RouteSettings.To;

            route.gameObject.SetActive(true);
        }
    }






}
