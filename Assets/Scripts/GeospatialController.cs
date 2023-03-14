// <copyright file="GeospatialController.cs" company="Google LLC">
//
// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Firebase.Database;
using Mapbox;
using Google.XR.ARCoreExtensions;
using UnityEngine.Android;
using Newtonsoft.Json;
using static Mercator;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for Geospatial sample.
/// </summary>

public class ShouldResolvingHistory
{
    public bool _shouldResolvingHistory;
}

[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines",
    Justification = "Bypass source check.")]
public class GeospatialController : MonoBehaviour
{
    [Header("AR Components")]

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
    public AREarthManager EarthManager;

    /// <summary>
    /// The ARCoreExtensions used in the sample.
    /// </summary>
    public ARCoreExtensions ARCoreExtensions;

    [Header("UI Elements")]

    /// <summary>
    /// A 3D object that presents an Geospatial Anchor.
    /// </summary>
    public GameObject GeospatialPrefab;

    /// <summary>
    /// A 3D object that presents an Geospatial Terrain anchor.
    /// </summary>
    public GameObject TerrainPrefab;

    public DatabaseController dataController;

    /// <summary>
    /// UI element showing VPS availability notification.
    /// </summary>
    public GameObject VPSCheckCanvas;


    public GeospatialAnchorHistoryCollection geospacialPoints = new GeospatialAnchorHistoryCollection();

    /// <summary>
    /// UI element containing all AR view contents.
    /// </summary>

    public Text InfoText;

    public double _DistanceToCreatAnchor = 30;

    /// <summary>
    /// Text displaying in a snack bar at the bottom of the screen.
    /// </summary>
    public Text SnackBarText;



    /// <summary>
    /// Help message shows while localizing.
    /// </summary>
    private const string _localizingMessage = "Localizing your device to set anchor.";

    /// <summary>
    /// Help message shows while initializing Geospatial functionalities.
    /// </summary>
    private const string _localizationInitializingMessage =
        "Initializing Geospatial functionalities.";

    /// <summary>
    /// Help message shows when <see cref="AREarthManager.EarthTrackingState"/> is not tracking
    /// or the pose accuracies are beyond thresholds.
    /// </summary>
    private const string _localizationInstructionMessage =
        "Point your camera at buildings, stores, and signs near you.";

    /// <summary>
    /// Help message shows when location fails or hits timeout.
    /// </summary>
    private const string _localizationFailureMessage =
        "Localization not possible.\n" +
        "Close and open the app to restart the session.";

    /// <summary>
    /// Help message shows when location success.
    /// </summary>
    private const string _localizationSuccessMessage = "Localization completed.";

    /// <summary>
    /// Help message shows when resolving takes too long.
    /// </summary>
    private const string _resolvingTimeoutMessage =
        "Still resolving the terrain anchor.\n" +
        "Please make sure you're in an area that has VPS coverage.";

    /// <summary>
    /// The timeout period waiting for localization to be completed.
    /// </summary>
    private const float _timeoutSeconds = 180;

    /// <summary>
    /// Indicates how long a information text will display on the screen before terminating.
    /// </summary>
    private const float _errorDisplaySeconds = 3;

    /// <summary>
    /// The key name used in PlayerPrefs which indicates whether the privacy prompt has
    /// displayed at least one time.
    /// </summary>
    private const string _hasDisplayedPrivacyPromptKey = "HasDisplayedGeospatialPrivacyPrompt";

    /// <summary>
    /// The key name used in PlayerPrefs which stores geospatial anchor history data.
    /// The earliest one will be deleted once it hits storage limit.
    /// </summary>
    private const string _persistentGeospatialAnchorsStorageKey = "PersistentGeospatialAnchors";

    /// <summary>
    /// The limitation of how many Geospatial Anchors can be stored in local storage.
    /// </summary>
    private const int _storageLimit = 5;

    /// <summary>
    /// Accuracy threshold for orientation yaw accuracy in degrees that can be treated as
    /// localization completed.
    /// </summary>
    private const double _orientationYawAccuracyThreshold = 25;

    /// <summary>
    /// Accuracy threshold for heading degree that can be treated as localization completed.
    /// </summary>
    private const double _headingAccuracyThreshold = 25;

    /// <summary>
    /// Accuracy threshold for altitude and longitude that can be treated as localization
    /// completed.
    /// </summary>
    private const double _horizontalAccuracyThreshold = 20;


    private bool _waitingForLocationService = false;
    private bool _isInARView = false;
    private bool _isReturning = false;
    private bool _isLocalizing = false;
    private bool _enablingGeospatial = false;
    public ShouldResolvingHistory _shouldResolvingHistory = new ShouldResolvingHistory();
    private bool _usingTerrainAnchor = true;
    private float _localizationPassedTime = 0f;
    private static List<String> _InstantiatedAnchors = new List<string>();
    private float _configurePrepareTime = 3f;
    private static List<GameObject> _anchorObjects = new List<GameObject>();
    private IEnumerator _startLocationService = null;
    private IEnumerator _asyncCheck = null;
    private DatabaseController dataIns;
    private GeospatialAnchorHistoryCollection _historyCollection = null;





    /// <summary>
    /// Callback handling "Get Started" button click event in Privacy Prompt.
    /// </summary>


    /// <summary>
    /// Callback handling "Learn More" Button click event in Privacy Prompt.
    /// </summary>
    public void OnLearnMoreClicked()
    {
        Application.OpenURL(
            "https://developers.google.com/ar/data-privacy");
    }



    private bool IsInRange(GeospatialAnchorHistory Point)
    {

        GeoCoordinate GeoPoint = new GeoCoordinate(Point.Latitude, Point.Longitude, Point.Latitude);
        GeospatialPose myPostion = EarthManager.CameraGeospatialPose;
        GeoCoordinate myPostionGeo = new GeoCoordinate(myPostion.Latitude, myPostion.Longitude, myPostion.Altitude);
        double distance = myPostionGeo.GetDistanceTo(GeoPoint);
        if (distance < _DistanceToCreatAnchor)
        {
            return true;
        }


        return false;
    }

    /// <summary>
    /// Unity's Awake() method.
    /// </summary>
    public async void Awake()
    {
        Debug.Log(" iN AWAK ......");
        // Lock screen to portrait.
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Portrait;
        _shouldResolvingHistory._shouldResolvingHistory = false;
        getData();


        // Enable geospatial sample to target 60fps camera capture frame rate
        // on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;

        if (SessionOrigin == null)
        {
            Debug.LogError("Cannot find ARSessionOrigin.");
        }

        if (Session == null)
        {
            Debug.LogError("Cannot find ARSession.");
        }

        if (ARCoreExtensions == null)
        {
            Debug.LogError("Cannot find ARCoreExtensions.");
        }


    }

    public void getData()
    {
        Debug.Log("before StartCoroutine");

        StartCoroutine(getAnchores((DataSnapshot snapshot) =>
            {


                using (var sequenceEnum = snapshot.Children.GetEnumerator())
                {
                    for (int i = 0; i < snapshot.ChildrenCount; i++)
                    {

                        while (sequenceEnum.MoveNext())
                        {
                            var id = sequenceEnum.Current.Key;
                            try
                            {


                                IDictionary dictContent = (IDictionary)sequenceEnum.Current.Value;

                                var json = JsonConvert.SerializeObject(dictContent);

                                GeospatialAnchorHistory historyanchor =
                                JsonConvert.DeserializeObject<GeospatialAnchorHistory>(json);
                                historyanchor.Instaniated = false;
                                historyanchor.Id = id;

                                Debug.Log("Id =" + historyanchor.Id + "\n" + historyanchor.Latitude + " ,\n " + historyanchor.Description +
                                " , " + historyanchor.Heading + " ,\n " + historyanchor.Longitude + " ,\n " +
                                historyanchor.FullDiscription + " ,\n " + historyanchor.Altitude + " ,\n "
                                + historyanchor.Title +
                                 "\n Instaniated" + historyanchor.Instaniated
                                 + "\n ManualHeight" +historyanchor.URL);

                                geospacialPoints.Collection.Add(historyanchor);

                            }
                            catch (Exception ex)
                            {
                                Debug.Log("Error" + ex.Message);
                            }
                        }

                    }

                    _shouldResolvingHistory._shouldResolvingHistory = true;

                }

            }
            ));
    }


    public IEnumerator getAnchores(Action<DataSnapshot> onCallback)
    {
        Debug.Log("getAnchores(Action<DataSnapshot> onCallback)");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("reference" + reference.ToString());

        yield return FirebaseDatabase.DefaultInstance.GetReference("anchors").GetValueAsync().ContinueWith(task =>
         {
             if (task.IsFaulted)

             {
                 Debug.Log("No Data.....");
                 return;
             }

             else if (task.IsCompletedSuccessfully)
             {
                 DataSnapshot snapshot = task.Result;
                 Debug.Log(snapshot.Value.ToString() + "get value");


                 onCallback.Invoke(snapshot);

             }
         });


    }






    // Update is called once per frame
    /// <summary>
    /// Unity's OnEnable() method.
    /// </summary>
    public void OnEnable()
    {
        _startLocationService = StartLocationService();
        StartCoroutine(_startLocationService);
        _isReturning = false;
        _enablingGeospatial = false;
        //SnackBarText.gameObject.SetActive(Debug.isDebugBuild && EarthManager != null);
        _localizationPassedTime = 0f;
        _isLocalizing = true;

        SnackBarText.text = _localizingMessage;
        _shouldResolvingHistory._shouldResolvingHistory = geospacialPoints.Collection.Count > 0;
        SwitchToARView(true);

    }


    private void SwitchToARView(bool enable)
    {
        _isInARView = enable;
        SessionOrigin.gameObject.SetActive(enable);
        Session.gameObject.SetActive(enable);
        ARCoreExtensions.gameObject.SetActive(enable);

        VPSCheckCanvas.SetActive(false);
        if (enable && _asyncCheck == null)
        {
            _asyncCheck = AvailabilityCheck();
            StartCoroutine(_asyncCheck);

        }
    }

    private IEnumerator AvailabilityCheck()
    {
        if (ARSession.state == ARSessionState.None)
        {
            yield return ARSession.CheckAvailability();
        }

        // Waiting for ARSessionState.CheckingAvailability.
        yield return null;

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            yield return ARSession.Install();
        }

        // Waiting for ARSessionState.Installing.
        yield return null;

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Requesting camera permission.");
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitForSeconds(3.0f);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // User has denied the request.
            Debug.LogWarning(
                "Failed to get camera permission. VPS availability check is not available.");
            yield break;
        }
#endif
#if UNITY_IOS
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
#endif

        while (_waitingForLocationService)
        {
            yield return null;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogWarning(
                "Location service is not running. VPS availability check is not available.");
            yield break;
        }

        // Update event is executed before coroutines so it checks the latest error states.
        if (_isReturning)
        {
            yield break;
        }

        var location = Input.location.lastData;
        var vpsAvailabilityPromise =
            AREarthManager.CheckVpsAvailability(location.latitude, location.longitude);
        yield return vpsAvailabilityPromise;

        SnackBarText.text = "VPS Availability at ({0}, {1}): {2}" +
            location.latitude + "," + location.longitude + "," + vpsAvailabilityPromise.Result;
        VPSCheckCanvas.SetActive(vpsAvailabilityPromise.Result != VpsAvailability.Available);


    }


    /// <summary>
    /// Go To Indoor Scene.
    /// </summary>
    private void GoToIndoorMood()
    {


        GeoCoordinate GeoPoint = new GeoCoordinate(47.431885611009406, 9.384837285619275, 661.5821774);
        GeospatialPose myPostion = EarthManager.CameraGeospatialPose;
        GeoCoordinate myPostionGeo = new GeoCoordinate(myPostion.Latitude, myPostion.Longitude, myPostion.Altitude);
        double distance = myPostionGeo.GetDistanceTo(GeoPoint);
        if (distance < 300)
        {
            SceneManager.LoadScene("Hol9");
        }

    }

    /// <summary>
    /// Unity's Update() method.
    /// </summary>
    public void Update()
    {
            
        GoToIndoorMood();

        if (!_isInARView)
        {

            return;
        }

        UpdateDebugInfo();

        // Check session error status.
        // ResolveHistory();
        _shouldResolvingHistory._shouldResolvingHistory =!(geospacialPoints.Collection.Count == _InstantiatedAnchors.Count);
        
        LifecycleUpdate();
        if (_isReturning)
        {

            return;
        }

        if (ARSession.state != ARSessionState.SessionInitializing &&
            ARSession.state != ARSessionState.SessionTracking)
        {

            return;
        }

        // Check feature support and enable Geospatial API when it's supported.
        var featureSupport = EarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        switch (featureSupport)
        {
            case FeatureSupported.Unknown:
                return;
            case FeatureSupported.Unsupported:
                ReturnWithReason("Geospatial API is not supported by this devices.");
                return;
            case FeatureSupported.Supported:
                if (ARCoreExtensions.ARCoreExtensionsConfig.GeospatialMode ==
                    GeospatialMode.Disabled)
                {
                    Debug.Log("Geospatial sample switched to GeospatialMode.Enabled.");
                    ARCoreExtensions.ARCoreExtensionsConfig.GeospatialMode =
                        GeospatialMode.Enabled;
                    _configurePrepareTime = 3.0f;
                    _enablingGeospatial = true;
                    return;
                }

                break;
        }

        // Waiting for new configuration taking effect.
        if (_enablingGeospatial)
        {
            _configurePrepareTime -= Time.deltaTime;
            if (_configurePrepareTime < 0)
            {
                _enablingGeospatial = false;
            }
            else
            {
                return;
            }
        }

        // Check earth state.
        var earthState = EarthManager.EarthState;
        if (earthState == EarthState.ErrorEarthNotReady)
        {
            SnackBarText.text = _localizationInitializingMessage;
            return;
        }
        else if (earthState != EarthState.Enabled)
        {
            string errorMessage =
                "Geospatial sample encountered an EarthState error: " + earthState;
            Debug.LogWarning(errorMessage);
            SnackBarText.text = errorMessage;
            return;
        }

        if (_anchorObjects.Count < geospacialPoints.Collection.Count)
        {
            ResolveHistory();

        }



        // Check earth localization.
        bool isSessionReady = ARSession.state == ARSessionState.SessionTracking &&
            Input.location.status == LocationServiceStatus.Running;

        var earthTrackingState = EarthManager.EarthTrackingState;

        var pose = earthTrackingState == TrackingState.Tracking ?
            EarthManager.CameraGeospatialPose : new GeospatialPose();


        if (!isSessionReady || earthTrackingState != TrackingState.Tracking ||
            pose.OrientationYawAccuracy > _orientationYawAccuracyThreshold ||
            pose.HorizontalAccuracy > _horizontalAccuracyThreshold)
        {
            // Lost localization during the session.
            if (!_isLocalizing)
            {
                _isLocalizing = true;
                _localizationPassedTime = 0f;



                foreach (var go in _anchorObjects)
                {
                    go.SetActive(false);
                }
            }

            if (_localizationPassedTime > _timeoutSeconds)
            {
                SnackBarText.text = "Geospatial sample localization passed timeout.";

                ReturnWithReason(_localizationFailureMessage);
            }
            else
            {
                _localizationPassedTime += Time.deltaTime;
                SnackBarText.text = _localizationInstructionMessage;
            }
        }
        else if (_isLocalizing)
        {
            // Finished localization.
            _isLocalizing = false;
            _localizationPassedTime = 0f;

            SnackBarText.text = _localizationSuccessMessage;
            foreach (var go in _anchorObjects)
            {
                var terrainState = go.GetComponent<ARGeospatialAnchor>().terrainAnchorState;
                if (terrainState != TerrainAnchorState.None &&
                    terrainState != TerrainAnchorState.Success)
                {
                    // Skip terrain anchors that are still waiting for resolving
                    // or failed on resolving.
                    continue;
                }

                go.SetActive(true);
            }

           

        }
       

        // Set anchor on screen tap.
        if (earthTrackingState == TrackingState.Tracking)
        {
            InfoText.text = string.Format(
            "Latitude/Longitude: {1}°, {2}°{0}" +
            "Horizontal Accuracy: {3}m{0}" +
            "Altitude: {4}m{0}" +
            "Vertical Accuracy: {5}m{0}" +
            "Eun Rotation: {6}{0}" +
            "Orientation Yaw Accuracy: {7}°",
            Environment.NewLine,
            pose.Latitude.ToString(),
            pose.Longitude.ToString(),
            pose.HorizontalAccuracy.ToString("F6"),
            pose.Altitude.ToString("F2"),
            pose.VerticalAccuracy.ToString("F2"),
            pose.EunRotation.ToString("F1"),
            pose.OrientationYawAccuracy.ToString("F1"));
        }
        else
        {
            InfoText.text = "GEOSPATIAL POSE: not tracking";
        }
    }

    private IEnumerator CheckTerrainAnchorState(ARGeospatialAnchor anchor)
    {
        if (anchor == null || _anchorObjects == null)
        {
            yield break;
        }

        int retry = 0;
        while (anchor.terrainAnchorState == TerrainAnchorState.TaskInProgress)
        {
            if (_anchorObjects.Count == 0 || !_anchorObjects.Contains(anchor.gameObject))
            {
                Debug.LogFormat(
                    "{0} has been removed, exist terrain anchor state check.",
                    anchor.trackableId);
                yield break;
            }

            if (retry == 100 && _anchorObjects.Last().Equals(anchor.gameObject))
            {
                SnackBarText.text = _resolvingTimeoutMessage;
            }

            yield return new WaitForSeconds(0.1f);
            retry = Math.Min(retry + 1, 100);
        }

        anchor.gameObject.SetActive(
            !_isLocalizing && anchor.terrainAnchorState == TerrainAnchorState.Success);
        if (_anchorObjects.Last().Equals(anchor.gameObject))
        {
            SnackBarText.text = $"Terrain anchor state: {anchor.terrainAnchorState}";
        }

        yield break;
    }



    public void OnContinueClicked()
    {
        VPSCheckCanvas.SetActive(false);
    }

    private ARGeospatialAnchor PlaceGeospatialAnchor(

        GeospatialAnchorHistory history, bool terrain = true)
    {
        Debug.Log("in PlaceGeospatialAnchor \n");


        GeospatialPose point = EarthManager.CameraGeospatialPose;

        // Quaternion eunRotation = history.heading == 0f ? point.EunRotation : history.eunRotation;


        try
        {
            //---------------------------------------------------//
            //------------- get the Height from database --------//
            //---------------------------------------------------//
            var Height = history.Terrain ? history.TerainHeigt : history.Altitude;

            Quaternion eunRotation = Quaternion.AngleAxis((180f - (float)point.Heading), Vector3.up);
            Debug.Log(history.Terrain + "------------ history.Terrain ----------");
            //----------------------------------------------------//
            // Create Geospaciale Anchor and resolve Terain Anchore
            //----------------------------------------------------//
            

            //history.Terrain
            var anchor = history.Terrain ?
                AnchorManager.ResolveAnchorOnTerrain(
                    history.Latitude, history.Longitude, Height, eunRotation) :
                AnchorManager.AddAnchor(
                    history.Latitude, history.Longitude, history.Altitude, eunRotation);

            // ----------------- Debugging -------------------------//
           

            if (anchor != null)
            {
                //------------------------------------------------//
                // Instantiate GameObject and Put it in the Terrain
                //------------------------------------------------//
                GameObject anchorGO = terrain ?
                Instantiate(TerrainPrefab, anchor.transform) :
                Instantiate(GeospatialPrefab, anchor.transform);
                history.Instaniated = true;
                _InstantiatedAnchors.Add(history.Id);


                //----------------------------------------------//
                // Evaluate GameObject MarkerDat with Correct Data
                //----------------------------------------------//
                anchorGO.transform.GetComponent<MarkerData>().Marker.Description = history.Description;
                anchorGO.transform.GetComponent<MarkerData>().Marker.Title = history.Title;
                anchorGO.transform.GetComponent<MarkerData>().Marker.Altitude = history.Altitude;
                anchorGO.transform.GetComponent<MarkerData>().Marker.Latitude = history.Latitude;
                anchorGO.transform.GetComponent<MarkerData>().Marker.Longitude = history.Longitude;
                anchorGO.transform.GetComponent<MarkerData>().Marker.FullDiscription = history.FullDiscription;
                anchorGO.transform.GetComponent<MarkerData>().Discription.text = history.FullDiscription;
                anchorGO.transform.GetComponent<MarkerData>().Title.text = history.FullDiscription;
                


                SnackBarText.text = "Anchore with position = " + "x : " + anchor.transform.position.x.ToString()
                + "y : " + anchor.transform.position.y.ToString() +
                "z : " + anchor.transform.position.z.ToString();
                if (_isLocalizing)
                {
                    anchor.gameObject.SetActive(terrain);
                }

                _anchorObjects.Add(anchor.gameObject);
               
                Debug.Log("in _anchorObjects add");

                if (terrain)
                {
                    StartCoroutine(CheckTerrainAnchorState(anchor));
                }
                else
                {
                    SnackBarText.text = $"{_anchorObjects.Count} Anchor(s) Set!";
                }
            }
            else
            {
                SnackBarText.text = string.Format(
                    "Failed to set {0}!", terrain ? "a terrain anchor" : "an anchor");
            }


            return anchor;
        }

        catch (Exception ex)
        {
            Debug.Log("exxxxxxxxxxx" + ex);


        }


        return null;
    }

    private void ResolveHistory()
    {


        if (!_shouldResolvingHistory._shouldResolvingHistory)
        {
            
            return;
        }

        _shouldResolvingHistory._shouldResolvingHistory = false;


        foreach (var history in geospacialPoints.Collection)
        {

            try
            {
                Debug.Log(IsInRange(history));
                Debug.Log(history.Id +"history.Id");

                if (IsInRange(history)){

                    if (!_InstantiatedAnchors.Contains(history.Id))
                    {
                        PlaceGeospatialAnchor(history);
                    }
                    

               }
                
            }
            catch (Exception ex)
            {
                Debug.Log("Exception:" + ex);

                InfoText.text += ex.ToString();
            }

        }


        InfoText.text = string.Format("{0} anchor(s) set from history.",
        _anchorObjects.Count);
    }





    private IEnumerator StartLocationService()
    {
        _waitingForLocationService = true;
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Requesting fine location permission.");
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(3.0f);
        }
#endif

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location service is disabled by User.");
            _waitingForLocationService = false;
            yield break;
        }

        Debug.Log("Start location service.");
        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return null;
        }

        _waitingForLocationService = false;
        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogWarningFormat(
                "Location service ends with {0} status.", Input.location.status);
            Input.location.Stop();
        }
    }

    private void LifecycleUpdate()
    {
        // Pressing 'back' button quits the app.
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (_isReturning)
        {
            return;
        }

        // Only allow the screen to sleep when not tracking.
        var sleepTimeout = SleepTimeout.NeverSleep;
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            sleepTimeout = SleepTimeout.SystemSetting;
        }

        Screen.sleepTimeout = sleepTimeout;

        // Quit the app if ARSession is in an error status.
        string returningReason = string.Empty;
        if (ARSession.state != ARSessionState.CheckingAvailability &&
            ARSession.state != ARSessionState.Ready &&
            ARSession.state != ARSessionState.SessionInitializing &&
            ARSession.state != ARSessionState.SessionTracking)
        {
            returningReason = string.Format(
                "Geospatial sample encountered an ARSession error state {0}.\n" +
                "Please start the app again.",
                ARSession.state);
        }
        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            returningReason =
                "Geospatial sample failed to start location service.\n" +
                "Please start the app again and grant precise location permission.";
        }
        else if (SessionOrigin == null || Session == null || ARCoreExtensions == null)
        {
            returningReason = string.Format(
                "Geospatial sample failed with missing AR Components.");
        }

        ReturnWithReason(returningReason);
    }

    private void ReturnWithReason(string reason)
    {
        if (string.IsNullOrEmpty(reason))
        {
            return;
        }


        Debug.LogError(reason);
        SnackBarText.text = reason;
        _isReturning = true;
        Invoke(nameof(QuitApplication), _errorDisplaySeconds);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    private void UpdateDebugInfo()
    {
        if (!Debug.isDebugBuild || EarthManager == null)
        {
            return;
        }

        var pose = EarthManager.EarthState == EarthState.Enabled &&
            EarthManager.EarthTrackingState == TrackingState.Tracking ?
            EarthManager.CameraGeospatialPose : new GeospatialPose();
        var supported = EarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        InfoText.text =
            $"IsReturning: {_isReturning}\n" +
            $"IsLocalizing: {_isLocalizing}\n" +
            $"SessionState: {ARSession.state}\n" +
            $"LocationServiceStatus: {Input.location.status}\n" +
            $"FeatureSupported: {supported}\n" +
            $"EarthState: {EarthManager.EarthState}\n" +
            $"EarthTrackingState: {EarthManager.EarthTrackingState}\n" +
            $"  LAT/LNG: {pose.Latitude:F6}, {pose.Longitude:F6}\n" +
            $"  HorizontalAcc: {pose.HorizontalAccuracy:F6}\n" +
            $"  ALT: {pose.Altitude:F2}\n" +
            $"  VerticalAcc: {pose.VerticalAccuracy:F2}\n" +
            $". EunRotation: {pose.EunRotation:F2}\n" +
            $"  OrientationYawAcc: {pose.OrientationYawAccuracy:F2}" +
            $"  count Of Anchors: {geospacialPoints.Collection.Count:F2}" +
            $"  count Of gameObject Created: {_anchorObjects.Count:F2}";

    }
}

