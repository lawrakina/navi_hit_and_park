
using NavySpade.Core.Runtime.Game;
using NavySpade.Core.Runtime.Levels;
using UnityEngine;

namespace NavySpade.PJ70.Analytics
{
    public class HoopslySDKIntegration : AnalyticsProvider
    {
        protected override void OnResetLevel()
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.white)}> Hoopsly startLevel {CurrentLevel} </color>");
            HoopslyIntegration.RaiseLevelStartEvent(CurrentLevel.ToString());
            IsLevelStarted = true;
        }

        protected override void OnLevelFailed()
        {
            if(IsLevelStarted == false)
                return;
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}> Hoopsly endLevel lose {CurrentLevel} </color>");
            RaiseLevelEndEvent(LevelFinishedResult.lose);
        }

        protected override void OnLevelWin()
        {
            if(IsLevelStarted == false)
                return;
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}> Hoopsly endLevel win {CurrentLevel} </color>");
            RaiseLevelEndEvent(LevelFinishedResult.win);
        }

        private void RaiseLevelEndEvent(LevelFinishedResult result)
        {
            HoopslyIntegration.RaiseLevelFinishedEvent(CurrentLevel.ToString(), result);
            IsLevelStarted = false;
        }
    }
}
