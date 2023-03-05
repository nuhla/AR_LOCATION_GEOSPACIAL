using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ARFoundation ARCoreExtensions
using Google.XR.ARCoreExtensions;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
namespace AR_Fukuoka
{
    public class SampleScript : MonoBehaviour
    {
        // Start is called before the first frame update//void Start()

        //GeospatialAPI
        public AREarthManager EarthManager;

        //GeospatialAPI
        public GameObject anchorPrefab = null;

        public ARAnchorManager Aranchormanger;
        public VpsInitializer Initializer;

        private ARGeospatialAnchor anchor;

        public Text OutputText;

        

        public Text DebugText;
        private Mercator.GeoCoordinate geoCoord;
        private GameObject anchoredAsset = null;
        private bool isCalled = false;
        void Start()
        {
            OutputText.text += "start";

        }

        // Update is called once per frame

        // Display lat, lng, alt and thier percision 
        void ShowTrackingFailling(string status)
        {
            OutputText.text = "Traking status is :" + status + " , Tracking is not Active";
        }


        // Display lat, lng, alt and thier percision 
        void ShowTrackingInfo(string status, GeospatialPose pose)
        {
            Vector3 mypalce = new Vector3(((float)pose.Latitude), ((float)pose.Longitude), ((float)pose.Altitude));
            Vector3 posPlce = new Vector3(((float)31.896451), ((float)35.175258), ((float)815.225));
            var distance = mypalce - posPlce;


            OutputText.text = string.Format(
            "Latitude/Longitude: {0}°, {1}°\n" +
            "Horizontal Accuracy: {2}m\n" +
            "Altitude: {3}m\n" +
            "Vertical Accuracy: {4}m\n" +
            "Heading: {5}°\n" +
            "Heading Accuracy: {6}°\n" +
            "{7} \n"
            ,
            pose.Latitude.ToString("F6"),  //{0}
            pose.Longitude.ToString("F6"), //{1}
            pose.HorizontalAccuracy.ToString("F6"), //{2}
            pose.Altitude.ToString("F2"),  //{3}
            pose.VerticalAccuracy.ToString("F2"),  //{4}
            pose.EunRotation.ToString("F1"),   //{5}
            pose.OrientationYawAccuracy.ToString("F1"),   //{6}

            status //{7}
);

        }
        void Update()
        {


            string status = "";


            // if inialization fails or you do not want to track , do nothing and return 
            if (Initializer.IsReady || EarthManager.EarthTrackingState != TrackingState.Tracking)
            {
                status = EarthManager.EarthTrackingState.ToString() + Initializer.IsReady + "\n";
                ShowTrackingFailling(status);
                return;
            }
            // get tracking resude
            GeospatialPose geoPose = EarthManager.CameraGeospatialPose;

            
            // show result
            ShowTrackingInfo(status, geoPose);

            if (isCalled == false)
            {


                //OutputText.text = "\nintry  - Prephap IS NULL" + "\n";
                isCalled = true;
                try
                {

                    geoCoord =
                new Mercator.GeoCoordinate(31.896468, 35.175253, 814.75);





                    var anchorRot = Quaternion.AngleAxis(0, new Vector3(0.0f, 1.0f, 0.0f));

                    //nchor = Aranchormanger.ResolveAnchorOnTerrain(geoCoordAhead.latitude,
                    //geoCoordAhead.longitude, geoCoordAhead.altitude, anchorRot);
                    StartCoroutine(PlaceTerrainAnchor(31.896468, 35.175253, 0));



                    //anchoredAsset = Instantiate(anchorPrefab, anchor.transform);

                    //OutputText.text += anchoredAsset.transform.position.ToString() + "\n";

                    anchoredAsset.SetActive(false);
                    anchoredAsset.transform.SetParent(anchor.transform, false);
                    anchoredAsset.transform.localPosition = Vector3.zero;
                    //anchoredAsset.transform.localScale = Vector3.one;
                    anchoredAsset.SetActive(true);
                    //anchoredAsset.transform.SetParent(anchor.transform, true);

                    OutputText.text += anchor.transform.position.ToString();
                    // ------------------------ creat geopos Anchor ------------------// 





                }
                catch (Exception ex)
                {
                    OutputText.text += ex.ToString();
                }

                DebugText.text = "Start Coroutine ";




            }

        }

        private IEnumerator PlaceTerrainAnchor(double latitude, double longitude, float altitude)

        {
         

            anchor = Aranchormanger.ResolveAnchorOnTerrain(latitude,
             longitude, 
             altitude,
              Quaternion.identity);
            DebugText.text = "TerrainAnchorState = TaskInProgress ";

            while (anchor.terrainAnchorState == TerrainAnchorState.TaskInProgress)
            {
                DebugText.text = "TerrainAnchorState = TaskInProgress ";
                yield return null;
            }

            anchoredAsset = Instantiate(anchorPrefab, anchor.transform);
            anchoredAsset.SetActive(false);
            anchoredAsset.transform.SetParent(anchor.transform, false);
            anchoredAsset.transform.localPosition = Vector3.zero;
            //anchoredAsset.transform.localScale = Vector3.one;
            anchoredAsset.SetActive(true);
            DebugText.text += "Start Instantiate ";

        }


    }
}


