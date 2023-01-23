using System;
using System.Collections;
using System.Threading.Tasks;
using NavySpade.Modules.Vibration.Runtime;
using pj94.Code.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;


namespace pj94.Code.Cars{
    public class Bumper : MonoBehaviour
    {
        private Car _car;

        public Car Car => _car;

        public void Init(Car car){
            _car = car;
        }

        private void OnTriggerEnter(Collider other){
            if (!other.TryGetComponent(out Bumper bumper)) return;

            if (!(Car.Impulce.Value > 0.1f) || Car.IsStopped) return;
            bumper.Car.RotateTo(_car.transform.forward);
            var impulce = Car.Impulce.Value;
            Car.SlowlyStopped(bumper.Car.transform.position);

            bumper.Car.Run(impulce, Car.Speed);

            var position = (Car.transform.position + bumper.Car.transform.position) / 2;
            var effect = Instantiate(PrefabSettings.Instance.CarExplosionFire, position, Quaternion.identity);
            Destroy(effect, 1.5f);
            Vibro.Vibrate();
        }
    }
}