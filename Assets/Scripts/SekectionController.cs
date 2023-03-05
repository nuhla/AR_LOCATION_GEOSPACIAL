using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


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
                    placmentObject placmentObject = hitObect.transform.GetComponent<placmentObject>();
                    if (placmentObject != null)
                    {

                        MeshRenderer meshRenderer = placmentObject.GetComponent<MeshRenderer>();
                        Pannel.gameObject.GetComponent<PannalData>().data = placmentObject.data;

                        Pannel.SetActive(true);
                        meshRenderer.material.color = InactiveColor;
                    }


                }

            }



        }
    }


}
