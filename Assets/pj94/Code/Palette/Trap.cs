using System;
using System.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using pj94.Code.Cars;
using pj94.Code.Extensions;
using UnityEngine;


namespace pj94.Code.Palette{
    public class Trap : MonoBehaviour{
        [field:SerializeField]
        private Transform StayPoint{ get; set; }
        [SerializeField]
        private Animator _animator;
        [SerializeField, ReadOnly]
        private bool _state = true;

        private int _animation = Animator.StringToHash($"Start");

        private async void OnTriggerEnter(Collider other){
            if (!_state) return;
            if (!other.TryGetComponent(out Car car)) return;
            _state = false;
            car.Off(true);
            car.transform.SetParent(StayPoint);
            _animator.SetTrigger(_animation);
            Vibro.Vibrate();
            car.transform.DOMove(StayPoint.position, 0.2f);
            var tween = car.transform.DOScale(new Vector3(0.9f, 0.01f, 0.9f), 0.35f);
            // tween.onComplete += () => { transform.position = new Vector3(
            //     transform.position.x,
            //     transform.position.y - 0.5f,
            //     transform.position.z
            //     );};
            await Task.Delay(200);
            car.StateProperty.Value = CarState.Trap;
        }
    }
}