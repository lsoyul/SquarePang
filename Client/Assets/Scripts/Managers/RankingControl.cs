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
    private const string IOS_Ranking_ID = "score_short_test";
    private const string ANDROID_Ranking_ID = GPGSIds.leaderboard_test_score_rank;

    private void Awake()
    {
        GameBoard.onGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameBoard.onGameOver -= OnGameOver;
    }

    public void OnClickLeaderBoard()
    {
        if (Social.localUser.authenticated == true)
        {
            Social.ShowLeaderboardUI();
        }
    }

    public static void ReportScore(int newScore)
    {
        string leaderBoardId = string.Empty;

#if UNITY_ANDROID
        leaderBoardId = ANDROID_Ranking_ID;
#elif UNITY_IOS
        leaderBoardId = IOS_Ranking_ID;
#endif
        if (string.IsNullOrEmpty(leaderBoardId) == false)
        {
            Social.ReportScore(newScore, leaderBoardId, (bool success) =>
            {
            // handle success or failure
        });
        }
    }

    void OnGameOver()
    {
        if (Social.localUser.authenticated == true)
        {
            ReportScore(GameBoard.Score_MadeBlocks);
        }
    }
}
