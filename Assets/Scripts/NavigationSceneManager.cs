using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationSceneManager : MonoBehaviour
{
    
    public void GoToHomeScen(){
        StopAllCoroutines();
        
        SceneManager.LoadScene("Main");

    }

    public void UserProfile(){
        SceneManager.LoadScene("UserProfile");
    }

    public void OpenExplore(){

        Debug.Log("Open Explore");
    }

    
   
}
