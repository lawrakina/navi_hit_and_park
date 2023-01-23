using System;
using NavySpade.Core.Runtime.Game;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Meta.Runtime.Shop.Items;
using NavySpade.Modules.Configuration.Runtime.SO;
using NavySpade.Modules.Saving.Runtime;
using UnityEngine;


namespace NavySpade.pj88.Meta{
    [CreateAssetMenu(fileName = "MetaGameConfig", menuName = "Meta/Config")]
    public class MetaGameConfig : ObjectConfig<MetaGameConfig>{
        private const string PREFS_LAST_EARN_COUNT_KEY = "Meta.Chest.LastEarned";

        [field: SerializeField]
        public bool EnableMetaGame{ get; private set; } = true;

        [SerializeField]
        private bool _enableQuests = true;
        [SerializeField]
        private bool _enableStickyFactor = true;
        [SerializeField]
        private bool _enableSkinsShop = true;

        [SerializeField]
        private ShopItem[] _rewards;

        private bool _progressUpdated;

        [field: Min(0), SerializeField]
        public uint DelayBetweenNextRewardInSeconds{ get; private set; } = 60 * 60 * 24;

        public bool EnableQuests => EnableMetaGame && _enableQuests;

        public bool EnableStickyFactor => EnableMetaGame && _enableStickyFactor;

        public bool EnableSkinsShop => EnableMetaGame && _enableSkinsShop;

        public ShopItem[] Rewards => _rewards;

        public bool CanUnlock{
            get{
                if (EnableMetaGame == false)
                    return false;

                return _progressUpdated;
            }
        }

        public void UnlockItem(){
            var shopItems = Rewards;
            int lastEarnedItem = SaveManager.Load(PREFS_LAST_EARN_COUNT_KEY, 1);
            _progressUpdated = false;

            if (lastEarnedItem >= shopItems.Length)
                return;

            ShopItem currentUnlockingItem = shopItems[lastEarnedItem];

            int reachedLvl = LevelManager.LevelIndex + 1;
            
            LevelUnlockCondition levelUnlockCondition =
                currentUnlockingItem.UnlockConditions[0] as LevelUnlockCondition;
            if (levelUnlockCondition.CompletedLevel < reachedLvl){
                currentUnlockingItem.ForceUnlock();
                lastEarnedItem++;
                SaveManager.Save(PREFS_LAST_EARN_COUNT_KEY, lastEarnedItem);
            }

            if(levelUnlockCondition.CountFrom + 1 < reachedLvl)
                _progressUpdated = true;
        }

        // public void ClearUpgradeProgress()
        // {
        //     Array.ForEach(_upgrades, (r) => r.ClearProgress());
        // }
    }
}