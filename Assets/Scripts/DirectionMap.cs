using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DirectionMap : MonoBehaviour
{

    public string accessToken;
    public float crentLatitud = -33.8873f;

    public float centerLongitud = 151.2189f;

    public float zoom = 12.0f;

    public int bearing = 0;

    public int pitch = 0;
    public AREarthManager EarthManager;

    public enum style
    {

        Light, Dart, Streets, Outdoors, Satellite, SatelliteStreets
    };

    public style mapStyle = style.Streets;

    public enum resolustion
    {
        low = 1, high = 2
    };

    public resolustion mapResolution = resolustion.low;
    public Text debugText;

    private int mapwidth = 800;
    private int mapHight = 400;
    private string[] styleStr = new string[] {
        "light-v10", "dark-v10",
     "streets-v11", "outdoors-v11",
     "satellite-v9", "satellite-streets-v11" };

    private string url = "";

    private bool mapIsLoading = false;
    private Rect rect;

    private bool UpdateMap = false;

    private string accessTokenLast;
    private float crentLatitudEnd = -33.8873f;
    private float centerLongitudEnd = 151.2189f;

    private float zoomLast = 12.0f;
    private int bearingLast = 0;

    private int pitchLast = 0;

    private style mapStyleLast = style.Streets;

    private resolustion mapResolutionLast = resolustion.low;


    // Start is called before the first frame update
    void Start()
    {
        GeospatialPose pos = EarthManager.CameraGeospatialPose;
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator getDurection()
    {
        url = "https://api.mapbox.com/directions/v5/mapbox/walking/" + centerLongitud + "," + crentLatitud + ")/" + crentLatitudEnd + "," + centerLongitudEnd + ",20,0/300x200?before_layer=aeroway-line&access_token=" + accessToken;
        //      url = https://api.mapbox.com/styles/v1/mapbox/streets-v11/static/path-2+f50000+ff0000(kx%7CiFzctcVc%7CvAs%5DzzvA%3F)/-121.5333,38.7242,7.55,0/300x200?before_layer=bridge-motorway-trunk&access_token=YOUR_MAPBOX_ACCESS_TOKEN
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url, false);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {

            debugText.text = www.error;
            debugText.gameObject.SetActive(true);
        }

        else
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            accessTokenLast = accessToken;
            //centerLongitudLast = centerLongitud;
            //crentLatitudLast = crentLatitud;
            zoomLast = zoom;
            pitchLast = pitch;
            mapResolutionLast = mapResolution;
            mapStyleLast = mapStyle;
            UpdateMap = true;

            debugText.text = "";
            debugText.gameObject.SetActive(false);


        }


    }


    private string FormatingJson()
    {
        return "";
    }


}
