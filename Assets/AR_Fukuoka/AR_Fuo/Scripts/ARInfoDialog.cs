using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ARInfoDialog : MonoBehaviour
{


    public GameObject Prefab;

    /// <summary>
    /// The Balloon fade start distance (from the user camera)
    /// </summary>
    static float ARINfODIAlOG_FADE_MIN_DIST_TO_CAM = 10f;

    /// <summary>
    /// The Balloon fade end distance (from the user camera)
    /// </summary>
    static float ARINfODIAlOG_FADE_MAX_DIST_TO_CAM = 14f;

    /// <summary>
    /// The Balloon fade range
    /// </summary>
    static float DIST_TO_CAM_RANGE = ARINfODIAlOG_FADE_MAX_DIST_TO_CAM - ARINfODIAlOG_FADE_MIN_DIST_TO_CAM;

    /// <summary>
    /// The average/estimated height of the user camera
    /// (So a balloon can be placed on the floor using the height of the camera)
    /// </summary>
    static public float ESTIMATED_CAM_HEIGHT_FROM_FLOOR = 5.0f;

    /// <summary>
    /// The root transform the the Balloon
    /// </summary>
    public Transform ARInfoDialogRoot;

    /// <summary>
    /// Pointer to the balloon at the end of the string
    /// </summary>
    public Transform ARInfoDialogTransform;




    /// <summary>
    /// A pointer to the BalloonData object
    /// </summary>
    //private ARInfoDialogData _Data = null;
    //public ARInfoDialogData Data { get { return _Data; } }

    // ====================================================================

    /// <summary>
    /// A flag for the debug canvas visibility
    /// </summary>
    private bool _debugCanvasVisible = false;

    private void Awake() {
        // balloonWasPopped = new BalloonEvent();

        // _Data = new BalloonData();
        // _Data.SetVisualChangesListener(this);
            
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBalloonCamYPosFadeAndDistToCamera(float camYPosWorld, float distToCamera, Vector3 camPos, bool adjustFade)
    {
        float distPercent = Mathf.Max(0, Mathf.Min(1f,
                                  (distToCamera - ARINfODIAlOG_FADE_MIN_DIST_TO_CAM) / DIST_TO_CAM_RANGE
                              ));
        // distPercent = 0.0 (when the Balloon is close to the camera)
        // distPercent = 1.0 (when the Balloon is far from the camera)

        Vector3 p = this.ARInfoDialogRoot.position;

        // Set the height of the balloon at some percentage between 
        // the camera height and the anchor's natural altitude

        // Always set the height to the estimated floor height based on the camera
        p.y = (camYPosWorld - ESTIMATED_CAM_HEIGHT_FROM_FLOOR);

        this.ARInfoDialogRoot.position = p;
    }

    // ==============================================

    /// <summary>
    /// Set a new BalloonData object
    /// </summary>
    /// <param name="newBalloonData">The new BalloonData object</param>


}
