using System.Collections;
using System.Collections.Generic;
using ARLocation.MapboxRoutes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PannalData : MonoBehaviour
{
    public GameObject ReadMore;
    private RouteLoader routeLoader;
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

    public void OpenReadMore()
    {
        ReadMore.gameObject.GetComponent<AnchoreData>().FullDiscription = data.FullDiscription;
        ReadMore.gameObject.GetComponent<AnchoreData>().Title = data.Title;
        ReadMore.SetActive(true);

    }

    public void OpenGo()
    {
        PlayerPrefs.SetString("Latitude", data.Latitude.ToString());
        PlayerPrefs.SetString("Longitude", data.Longitude.ToString());
        PlayerPrefs.SetString("altitud", data.Altitude.ToString());
        StopAllCoroutines();
        SceneManager.LoadScene("OuterNavigation");
    }
    // Update is called once per frame

}
