using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;

#elif UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#endif


public class GPGS : MonoBehaviour
{
    public TMPro.TMP_Text loginInfo;
    public UnityEngine.UI.RawImage userImage;

    private void Start()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif

#if UNITY_ANDROID || UNITY_IOS
        //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    string infos = string.Empty;
                    infos += "=Name: " + Social.localUser.userName + "\n";
                    loginInfo.text = infos;
                    userImage.texture = Social.localUser.image;
                }
                else
                    Debug.Log("Authentication failed");
            });
        }
        //GameCenterLogin();
#else
        loginInfo.gameObject.SetActive(false);
        userImage.gameObject.SetActive(false);
#endif

        //PlayGamesPlatform.Activate();
        //Social.localUser.Authenticate(ProcessAuthentication);
    }

#if UNITY_ANDROID
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            string infos = string.Empty;
            infos += "=ID: " + Social.localUser.id + "\n";
            infos += "=Name: " + Social.localUser.userName + "\n";
            loginInfo.text = infos;
            userImage.texture = Social.localUser.image;

        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            loginInfo.text = "==Logout State: " + status.ToString();
        }
    }
#endif

#if UNITY_IOS

    public void GameCenterLogin()
    {
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success to true");
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Success to authenticate");
                }
                else
                {
                    Debug.Log("Faile to login");
                }
            });
        }
    }


#endif
}
