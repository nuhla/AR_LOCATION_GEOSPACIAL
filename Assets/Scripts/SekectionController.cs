using System.Collections;
using System.Collections.Generic;
using ARLocation;
using ARLocation.MapboxRoutes;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static ARLocation.MapboxRoutes.MapboxRoute;

public class SekectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject Pannel;


    [SerializeField]
    public Camera arCamera;

    [SerializeField]
    private Color activeColor = Color.blue;

    [SerializeField]
    private Color InactiveColor = Color.green;

    private Vector2 touchPosition = default;

    public Text DebugText;
    private string MapboxToken = "pk.eyJ1IjoiZG1iZm0iLCJhIjoiY2tyYW9hdGMwNGt6dTJ2bzhieDg3NGJxNyJ9.qaQsMUbyu4iARFe0XB2SWg";
    private MapboxRoute route;
    private AREarthManager EarthManager;

    private RouteWaypoint from;
    private RouteWaypoint to;
    public AnchoreData data = new AnchoreData();
    public GameObject Parent;
    public AbstractRouteRenderer RoutePathRenderer;
    public AbstractRouteRenderer NextTargetPathRenderer;


    private AbstractRouteRenderer currentPathRenderer => s.LineType == LineType.Route ? RoutePathRenderer : NextTargetPathRenderer;
    private string _MapBpxStatus;
    SettingsData settings = new SettingsData();
    public LineType PathRendererType
    {
        get => s.LineType;
        set
        {
            if (value != s.LineType)
            {
                currentPathRenderer.enabled = false;
                s.LineType = value;
                currentPathRenderer.enabled = true;


                route.RoutePathRenderer = currentPathRenderer;

            }
        }
    }

    private class State
    {
        public string QueryText = "";
        public List<GeocodingFeature> Results = new List<GeocodingFeature>();
        //public View View = View.SearchMenu;

        public LineType LineType = LineType.NextTarget;
        public string ErrorMessage;
    }

    private State s = new State();
    private RouteResponse currentResponse;

    private void Awake()
    {
        route = GameObject.FindObjectOfType<MapboxRoute>();
        if (route == null)
        {
            _MapBpxStatus = "MapboxRout was not Found";
        }
        else
        {
            _MapBpxStatus = "MapboxRout was Found" + route.ToString();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObect;

                if (Physics.Raycast(ray, out hitObect))
                {
                    //---------------- Configuring Our Target Objects --------------/
                    placmentObject placmentObject = hitObect.transform.GetComponent<placmentObject>();
                    NavigateButton navigateButton = hitObect.transform.GetComponent<NavigateButton>();

                    if (placmentObject != null)
                    {

                        //---------------- For Debugging Purpose --------------//
                        MeshRenderer meshRenderer = placmentObject.GetComponent<MeshRenderer>();
                        meshRenderer.material.color = InactiveColor;

                        //---------------- Set Data To Window --------------//
                        Pannel.gameObject.GetComponent<PannalData>().data = placmentObject.data;
                        Pannel.SetActive(true);

                    }

                    else if (navigateButton != null)
                    {
                        //navigateButton.data;
                        RoutOrgnizer rout = GameObject.FindObjectOfType<RoutOrgnizer>();
                        rout.anchoreData = navigateButton.data;

                        //---------------- For Debugging Purpose --------------//
                        MeshRenderer meshRenderer = navigateButton.GetComponent<MeshRenderer>();
                        rout.anchoreData = navigateButton.data;
                        rout.anchoreData = navigateButton.data;
                        Pannel.gameObject.GetComponent<PannalData>().data.FullDiscription = " ---- Start Rout -----" + "\n----Latitude---- -"
                         + rout.anchoreData.Latitude + "\n----Longitude---- -" + rout.anchoreData.Longitude;
                        Pannel.SetActive(true);
                        meshRenderer.material.color = activeColor;
                        rout.StartRouting();
                    }

                }

            }

        }
    }



}
