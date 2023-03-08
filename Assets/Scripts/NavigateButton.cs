using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateButton : MonoBehaviour
{
    // Start is called before the first frame update
    public AnchoreData data = new AnchoreData();
    public GameObject Parent;

    // Update is called once per frame
    void Awake()
    {
        data = Parent.gameObject.GetComponent<MarkerData>().marker;
    }
}
