using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManger : MonoBehaviour
{

    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManger eventManger;

    public static EventManger instance
    {

        get
        {
            if (!eventManger)
            {
                eventManger = FindObjectOfType(typeof(EventManger)) as EventManger;
                if (!eventManger)
                {
                    Debug.Log("There need to be one active EventManager script on GameObject in Youe Game");
                }
                else
                {
                    eventManger.Init();
                    Debug.Log("Created A new Event eventDictionary");
                }

            }
            return eventManger;

        }
    }


    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
            Debug.Log("Created A new Event eventDictionary");
        }
    }

    public static void StartListening(string eventName, UnityAction Listner)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(Listner);
            Debug.Log("StartListening " + eventName);

        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(Listner);
            instance.eventDictionary.Add(eventName, thisEvent);
            Debug.Log(" else StartListening " + eventName);
        }
    }

    public static void StopListening(string eventName, UnityAction listner)
    {
        if (eventManger == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listner);
        }
    }

    public static void OnTriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }

    }

}




