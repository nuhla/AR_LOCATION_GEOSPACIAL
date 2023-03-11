using System.Collections;
using System.Collections.Generic;
using ARLocation;
using ARLocation.MapboxRoutes;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public AnchoreData data = new AnchoreData();
    public GameObject Parent;


    private void Awake()
    {

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



                        //---------------- For Debugging Purpose --------------//
                        MeshRenderer meshRenderer = navigateButton.GetComponent<MeshRenderer>();
                        //---------------- For Debugging Purpose --------------//
                        meshRenderer.material.color = InactiveColor;

                        PlayerPrefs.SetString("Latitude", navigateButton.data.Latitude.ToString());
                        PlayerPrefs.SetString("Longitude", navigateButton.data.Longitude.ToString());
                        PlayerPrefs.SetString("altitud", navigateButton.data.Altitude.ToString());
                        StopAllCoroutines();
                        SceneManager.LoadScene("OuterNavigation");
                    }

                }

            }

        }
    }



}
