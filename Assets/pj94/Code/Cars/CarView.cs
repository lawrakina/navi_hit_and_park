using System;
using System.Collections.Generic;
using UnityEngine;


namespace pj94.Code.Cars{
    internal class CarView: MonoBehaviour{
        [SerializeField]
        private Transform _point;

        [SerializeField]
        private Transform _arrow;

        public void SetCar(GameObject skin){
            var children = new List<GameObject>();
            foreach (Transform child in _point) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
            var car = Instantiate(skin, _point);
            car.transform.localPosition = Vector3.zero;
            car.transform.localRotation= Quaternion.identity;
        }
    }
}