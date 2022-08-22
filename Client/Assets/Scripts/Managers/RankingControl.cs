using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SocialPlatforms;

#if UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#endif

#if UNITY_ANDROID


#endif

public class RankingControl : MonoBehaviour
{
    public void OnClickLeaderBoard()
    {
        if (Social.localUser.authenticated == true)
        {
            Social.ShowLeaderboardUI();
        }
    }
}
