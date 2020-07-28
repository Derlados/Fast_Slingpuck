using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPGSAchievements : MonoBehaviour
{
    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void UpdateIncremental()
    {
        //увеличение ачивки на 1
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_goals, 1, null);
    }

  
    public void UnLockRegular()
    {
        //100f - полный прогресс
        Social.ReportProgress(GPGSIds.achievement_lava_boss, 100f, null);
    }
}
