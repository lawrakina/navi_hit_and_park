using DG.Tweening;
using pj94.Code.Cars;
using UnityEngine;


namespace pj94.Code.Palette{
    public class EndWayPoint : MonoBehaviour{
        private Car _carInPark;
        [field: SerializeField]
        private Transform StayPoint{ get; set; }

        private void OnTriggerEnter(Collider other){
            if (!other.TryGetComponent(out Car car)) return;
            if (_carInPark){
                _carInPark.transform.SetParent(null);
                _carInPark.RotateTo(car.transform.forward);
                _carInPark.Run(car.Impulce.Value, car.Speed);
                _carInPark = null;
            }

            car.Stop();
            car.transform.SetParent(StayPoint);
            car.transform.DOMove(StayPoint.position, 0.4f).OnComplete(() => { });
            if (car.transform != car.CurrentPoint) car.transform.DOLookAt(car.CurrentPoint.transform.position, 0.4f);
            _carInPark = car;
        }

        private void OnTriggerExit(Collider other){
            if (!other.TryGetComponent(out Car car)) return;
            if (car != _carInPark) return;
            _carInPark.transform.SetParent(null);
            _carInPark = null;
        }
    }
}