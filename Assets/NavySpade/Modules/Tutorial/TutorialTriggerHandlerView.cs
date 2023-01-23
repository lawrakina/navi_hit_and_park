using System;
using System.Threading.Tasks;
using pj94.Code;
using pj94.Code.Cars;
using UnityEngine;


namespace NavySpade.Modules.Tutorial{
    [RequireComponent(typeof(Collider))] public class TutorialTriggerHandlerView : MonoBehaviour{
        [SerializeField]
        private TutorAction _tutorAction;
        private bool _isOn = false;
        private Collider _collider;
        [SerializeField]
        private float _timeDelay = 0.1f;

        private void Awake(){
            _isOn = false;
            _collider = GetComponent<Collider>();
        }

        private async void OnTriggerEnter(Collider other){
            if (other.TryGetComponent(out PlayerCar car)){
                if (car.TryGetComponent(out Car playerCar)){
                    if (playerCar.IsOn){
                        await Task.Delay((int) (_timeDelay * 100f));
                        TutorialController.InvokeAction(_tutorAction);
                    }
                }
            }
        }

        public void Enable(){
            _isOn = true;
            _collider.enabled = true;
        }
    }
}