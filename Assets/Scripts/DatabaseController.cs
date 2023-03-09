using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using Newtonsoft.Json;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;

public class DatabaseController : MonoBehaviour
{
    // Start is called before the first frame update

    private DatabaseReference databaseReference;
    private FirebaseAuth firebaseAuth;
    private FirebaseUser firebaseUser;

    [SerializeField]
    TMP_InputField phoneNumber;

    [SerializeField]
    TMP_InputField outPutCode;

    [SerializeField]
    TMP_Text debug;

    public Button VarifyCode;
    public Button SignIn;


    [SerializeField]
    TMP_InputField CountryCode;
    private uint phoneAuthTimeoutMs = 60 * 1000;
    PhoneAuthProvider phoneAuthProvider;

    private string VerificationId;
    private Credential credential;
    public static string userID;


    private void Awake()
    {

        Firebase.AppOptions options = new Firebase.AppOptions();
        options.ApiKey = "AIzaSyBTvLLETfBfCGotIX01rbWFdB-D5elJA-Y";
        options.AppId = "277860542920";
        options.MessageSenderId = "277860542920-maso2ms71job15vced7p9o82f55hvjm3.apps.googleusercontent.com";
        options.ProjectId = "artelo-f7475";
        options.StorageBucket = "artelo-f7475.appspot.com";
        // options.DatabaseUrl="https://arpal-377917-default-rtdb.firebaseio.com";


        var app = Firebase.FirebaseApp.Create(options);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
     {
         var dependencyStatus = task.Result;
         if (dependencyStatus == Firebase.DependencyStatus.Available)
         {
             // Create and hold a reference to your FirebaseApp,
             // where app is a Firebase.FirebaseApp property of your application class.
             app = Firebase.FirebaseApp.DefaultInstance;
             firebaseAuth = FirebaseAuth.DefaultInstance;
             // Get a reference to the storage service, using the default Firebase App
             //FirebaseStorage storage = FirebaseStorage.DefaultInstance;
             isUserIsSignedIn();

             // Set a flag here to indicate whether Firebase is ready to use by your app.
         }
         else
         {
             UnityEngine.Debug.LogError(System.String.Format(
               "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
             Debug.Log(" Firebase Unity SDK is not safe to use here.");
         }
     });

        Debug.Log("in firbase start");
    }

    private void isUserIsSignedIn()
    {
        userID = UnityEngine.PlayerPrefs.GetString("UserID");
        if (userID == null || userID == "")
        {
            // User Is Not Logged In
        }
    }

    /// <summary>
    /// Creates a New Anchore In DataBase.
    /// </summary>
    // CreateAnchore;
    private void CreateAnchore(string Title,
     string Description,
      string FullDiscription,
      double Latitude
     , double Longitude,
      double Altitude,
       double Heading,
       float Qua_Z,
       float Qua_Y,
       float Qua_x)
    {

        AnchoreData anchor = new AnchoreData
        (Title, Description, FullDiscription, Latitude
        , Longitude, Altitude, Heading, Qua_x, Qua_Y, Qua_Z);
        string json = JsonUtility.ToJson(anchor);



    }

    /// <summary>
    /// Takes The User Phone and Enable User to loge in Using It and create a new User ifnot Exist.
    /// </summary>
    // SignIn_PhonAuth;
    public void SignIn_PhonAuth()
    {


        debug.text = "Waiting For Code";
        VarifyCode.gameObject.SetActive(false);
        phoneAuthProvider = PhoneAuthProvider.GetInstance(firebaseAuth);
        phoneAuthProvider.VerifyPhoneNumber(CountryCode.text + phoneNumber.text, phoneAuthTimeoutMs, null,
          verificationCompleted: (credential) =>
          {
              debug.text = "verificationCompleted  : ";

              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
          },
          verificationFailed: (error) =>
          {
              debug.text = "verification Failed  - " + error;
              VarifyCode.gameObject.SetActive(false);
              SignIn.enabled = false;

              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
          },
          codeSent: (id, token) =>
          {
              VerificationId = id;
              VarifyCode.gameObject.SetActive(true);
              SignIn.gameObject.SetActive(false);

              //SignInWithCredential(id, Code);
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.

          },
          codeAutoRetrievalTimeOut: (id) =>
          {

              string[] varvtcartionCode = id.Split("-");
              string code = varvtcartionCode[varvtcartionCode.Length - 1];
              VarifyCode.gameObject.SetActive(true);
              debug.text = "codeAutoRetrievalTimeOut  : " + code;
              // Called when the auto-sms-retrieval has timed out, based on the given
              // timeout parameter.
              // `id` contains the verification id of the request that timed out.
          });
    }

    /// <summary>
    /// Checkes the Credantals after Entering the Code.
    /// </summary>
    // CheckandVarifyCode;

    public void CheckandVarifyCode()
    {

        string Code = outPutCode.text;

        SignInWithCredential(Code);

    }


    /// <summary>
    /// Takes the VerificationId after Sending the SMS Code and checks the user Credintals.
    // and Save user Id in Phone Memory 
    /// </summary>
    // CheckandVarifyCode;
    private void SignInWithCredential(string verificationCode)
    {
        credential =
           phoneAuthProvider.GetCredential(VerificationId, verificationCode);

        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " +
                               task.Exception);
                return;
            }

            firebaseUser = task.Result;
            UnityEngine.PlayerPrefs.SetString("UserID", firebaseUser.UserId);
            userID = firebaseUser.UserId;
            debug.text = ("User signed in successfully");
            // This should display the phone number.
            debug.text += ("Phone number: " + firebaseUser.PhoneNumber);
            UnityEngine.PlayerPrefs.SetString("PhoneNumber", firebaseUser.PhoneNumber);
            // The phone number providerID is 'phone'.
            debug.text += ("Phone provider ID: " + firebaseUser.ProviderId);
            UnityEngine.PlayerPrefs.SetString("ProviderId", firebaseUser.ProviderId);
        });
    }


    /// <summary>
    /// Takes the VerificationId after Sending the SMS Code and checks the user Credintals.
    // and Save user Id in Phone Memory 
    /// </summary>
    // CheckandVarifyCode;
    public void UserLogOut()
    {
        try
        {
            firebaseAuth.SignOut();
            UnityEngine.PlayerPrefs.DeleteKey("UserID");
            UnityEngine.PlayerPrefs.DeleteKey("PhoneNumber");
            UnityEngine.PlayerPrefs.DeleteKey("ProviderId");
        }
        catch (Exception ex)
        {
            Debug.Log("Error : " + ex.Message);
        }

    }


}
