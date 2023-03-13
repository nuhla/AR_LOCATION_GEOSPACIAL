using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{

    private float TimeWaitingToLoad = 9.0f;


    public GameObject LoadSceen;

    public GameObject Screen1;
    public GameObject Screen2;
    public GameObject Screen3;
    public GameObject ScreenLogIn;
    public GameObject SignInByPhon;
    
    // Start is called before the first frame update
    private void OnEnable()
    {

        LoadSceen.gameObject.SetActive(true);
        Screen1.SetActive(false);
        Screen2.SetActive(false);
        Screen3.SetActive(false);
        ScreenLogIn.SetActive(false);
        SignInByPhon.SetActive(false);
        

       StartCoroutine(LoadingAnimation());

    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadingAnimation(){


        var userID = UnityEngine.PlayerPrefs.GetString("UserID");
        

        yield return new WaitForSecondsRealtime(9);
        if (userID == null || userID == "" )
        {
            
            LoadSceen.gameObject.SetActive(false);
            Screen1.SetActive(true);
            Screen2.SetActive(false);
            Screen3.SetActive(false);
            ScreenLogIn.SetActive(false);
            SignInByPhon.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("Main");
        }

    }
    
}
