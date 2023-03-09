using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ClosestPlaces : MonoBehaviour
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

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
