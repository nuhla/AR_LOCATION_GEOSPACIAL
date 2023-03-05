using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextTargetBehavoir : MonoBehaviour, IPointerClickHandler
{

    // Start is called before the first frame update
    private void Awake()
    {
        var debuggObjGameOb = GameObject.FindGameObjectWithTag("debuggText").gameObject;
        if (debuggObjGameOb = null)
        {
            Debug.Log("not Found");
        }


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            string eventName = "onPotoTapped";
            EventManger.OnTriggerEvent(eventName);
        }
    }

}
