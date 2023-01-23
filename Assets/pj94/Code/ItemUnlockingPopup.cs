using System.Collections;
using EventSystem.Runtime.Core.Managers;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Meta.Runtime.Shop.Items;
using NavySpade.pj88.Meta;
using NavySpade.UI.Popups.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace NavySpade.pj88.Galery{
    public class ItemUnlockingPopup : Popup{
        private const string PREFS_LAST_EARN_COUNT_KEY = "Meta.Chest.LastEarned";

        [SerializeField]
        protected Image _unlockIcon;
        [SerializeField]
        protected TextMeshProUGUI _itemName;

        [Header("HasProgress")]
        [SerializeField]
        private bool _hasProgress;
        [SerializeField]
        [ShowIf(nameof(_hasProgress))]
        protected Image _lockedIcon;
        [SerializeField]
        [ShowIf(nameof(_hasProgress))]
        protected TextMeshProUGUI _progressText;
        [SerializeField]
        [ShowIf(nameof(_hasProgress))]
        private float _fill_Speed = 1f;

        public override void OnAwake(){
        }

        public override void OnStart(){
            Initialize();
        }

        private void Initialize(){
            var shopItems = MetaGameConfig.Instance.Rewards;
            int reachedLvl = LevelManager.ActualLevelIndex + 1;
            int completedLvl = reachedLvl - 1;

            for (int i = 0; i < shopItems.Length; i++){
                ShopItem shopItem = shopItems[i];
                LevelUnlockCondition levelUnlockCondition = shopItem.UnlockConditions[0] as LevelUnlockCondition;
                if (levelUnlockCondition.CompletedLevel >= completedLvl){
                    ItemReward itemReward = shopItem.Product.Reward as ItemReward;
                    _unlockIcon.sprite = itemReward.Data.Value.ChestUnlockIcon;
                    _itemName.text = itemReward.Data.Value.Name;

                    if (_hasProgress){
                        _lockedIcon.sprite = itemReward.Data.Value.ChestLockedIcon;

                        int lastUnlockLvl = levelUnlockCondition.CountFrom;
                        // int lastUnlockLvl = GetPreviousChestUnlockedLvl(i);
                        float currentValue = Mathf.InverseLerp(lastUnlockLvl, levelUnlockCondition.CompletedLevel,
                            completedLvl);
                        float previousValue =
                            Mathf.InverseLerp(lastUnlockLvl, levelUnlockCondition.CompletedLevel, completedLvl - 1);

                        StartCoroutine(UpdateProgress(previousValue, currentValue));
                    }

                    return;
                }
            }
        }

        private IEnumerator UpdateProgress(float previousValue, float currentValue){
            _unlockIcon.fillAmount = previousValue;

            float progress = 0;
            while (progress < 1){
                progress += Time.deltaTime * _fill_Speed;
                float newValue = Mathf.Lerp(previousValue, currentValue, progress);
                _unlockIcon.fillAmount = newValue;
                _progressText.text = ((int) (newValue * 100)) + "%";
                yield return null;
            }
        }

        private int GetPreviousChestUnlockedLvl(int index){
            if (index == 0)
                return 1;

            ShopItem currentUnlockingItem = MetaGameConfig.Instance.Rewards[index - 1];
            LevelUnlockCondition levelUnlockCondition =
                currentUnlockingItem.UnlockConditions[0] as LevelUnlockCondition;
            return levelUnlockCondition.CompletedLevel;
        }

        public void NextLevel(){
            GlobalParameters.DoubleLevelNumber++;
            Close();

            EventManager.Invoke(GameStatesEM.NextLevel);
        }
    }
}