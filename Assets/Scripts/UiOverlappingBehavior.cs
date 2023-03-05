using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class UiOverlappingBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject OverLap;
    private Text Text;
    private UnityAction onPotoTapped;
    private Text debuggText;

    void Awake()
    {
        //find the pannal with name Menu 
        OverLap = transform.Find("DataViwer").gameObject;
        OverLap.SetActive(false);
        var debuggObjGameOb = GameObject.FindGameObjectWithTag("debuggText").gameObject;
        if (debuggObjGameOb = null)
        {
            Debug.Log("not Found");
        }
        else
        {
           
            
            onPotoTapped = new UnityAction(DisplayMenu);
        }



    }
    private void OnEnable()
    {
        EventManger.StartListening("onPotoTapped", onPotoTapped);
        
    }
    private void OnDisable()
    {
        EventManger.StopListening("onPotoTapped", onPotoTapped);

    }
    void openMenu()
    {
        OverLap.SetActive(true);

    }
    void DisplayMenu()
    {
        openMenu();
        
    }




}
