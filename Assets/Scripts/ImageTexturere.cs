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

    public string imageTexturer;
    public PannalData data;

    private FirebaseStorage storage;
    private StorageReference stoeageRefrence;
    public RawImage image;




    private void Awake()
    {


    }


    private void Start()
    {
        //SetImage();
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
