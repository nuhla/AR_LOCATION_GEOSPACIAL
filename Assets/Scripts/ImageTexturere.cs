using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using Firebase;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Extensions;
using UnityEngine.UI;
using System;

public class ImageTexturere : MonoBehaviour
{

    private string imageTexturer;
    public GameObject parent;

    private FirebaseStorage storage;
    private StorageReference stoeageRefrence;
    private RawImage image;

    public string type;


    private void Awake()
    {

        if (type == "Explore")
        {
            Debug.Log("In Texture");
            imageTexturer = parent.gameObject.GetComponent<ExploreItemManager>().data.URL;
            Debug.Log(parent.gameObject.GetComponent<ExploreItemManager>().data.URL + "55555555555555555");
        }
        else
        {
            image = gameObject.GetComponent<RawImage>();
            imageTexturer = parent.gameObject.GetComponent<PannalData>().data.URL;
            //imageTexturer = "Hebron.jpg";
        }


    }


    private void Start()
    {
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
            Debug.Log("request.isNetworkError"+ request.error);
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

    }
}
