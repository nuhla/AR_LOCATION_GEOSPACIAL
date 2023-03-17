using System.Collections;
using System.Collections.Generic;
using ARLocation.MapboxRoutes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PannalData : MonoBehaviour
{
    public GameObject NextPannel;

    private RouteLoader routeLoader;

    public AnchoreData data = new AnchoreData();
    [SerializeField]
    private TMP_Text Title;
    [SerializeField]
    private TMP_Text Discription;

    [SerializeField]
    private TMP_Text FullDiscription;

    [SerializeField]
    private RawImage imge;


    private void OnEnable()
    {


        if (Discription != null)
        {
            Discription.text = data.Description;
        }
        if (FullDiscription != null)
        {
            FullDiscription.text = data.FullDiscription;
        }
        if (
            Title != null
        )
        {
            Title.text = data.Title;
        }
        if (imge != null)
        {
            imge.gameObject.GetComponent<ImageTexturere>().imageTexturer = data.URL;
            ImageTexturere TxImg = imge.gameObject.GetComponent<ImageTexturere>();
            TxImg.SetImage();
        }
    }



    public void OpenReadMore()
    {
        Debug.Log(data.URL + "data.URL;");
        NextPannel.gameObject.GetComponent<PannalData>().data = data;
        NextPannel.SetActive(true);
        gameObject.SetActive(false);

    }

    public void OpenReadLess()
    {


        NextPannel.SetActive(true);

    }

    public void OpenMe()
    {
        // ReadMore.gameObject.GetComponent<PannalData>().data = data;
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
