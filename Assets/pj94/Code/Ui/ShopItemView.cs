using System;
using System.Collections;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Meta.Runtime.Economic.Prices.Interfaces;
using NavySpade.Meta.Runtime.Economic.Products;
using NavySpade.Meta.Runtime.Shop.Items;
using NavySpade.pj88.Meta;
using pj94.Code.Meta;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace pj94.Code.Ui{
    public class ShopItemView : MonoBehaviour{
        [SerializeField]
        private GameObject _lock;
        [SerializeField]
        private GameObject _skin;
        [SerializeField]
        private Image _fill;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private Button _button;

        public void Init(ShopItem shopItem, int index, UnityAction<ShopItem> clickToItem){
            var reward = shopItem.Product.Reward;
            var carItemReward = reward as ItemReward;
            
            var unlock = shopItem.UnlockableItem.IsUnlocked();
            _icon.sprite = carItemReward.Data.Value.ChestUnlockIcon;
            _button.onClick.AddListener(() => { clickToItem(shopItem); });

            if (unlock){
                _lock.SetActive(false);
                _skin.SetActive(false);
            } else{
                ItemReward itemReward = shopItem.Product.Reward as ItemReward;
                _icon.sprite = itemReward.Data.Value.ChestUnlockIcon;

                int reachedLvl = LevelManager.LevelIndex + 1;
                LevelUnlockCondition levelUnlockCondition = shopItem.UnlockConditions[0] as LevelUnlockCondition;

                int lastUnlockLvl = GetPreviousChestUnlockedLvl(index);
                float currentValue = Mathf.InverseLerp(lastUnlockLvl, levelUnlockCondition.CompletedLevel, reachedLvl);
                float previousValue = Mathf.InverseLerp(lastUnlockLvl, levelUnlockCondition.CompletedLevel, reachedLvl - 1);

                StartCoroutine(UpdateProgress(previousValue, currentValue));
            }
        }

        private void OnDestroy(){
            _button.onClick.RemoveAllListeners();
        }

        private IEnumerator UpdateProgress(float previousValue, float currentValue){
            _fill.fillAmount = previousValue;

            float progress = 0;
            while (progress < 1){
                progress += Time.deltaTime * 10;
                float newValue = Mathf.Lerp(previousValue, currentValue, progress);
                _icon.fillAmount = newValue;
                //_progressText.text = ((int)(newValue * 100)) + "%";
                yield return null;
            }
        }

        private int GetPreviousChestUnlockedLvl(int index){
            if (index == 0)
                return 1;

            ShopItem currentUnlockingItem = MetaGameConfig.Instance.Rewards[index - 1];
            LevelUnlockCondition levelUnlockCondition = currentUnlockingItem.UnlockConditions[0] as LevelUnlockCondition;
            return levelUnlockCondition.CompletedLevel;
        }
    }
}