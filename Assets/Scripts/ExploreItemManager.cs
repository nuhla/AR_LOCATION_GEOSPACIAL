using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation;
using ARLocation.MapboxRoutes;
using Firebase.Extensions;
using Firebase.Storage;
using Google.XR.ARCoreExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using static ARLocation.MapboxRoutes.MapboxRoute;

public class ExploreItemManager : MonoBehaviour
{

    public TMP_Text Discrption;
    public TMP_Text Title;
    public Text duration;
    public Text distance;
    public RawImage Image;
    private FirebaseStorage storage;
    private StorageReference stoeageRefrence;

    private SettingsData settings = new SettingsData();


    public GeospatialAnchorHistory data = new GeospatialAnchorHistory();
    private string imageTexturer;
    private object parent;

    public double distanceOfLocation;

    private bool _isMainWindow = false;



    private string MapboxToken = "pk.eyJ1IjoiYXJ0ZWxvIiwiYSI6ImNsZWtrY2g0dTBtOGQzcm5wNWd6ajd4OW0ifQ.kuIQLXklaS1BTG4DALtTWg";



    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        _isMainWindow = scene.name == "Main" || scene.name == "UserProfile";
    }
    // Start is called before the first frame update
    void Start()
    {

        Title.text = data.Title;
        Discrption.text = data.Description;
        imageTexturer = data.URL;
        Debug.Log(data.URL);


        SetImage();
        GetDistanceAndDuration();
    }

    public void SetImage()
    {

        // get Refrence to Firebase Storage Defualt;
        storage = FirebaseStorage.DefaultInstance;

        // get Refrence to Bucket 
        stoeageRefrence = storage.GetReferenceFromUrl("gs://artelo-f7475.appspot.com");

        // get Refrence to Image in Buket
        StorageReference image = stoeageRefrence.Child("/imges/" + imageTexturer);


        // Fetch the download URL
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Download URL: " + task.Result);
                // ... now download the file via WWW or UnityWebRequest.
                Debug.Log(Convert.ToString(task.Result.ToString()));
                StartCoroutine(GetImageUrl(Convert.ToString(task.Result.ToString())));
            }
            else
            {
                Debug.Log("Http Error" + task.Exception.ToString());
            }
        });


    }


    IEnumerator GetImageUrl(string Mediaurl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(Mediaurl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("request.isNetworkError" + request.error);
        }
        else
        {
            Image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

    }

    public void onGoPress()
    {

        if (!_isMainWindow)
        {
            MapboxRoute rout = gameObject.GetComponentInParent<ExploreSponer>().route;
            if (rout)
            {
                Debug.Log("onGoPress");
                Debug.Log(rout.Settings.Language);

                rout.gameObject.SetActive(false);
                settings.RouteSettings = new RouteSettings();
                settings.RouteSettings.To = new RouteWaypoint { Type = RouteWaypointType.Location };

                settings.RouteSettings.To.Location = new ARLocation.Location(data.Latitude, data.Longitude, data.Altitude);
                settings.RouteSettings.To.Location.AltitudeMode = AltitudeMode.GroundRelative;

                rout.Settings.RouteSettings.To = settings.RouteSettings.To;
                rout.gameObject.SetActive(true);
                GameObject.FindGameObjectWithTag("Explore").SetActive(false);

            }
        }

        else
        {

            PlayerPrefs.SetString("Latitude", data.Latitude.ToString());
            PlayerPrefs.SetString("Longitude", data.Longitude.ToString());
            PlayerPrefs.SetString("altitud", data.Altitude.ToString());

            StopAllCoroutines();
            SceneManager.LoadScene("OuterNavigation");
        }



    }

    private void GetDistanceAndDuration()
    {
        RouteWaypoint start = new RouteWaypoint { Type = RouteWaypointType.UserLocation };
        if (_isMainWindow)
        {
            AREarthManager EarthManager = GameObject.FindObjectOfType<AREarthManager>();
            start = new RouteWaypoint { Type = RouteWaypointType.Location };
            if (EarthManager)
            {
                Debug.Log("Fouiiiiiiiiiiiiiiiiiiiiind");
                if (EarthManager.EarthTrackingState == TrackingState.Tracking)
                {
                    GeospatialPose point = EarthManager.CameraGeospatialPose;

                    start.Location = new ARLocation.Location(point.Latitude, point.Longitude, point.Altitude);
                }
            }
            else
            {
                Debug.Log("not Found");
            }

        }



        //start.Location = new ARLocation.Location(31.89629178243587, 35.17527455943225, 790.1016702932911);
        RouteWaypoint End = new RouteWaypoint { Type = RouteWaypointType.Location };
        End.Location = new ARLocation.Location(data.Latitude, data.Longitude, data.Altitude);

        StartCoroutine(LoadRoute(start, End));

    }

    public System.Collections.IEnumerator LoadRoute(RouteWaypoint start, RouteWaypoint end)
    {
        MapboxApi mapboxApi = new MapboxApi(MapboxToken, MapboxApiLanguage.English_US);
        Debug.Assert(mapboxApi != null);

        var loader = new RouteLoader(mapboxApi);

        yield return loader.LoadRoute(start, end);

        if (loader.Error != null)
        {

            Debug.Log("Error Accured" + loader.Error);
        }
        else
        {


            distance.text = loader.Result.routes[0].distance >= 1000 ?
            ((float)(loader.Result.routes[0].distance / 1000)).ToString() + "-Km" : ((float)(loader.Result.routes[0].distance)).ToString() + "-M";
            duration.text = loader.Result.routes[0].duration >= 216000 ?
             ((float)(loader.Result.routes[0].duration / 60 / 60)).ToString() + "-Hr" : ((float)(loader.Result.routes[0].duration / 60)).ToString() + "-Mi";

        }
    }
}
