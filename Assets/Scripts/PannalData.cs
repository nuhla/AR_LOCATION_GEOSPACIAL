using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PannalData : MonoBehaviour
{
    public AnchoreData data = new AnchoreData();
    [SerializeField]
    private TMP_Text Title;
    [SerializeField]
    private TMP_Text Discription;


    private void OnEnable()
    {
        Title.text = data.Title;
        Discription.text = data.FullDiscription;
    }

    // Update is called once per frame

}
