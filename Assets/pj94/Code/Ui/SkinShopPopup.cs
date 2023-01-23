using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Meta.Runtime.Economic.Currencies;
using NavySpade.Meta.Runtime.Economic.Prices.DifferentTypes;
using NavySpade.Meta.Runtime.Shop.Items;
using NavySpade.Modules.Vibration.Runtime;
using NavySpade.pj88.Meta;
using NavySpade.UI.Popups.Abstract;
using pj94.Code.Cars;
using pj94.Code.Extensions;
using pj94.Code.Meta;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace pj94.Code.Ui{
    public class SkinShopPopup : Popup{
        [SerializeField]
        private ShopItemView _template;
        [SerializeField]
        private Transform _content;
        [SerializeField]
        private Image _itemImage;
        [SerializeField]
        private Sprite _lockImage;
        [SerializeField]
        private Transform _itemCurrencyObject;
        [SerializeField]
        private TMP_Text _currencyText;
        // [SerializeField]
        // private Button _useButton;
        [SerializeField]
        private Button _buyButton;
        [SerializeField]
        private Image _carImage;


        #region PrivateData

        private ShopItem _selectItem;
        private ItemReward _carItemReward;
        private CurrencyPrice _cashPrice;

        #endregion


        public override void OnAwake(){
        }

        public override void OnStart(){
            DefaultState();
            UpdateList();

            InitBuyButton();
            HideBuy();
            // InitUseButton();
        }

        // private void InitUseButton(){
        //     _useButton.onClick.AddListener(() => {
        //         if (_selectItem != null){
        //             _itemCurrencyObject.gameObject.SetActive(false);
        //             UpdateCar(_carItemReward);
        //             _useButton.gameObject.SetActive(false);
        //             var effect = ResourceLoader.InstantiateObject(PrefabSettings.Instance.EffectChangeCar, Ui3D.Instance.EffectPoint, false);
        //             Destroy(effect, 2);
        //         }
        //     });
        // }

        private void UpdateList(){
            ClearItems();
            var rewards = MetaGameConfig.Instance.Rewards;
            for (var index = 0; index < rewards.Length; index++){
                var reward = rewards[index];
                var cell = Instantiate(_template, _content);
                cell.Init(reward, index, ClickToCell);
            }
        }

        private void DefaultState(){
            _carImage.gameObject.SetActive(true);
            SetCarToDefault();
            _itemCurrencyObject.gameObject.SetActive(false);
        }

        private void ClickToCell(ShopItem shopItem){
            Vibro.Vibrate();
            var reward = shopItem.Product.Reward;
            _carItemReward = reward as ItemReward;

            var unlock = shopItem.UnlockableItem.IsUnlocked();
            var purchased = shopItem.UnlockableItem.IsUnlockFromStart || shopItem.UnlockableItem.IsEarnedReward();

            UpdateVisualCar((_carItemReward.Data.Value as CarItemReward).Car);
            if (unlock){
                _selectItem = shopItem;
                if (purchased){
                    InstantlyUse();
                } else{
                    // _selectItem = shopItem;
                    SetCurrencyCount(shopItem.Product.Price as CurrencyPrice);
                    ShowBuy();
                }
            } else{
                HideBuy();
            }
        }

        private void HideBuy(){
            _buyButton.gameObject.SetActive(false);
        }

        private void InstantlyUse(){
            _itemCurrencyObject.gameObject.SetActive(false);
            UpdateCar(_carItemReward);
            var effect = ResourceLoader.InstantiateObject(PrefabSettings.Instance.EffectChangeCar,
                Ui3D.Instance.EffectPoint, false);
            Destroy(effect, 2);
        }

        private void ShowUse(){
            _buyButton.gameObject.SetActive(false);
            _itemCurrencyObject.gameObject.SetActive(false);
        }

        private void InitBuyButton(){
            _buyButton.onClick.AddListener(() => {
                if (_selectItem != null){
                    if (_cashPrice.Count > CurrencyConfig.Instance.UsedInGame[0].Count) return;
                    CurrencyConfig.Instance.UsedInGame[0].Count -= _cashPrice.Count;
                    _itemCurrencyObject.gameObject.SetActive(false);
                    _selectItem.ForceEarnReward();
                    var effect = ResourceLoader.InstantiateObject(PrefabSettings.Instance.EffectConfetti,
                        Ui3D.Instance.transform, false);
                    UpdateCar(_carItemReward);
                    UpdateList();
                    ShowUse();
                    Destroy(effect, 2);
                }
            });
        }

        private void ClearItems(){
            var children = new List<GameObject>();
            foreach (Transform child in _content) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        private void SetCurrencyCount(CurrencyPrice price){
            if (price.Count > 0){
                _cashPrice = price;
                _itemCurrencyObject.gameObject.SetActive(true);
                _currencyText.text = price.Count.ToString();
            } else{
                _itemCurrencyObject.gameObject.SetActive(false);
            }
        }

        private void SetCarToDefault(){
            var car = ((MetaGameConfig.Instance.Rewards[0].Product.Reward as ItemReward).Data.Value as CarItemReward)
                .Car;
            PrefabSettings.Instance.SetPlayerCar(car);

            RootCodeLevel.Instance.ChangeCarFromSettings();
        }

        private void ShowBuy(){
            // _useButton.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(true);
            _itemCurrencyObject.gameObject.SetActive(true);
            if (_cashPrice == null)
                _buyButton.gameObject.SetActive(false);
            else
                _buyButton.gameObject.SetActive(_cashPrice.Count <= CurrencyConfig.Instance.UsedInGame[0].Count);
        }

        private void UpdateVisualCar(GameObject prefab){
            _itemImage.sprite = null;
            _itemImage.material = PrefabSettings.Instance.Ui3DMat;

            Ui3D.Instance.Car = prefab;
        }

        private void UpdateCar(ItemReward carItemReward){
            _carImage.gameObject.SetActive(true);
            var itemReward = (carItemReward.Data.Value as CarItemReward);

            PrefabSettings.Instance.SetPlayerCar(itemReward.Car);
            RootCodeLevel.Instance.ChangeCarFromSettings();
        }
    }
}