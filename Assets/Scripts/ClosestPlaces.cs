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

public class ClosestPlaces : MonoBehaviour
{
    [Header("AR Components")]

    private SettingsData settings = new SettingsData();

    public AnchoreData anchoreData = new AnchoreData();

    public MapboxRoute route;
    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>
    public AREarthManager EarthManager;
    /// <summary>
    /// The ARSessionOrigin used in the sample.
    /// </summary>
    public ARSessionOrigin SessionOrigin;

    /// <summary>
    /// The ARSession used in the sample.
    /// </summary>
    public ARSession Session;

    /// <summary>
    /// The ARAnchorManager used in the sample.
    /// </summary>
    public ARAnchorManager AnchorManager;


    /// <summary>
    /// The ARRaycastManager used in the sample.
    /// </summary>
    public ARRaycastManager RaycastManager;

    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>

    public TMPro.TMP_Text _ErrorMessagePanel;
    public TMPro.TMP_Text _Distance;

    public Canvas ErrorCanvas;

    public Canvas RoutingInfoCanvas;

    private RouteResponse currentResponse;
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetInfo(){

        GeospatialPose pos= EarthManager.CameraGeospatialPose;

        settings.RouteSettings.From.Location = new ARLocation.Location(pos.Latitude, pos.Longitude, pos.Altitude);
        settings.RouteSettings.To.Location = new ARLocation.Location(anchoreData.Latitude, anchoreData.Longitude, anchoreData.Altitude);

        route.Settings = settings;
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
            //route.BuildRoute(currentResponse);

        }));
    }
}
