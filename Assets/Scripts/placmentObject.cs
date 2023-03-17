using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placmentObject : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsSekected { get; set; }



    public GameObject Parent;

    public AnchoreData data;
    private void Awake()
    {

        data = Parent.gameObject.GetComponent<MarkerData>().marker;
    }
}
