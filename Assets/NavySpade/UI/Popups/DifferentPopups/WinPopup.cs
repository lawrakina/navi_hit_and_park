using System.Collections;
using System.Threading.Tasks;
using Core.Meta.Economic.Rewards;
using Core.UI.Counters;
using Core.UI.Popups.Abstract;
using EventSystem.Runtime.Core.Managers;
using Main.Levels.Configuration;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Meta.Runtime.Economic.Currencies;
using NavySpade.Meta.Runtime.Economic.Rewards.DifferentTypes;
using NavySpade.Modules.Vibration.Runtime;
using NavySpade.UI.Popups.Abstract;
using pj94.Code;
using pj94.Code.Cars;
using pj94.Code.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Core.UI.Popups{
    public class WinPopup : Popup{
        [SerializeField]
        private bool _needExitTime = false;
        [SerializeField]
        private float _exitTime = 2f;
        [SerializeField]
        private TextMeshProUGUI _cointsCount;

        [Header("Currency")]
        [SerializeField]
        private Currency _currency;
        [SerializeField]
        private DigitalCounter _moneyAttractor;
        [SerializeField]
        private ParticleSystem _receiveMoneyEffect;

        private int _receivedMoney;
        private int _beggingCounterValue;

        private bool _continue;
        private bool _closeInvoked;
        private bool _effectInProgress;
        private CurrencyReward _reward;

        private Coroutine _exitCoroutine = null;
        private int _earnCount;
        private int _moneyLeft;

        [SerializeField]
        private Transform _levelReward;
        [SerializeField]
        private Transform _globalCurrency;

        [SerializeField]
        private Button _buttonContinue;
        [SerializeField]
        private Image _emoji;
        [SerializeField]
        private TMP_Text _emojiText;

        public void Initialize(CurrencyReward reward){
            // reward.Count
            // if (LevelManager.IsTutorialLevel){
            //     _cointsCount.gameObject.SetActive(false);
            //     _globalCurrency.gameObject.SetActive(false);
            //     return;
            // }
            _reward = reward;
            if (_cointsCount != null) _cointsCount.text = _reward.Count.ToString();

            if (_needExitTime)
                _exitCoroutine = StartCoroutine(WaitForExit());
        }

        public async void StartReceivingMoney(){
            if (_effectInProgress){
                // _closeInvoked = true;
                // NextLevel();
                return;
            }

            _receiveMoneyEffect.Play();
            _effectInProgress = true;
            _moneyLeft = _earnCount;

            _buttonContinue.interactable = false;
            await Task.Delay(1500);
            Close();
        }

        public async void ReceiveMoney(){
            int numbers = Mathf.Abs(_earnCount).ToString().Length;
            _receivedMoney += numbers == 1 ? 1 : 10 * (numbers - 1);
            // VibrationManager.Vibrate();
            Vibro.Vibrate();

            int currentValue = Mathf.Clamp(
                _beggingCounterValue + _receivedMoney,
                _beggingCounterValue,
                _currency.Count);

            _moneyAttractor.UpdateValue(currentValue);

            _moneyLeft = Mathf.Clamp(_earnCount - _receivedMoney, 0, _earnCount);
            _cointsCount.text = _moneyLeft.ToString();

            if (_moneyLeft <= 0 && _closeInvoked == false){
                _closeInvoked = true;
                _buttonContinue.interactable = false;
                await Task.Delay(2000);
                Close();
                // Invoke(nameof(NextLevel), _exitTime);
            }
        }

        public void RestartLevel(){
            //SoundPlayer.PlaySoundFx("Click");
            //  _reward.Complete(); todo reward
            EventManager.Invoke(GameStatesEM.Restart);
            Close();
        }

        public void NextLevel(){
            if (_exitCoroutine != null)
                StopCoroutine(_exitCoroutine);

            _exitCoroutine = null;
            GlobalParameters.DoubleLevelNumber++;
            //SoundPlayer.PlaySoundFx("Click");
            // _reward.Complete(); todo reward

            Close();

            EventManager.Invoke(GameStatesEM.NextLevel);
        }

        public override void OnAwake(){
            // if (LevelManager.LevelIndex <= LevelsConfig.Instance.Tutorial.Length){
            //     _levelReward.gameObject.SetActive(false);
            //     _globalCurrency.gameObject.SetActive(false);
            //     return;
            // }

            _beggingCounterValue = _currency.Count;
            _earnCount = ProgressLevelChecker.Reward;
            _currency.Count += _earnCount;
            _cointsCount.text = _earnCount.ToString();
            _moneyAttractor.UpdateValueInstantly(_beggingCounterValue);

            SetEmoji();
        }

        private void SetEmoji(){
            _emoji.sprite = PrefabSettings.Instance.GetHappyEmoji();
            _emojiText.text = PrefabSettings.Instance.GetHappyMessage();
        }

        public override void OnStart(){
            // StartReceivingMoney();
        }

        private IEnumerator WaitForExit(){
            yield return new WaitForSecondsRealtime(_exitTime);
            NextLevel();
        }
    }
}