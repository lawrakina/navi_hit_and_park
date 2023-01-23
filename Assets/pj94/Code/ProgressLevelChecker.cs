using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSystem.Runtime.Core.Managers;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Tutorial;
using NavySpade.pj88.Meta;
using pj94.Code.Cars;
using pj94.Code.Palette;
using UniRx;
using UnityEngine;


namespace pj94.Code{
    public class ProgressLevelChecker : MonoBehaviour{
        [SerializeField, ReadOnly]
        private Car[] _cars;
        protected CompositeDisposable _subscriptions = new CompositeDisposable();
        private bool _isOn;
        private Car _user;
        private bool _isTutor;
        private Trap[] _traps;
        public Car Player => _user;

        private void Awake(){
            Instance = this;
            _isOn = false;
            _cars = FindObjectsOfType<Car>();

            _traps = FindObjectsOfType<Trap>();
        }

        public static ProgressLevelChecker Instance{ get; set; }

        private void ChangeValue(CarState _){
            if (!_isOn) return;
            foreach (var car in _cars){
                if (_user.StateProperty.Value == CarState.Trap) EventManager.Invoke(MainEnumEvent.Fail);
                if (car == _user) continue;
                if (car.StateProperty.Value == CarState.Free) return;
            }

            // EventManager.Invoke(MainEnumEvent.NextLevel);
            EventManager.Invoke(MainEnumEvent.Fail);
        }

        public void Init(Car user){
            _user = user;
            _isOn = true;
            _user.StateProperty.Subscribe(CheckProgress).AddTo(_subscriptions);

            _isTutor = FindObjectOfType<TutorialController>() ? true : false;
        }

        private async void CheckProgress(CarState state){
            if (state != CarState.Trap) return;
            // if(_isTutor) return;

            await Task.Delay(2000);
            
            if (_cars.Any(car => car.StateProperty.Value == CarState.Free)){
                EventManager.Invoke(MainEnumEvent.Fail);
                return;
            }

            Reward = _traps.Length * GameSettings.Instance.GemsForTrapOnLevel;
            EventManager.Invoke(MainEnumEvent.Win);
        }

        public static int Reward{ get; set; }

        private void OnDisable(){
            _subscriptions.Dispose();
        }
    }
}