using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GPGS : MonoBehaviour
{
    public TMPro.TMP_Text loginInfo;
    public UnityEngine.UI.RawImage userImage;

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        //PlayGamesPlatform.Activate();
        //Social.localUser.Authenticate(ProcessAuthentication);
    }

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
}
