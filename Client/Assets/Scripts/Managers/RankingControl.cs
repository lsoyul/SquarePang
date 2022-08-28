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
    private const string IOS_Ranking_ID_sprint_short_test = "score_short_sprint_test";
    private const string IOS_Ranking_ID_sprint_classic_test = "score_classic_sprint_test";
    private const string IOS_Ranking_ID_endless_short_test = "score_short_endless_test";
    private const string IOS_Ranking_ID_endless_classic_test = "score_classic_endless_test";

    private const string ANDROID_Ranking_Endless_ID = GPGSIds.leaderboard_test_endless_score;
    private const string ANDROID_Ranking_Sprint_ID = GPGSIds.leaderboard_test_sprint_score;

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

    public static void ReportScore(GameStatics.GameMode gameMode, int newScore)
    {
        string leaderBoardId = string.Empty;
        string leaderBoardId_IOS_Temp = string.Empty;

#if UNITY_ANDROID
        if (gameMode == GameStatics.GameMode.Endless)
            leaderBoardId = ANDROID_Ranking_Endless_ID;
        else if (gameMode == GameStatics.GameMode.Sprint)
            leaderBoardId = ANDROID_Ranking_Sprint_ID;


#elif UNITY_IOS

        if (gameMode == GameStatics.GameMode.Sprint)
        {
            leaderBoardId = IOS_Ranking_ID_sprint_short_test;
            leaderBoardId_IOS_Temp = IOS_Ranking_ID_sprint_classic_test;
        }
        else if (gameMode == GameStatics.GameMode.Endless)
        {
            leaderBoardId = IOS_Ranking_ID_endless_short_test;
            leaderBoardId_IOS_Temp = IOS_Ranking_ID_endless_classic_test;
        }
#endif
        if (string.IsNullOrEmpty(leaderBoardId) == false)
        {
            Social.ReportScore(newScore, leaderBoardId, (bool success) =>
            {
                // handle success or failure
            });
        }

        if (string.IsNullOrEmpty(leaderBoardId_IOS_Temp) == false)
        {
            Social.ReportScore(newScore, leaderBoardId_IOS_Temp, (bool success) =>
            {
                // handle success or failure
            });
        }
    }

    void OnGameOver(GameStatics.GameEndType gameEndType)
    {
        if (Social.localUser.authenticated == true)
        {
            ReportScore(GameBoard.CurGameMode, GameBoard.Score_MadeBlocks);
        }
    }
}
