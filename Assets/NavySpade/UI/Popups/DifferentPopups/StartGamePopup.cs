using Core.UI.Popups.Abstract;
using NavySpade.Core.Runtime.Levels;
using NavySpade.UI.Popups.Abstract;
using pj94.Code.Cars;
using UnityEngine;
using UnityEngine.UI;


namespace NavySpade.UI.Popups.DifferentPopups
{
    public class StartGamePopup : PopupWithCondition{
        public static StartGamePopup Instance { get; private set; }
        
        [field: SerializeField]
        public Button GotoShopButton{ get; set; }

        public override void OnAwake()
        {
            Instance = this;
        }

        public override void OnStart()
        {
            if(LevelManager.LevelIndex == 0)
                Close();
            
            GotoShopButton.gameObject.SetActive(LevelManager.LevelIndex >= GameSettings.Instance.LevelAfterWhatShowingShopButton);
        }

        public override bool IsOpen{ get; }
    }
}