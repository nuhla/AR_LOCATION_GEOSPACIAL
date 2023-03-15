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
        // if (Title != null)
        // {
        //     Title.text = data.Title;
        //     Discription.text = data.Description;
        // }
        // else
        // {
        //     Discription.text = data.FullDiscription;
        // }

    }

    public void OpenReadMore()
    {
        ReadMore.gameObject.GetComponent<PannalData>().data = data;
        Title.text = data.Title;
        Discription.text = data.Description;
        // ReadMore.gameObject.GetComponent<PannalData>().data.Title = data.Title;
        ReadMore.SetActive(true);

    }

    public void OpenReadLess()
    {
        ReadMore.gameObject.GetComponent<PannalData>().data = data;
        // ReadMore.gameObject.GetComponent<AnchoreData>().Title = data.Title;
        // Title.text = data.Title;
        Discription.text = data.FullDiscription;
        ReadMore.SetActive(true);

    }

    public void OpenMe()
    {
        ReadMore.gameObject.GetComponent<PannalData>().data = data;
        Title.text = data.Title;
        Discription.text = data.Description;
        gameObject.SetActive(true);

    }

    public void OpenGo()
    {
        PlayerPrefs.SetString("Latitude", data.Latitude.ToString());
        PlayerPrefs.SetString("Longitude", data.Longitude.ToString());
        PlayerPrefs.SetString("altitud", data.Altitude.ToString());

    }

    public void StartNavgation()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("OuterNavigation", LoadSceneMode.Single);
    }

    public void GoToUserProfile()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("UserProfile", LoadSceneMode.Single);
    }
    public void ReloadScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void Home()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

}
