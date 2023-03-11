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
        // / <summary>
        // / Setting Up Main Router Settings .
        // / </summary>
        settings.Language = new MapboxApiLanguage();
        settings.Language = MapboxApiLanguage.Arabic;
        settings.MapboxToken = "pk.eyJ1IjoiYXJ0ZWxvIiwiYSI6ImNsZWtrY2g0dTBtOGQzcm5wNWd6ajd4OW0ifQ.kuIQLXklaS1BTG4DALtTWg";
        settings.LoadRouteAtStartup = true;
        settings.RouteSettings = new RouteSettings();
        settings.RouteSettings.RouteType = RouteType.Mapbox;
        settings.RouteSettings.From = new RouteWaypoint { Type = RouteWaypointType.UserLocation };
        settings.RouteSettings.To = new RouteWaypoint { Type = RouteWaypointType.Location };

        try
        {
            settings.OnScreenIndicator = transform.Find("MapboxRoute").gameObject.GetComponent<DefaultOnScreenTargetIndicator>();
            Debug.Log("----------------- DefaultOnScreenTargetIndicator Founded --------------");
        }
        catch
        {
            Debug.Log("----------------- Not Found --------------");
            settings.OnScreenIndicator = new DefaultOnScreenTargetIndicator();

        }
        try
        {
            settings.PathRenderer = transform.Find("MapboxRoute").gameObject.GetComponent<PathRouteRenderer>();
            Debug.Log("----------------- PathRouteRenderer Founded --------------");
        }
        catch
        {
            settings.PathRenderer = new PathRouteRenderer();


            Debug.Log("----------------- PathRouteRenderer Not Founded --------------");
        }


    }

    private void Start()
    {
        StartRouting();
    }

    public void StartRouting()
    {
        /// <summary>
        /// Setting Up Main To and from Depending On AnchoreData Position.
        /// </summary>


        if (route != null)
        {
            /// <summary>
            /// Debugging
            /// </summary>


            var lat = double.Parse(PlayerPrefs.GetString("Latitude"));
            var lang = double.Parse(PlayerPrefs.GetString("Longitude"));
            var alt = double.Parse(PlayerPrefs.GetString("altitud"));

            Debug.Log(lat + "," + lang + "," + alt);
            //settings.RouteSettings.From.Location = new ARLocation.Location(31.8968933882709, 35.17448570997088, 789.2380951624432);
            // settings.RouteSettings.From.Location.AltitudeMode = AltitudeMode.GroundRelative;
            settings.RouteSettings.To.Location = new ARLocation.Location(lat, lang, alt);
            settings.RouteSettings.To.Location.AltitudeMode = AltitudeMode.GroundRelative;
            route.Settings = settings;
            /// <summary>
            /// Debugging .
            /// </summary>
            route.gameObject.SetActive(true);
        }




    }





}
