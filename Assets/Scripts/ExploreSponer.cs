using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation.MapboxRoutes;
using Firebase.Database;
using Google.XR.ARCoreExtensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ExploreSponer : MonoBehaviour
{
    public GameObject prefab;
    public GameObject parent;
    public bool _CreatChilds = false;

    private string MapboxToken="pk.eyJ1IjoiYXJ0ZWxvIiwiYSI6ImNsZWtrY2g0dTBtOGQzcm5wNWd6ajd4OW0ifQ.kuIQLXklaS1BTG4DALtTWg";
    public List<GeospatialAnchorHistory> PrefabsCollection = new List<GeospatialAnchorHistory>();
    public List<GameObject> InstantiatedGameObects = new List<GameObject>();
    private object currentPathRenderer;

    public AREarthManager EarthManager;

    // Start is called before the first frame update
    private async void Awake()
    {

        getData();


    }

    private void Update()
    {
        if (_CreatChilds == true)
        {

            foreach (GeospatialAnchorHistory item in PrefabsCollection)
            {
                CreateChild(item);
            }
            _CreatChilds = false;
        }

    }

    public void getData()
    {


        StartCoroutine(getAnchores((DataSnapshot snapshot) =>
            {


                using (var sequenceEnum = snapshot.Children.GetEnumerator())
                {
                    for (int i = 0; i < snapshot.ChildrenCount; i++)
                    {

                        while (sequenceEnum.MoveNext())
                        {

                            var id = sequenceEnum.Current.Key;
                            try
                            {

                                    
                                
                                IDictionary dictContent = (IDictionary)sequenceEnum.Current.Value;

                                var json = JsonConvert.SerializeObject(dictContent);



                                GeospatialAnchorHistory historyanchor =
                                JsonConvert.DeserializeObject<GeospatialAnchorHistory>(json);
                                if(!(historyanchor.Title =="" && historyanchor.Description == "" || historyanchor.Title== null)){
                                    PrefabsCollection.Add(historyanchor);
                                }
                              
                               


                            }
                            catch (Exception ex)
                            {
                                Debug.Log("Error" + ex.Message);
                            }
                        }

                        Debug.Log("Finish Loo[]");
                        _CreatChilds = true;

                    }



                }

            }
            ));


    }

    public void CreateChild(GeospatialAnchorHistory data)
    {
        GameObject Geo = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        Geo.gameObject.GetComponent<ExploreItemManager>().data = data;
        Geo.transform.localScale = new Vector3(1f,1f,1f);

        
        Debug.Log(Geo.gameObject.GetComponent<ExploreItemManager>().data + "8888888888888888888");
        Geo.transform.parent = parent.transform;
        

        getDistance(data);
        InstantiatedGameObects.Add(Geo);



    }


    public void getDistance(GeospatialAnchorHistory data){

       // GeospatialPose point = EarthManager.CameraGeospatialPose;
        var api = new MapboxApi(MapboxToken);
        var loader = new RouteLoader(api);
        // StartCoroutine(
        //         api.QueryRoute()
    }
    public IEnumerator getAnchores(Action<DataSnapshot> onCallback)
    {
        
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference; 

        yield return FirebaseDatabase.DefaultInstance.GetReference("anchors").GetValueAsync().ContinueWith(task =>
         {
             if (task.IsFaulted)

             {
                 Debug.Log("No Data.....");
                 return;
             }

             else if (task.IsCompletedSuccessfully)
             {
                 DataSnapshot snapshot = task.Result;
                


                 onCallback.Invoke(snapshot);

             }
         });

        
    }



}
