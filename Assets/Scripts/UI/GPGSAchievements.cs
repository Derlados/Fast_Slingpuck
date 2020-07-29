using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GPGSAchievements : MonoBehaviour
{
    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public static void updateIncrementalScore()
    {
        int score = PlayerData.getInstance().incScore;
        if(score < 1) PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_adventure_has_started, 1, null);
        if(score < 15) PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_youre_almost_expert, 1, null);
        if(score < 50) PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_just_a_master, 1, null);
        if(score < 100) PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_do_cyborgs_play_games, 1, null);

        PlayerData.getInstance().incScore++;
        PlayerData.getInstance().Save();
    }

  //  PlayGamesPlatform.Instance.IncrementAchievement(id, 1, null);
  // Social.ReportProgress(id, 100f, null);

}
