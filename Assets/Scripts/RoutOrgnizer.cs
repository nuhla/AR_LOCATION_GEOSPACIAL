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


    public GameObject Parent;

    private SettingsData settings = new SettingsData();

    public AnchoreData anchoreData = new AnchoreData();

    public MapboxRoute route;
    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>
    public AREarthManager EarthManager;


    public TMPro.TMP_Text _ErrorMessagePanel;
    public TMPro.TMP_Text _Distance;

    public Canvas ErrorCanvas;

    public Canvas RoutingInfoCanvas;

    private RouteResponse currentResponse;


    private void Awake()
    {
        /// <summary>
        /// Setting Up Main Router Settings .
        /// </summary>
        settings.Language = new MapboxApiLanguage();
        settings.Language = MapboxApiLanguage.English_US;
        settings.MapboxToken = "pk.eyJ1IjoiYXJ0ZWxvIiwiYSI6ImNsZWtrY2g0dTBtOGQzcm5wNWd6ajd4OW0ifQ.kuIQLXklaS1BTG4DALtTWg";
        settings.LoadRouteAtStartup = false;
        settings.RouteSettings = new RouteSettings();
        settings.RouteSettings.RouteType = RouteType.Mapbox;
        settings.RouteSettings.From = new RouteWaypoint { Type = RouteWaypointType.Location };
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

    // private void Start()
    // {
    //     StartRouting();
    // }

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
            // settings.RouteSettings.From.Location = new ARLocation.Location(40.826362, -73.940747, 73.940747);
            // settings.RouteSettings.To.Location = new ARLocation.Location(40.733843, -73.994288, 73.940747);

            settings.RouteSettings.From.Location = new ARLocation.Location(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            settings.RouteSettings.To.Location = new ARLocation.Location(anchoreData.Latitude, anchoreData.Longitude, anchoreData.Altitude);

            route.Settings = settings;
            /// <summary>
            /// Debugging .
            /// </summary>
            Debug.Log(route.Settings.GroundHeight + " --------- GroundHeight---------");
            Debug.Log(route.Settings.MapboxToken + " --------- MapboxToken---------");
            Debug.Log(route.Settings.LoadRouteAtStartup + " --------- LoadRouteAtStartup---------");
            Debug.Log(route.Settings.RouteSettings.To.Location + " --------- RouteSettings.To.Location---------");
            Debug.Log(route.Settings.RouteSettings.RouteType + " --------- RouteSettings.To.RouteType---------");
            Debug.Log(route.Settings.RouteSettings.To.Location);
        }

        /// <summary>
        /// Create a Call To Rout.
        /// </summary>
        var api = new MapboxApi(settings.MapboxToken);
        var loader = new RouteLoader(api);
        route.gameObject.SetActive(true);
        StartCoroutine(loader.LoadRoute(route.Settings.RouteSettings.From, route.Settings.RouteSettings.To, (error, response) =>
        {
            if (error != null)
            {
                ErrorCanvas.gameObject.SetActive(true);
                RoutingInfoCanvas.gameObject.SetActive(false);
                _ErrorMessagePanel.text = error;

                return;
            }
            RoutingInfoCanvas.gameObject.SetActive(true);
            ErrorCanvas.gameObject.SetActive(false);
            currentResponse = response;
            var distance = 0f;
            var NuberOfSteps = 0;

            for (var i = 0; i < currentResponse.routes.Count; i++)
            {
                currentResponse.routes[i].
                distance += currentResponse.routes[i].distance;


            }
            _Distance.text = distance.ToString();
            Debug.Log("_Distance : " + _Distance + " , " + "NuberOfSteps : " + NuberOfSteps);

            /// <summary>
            /// Built Rout.
            /// </summary>
            route.BuildRoute(currentResponse);

        }));

    }





}
