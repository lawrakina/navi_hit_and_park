using NavySpade.Core.Runtime.Levels;
using UnityEngine;


namespace pj94.Code.Tutor{
    public class TutrShopAwakeHandler : MonoBehaviour{
        [SerializeField]
        private TutorShopAction _tutorAction;

        private async void Awake(){
            // await Task.Delay(100);
            if(LevelManager.ActualLevelIndex == 10)
                TutorialShopController.InvokeAction(_tutorAction);
        }
    }
}