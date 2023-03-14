using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExploreItemManager : MonoBehaviour
{

    public TMP_Text Discrption;
    public TMP_Text Title;
    public Text duration ;
    public Text distance;
    public RawImage Image;
    private FirebaseStorage storage;
    private StorageReference stoeageRefrence;
    

    public GeospatialAnchorHistory data = new GeospatialAnchorHistory();
    private string imageTexturer;
    private object parent;

    public double distanceOfLocation;
  
    // Start is called before the first frame update
    void Start()
    {
        
        Title.text = data.Title;
        Discrption.text = data.Description;
        imageTexturer = data.URL;
        Debug.Log(data.URL);
      

        SetImage();
    }

    public void SetImage()
    {

        // get Refrence to Firebase Storage Defualt;
        storage = FirebaseStorage.DefaultInstance;

        // get Refrence to Bucket 
        stoeageRefrence = storage.GetReferenceFromUrl("gs://artelo-f7475.appspot.com");

        // get Refrence to Image in Buket
        StorageReference image = stoeageRefrence.Child("/imges/" + imageTexturer);


        // Fetch the download URL
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Download URL: " + task.Result);
                // ... now download the file via WWW or UnityWebRequest.
                Debug.Log(Convert.ToString(task.Result.ToString()));
                StartCoroutine(GetImageUrl(Convert.ToString(task.Result.ToString())));
            }
            else
            {
                Debug.Log("Http Error" + task.Exception.ToString());
            }
        });


    }


    IEnumerator GetImageUrl(string Mediaurl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(Mediaurl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("request.isNetworkError" + request.error);
        }
        else
        {
            Image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

    }
}
