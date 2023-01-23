using System.Collections.Generic;
using pj94.Code.Extensions;
using UnityEngine;


namespace pj94.Code.Ui{
    internal class Ui3D: MonoBehaviour{
        private GameObject _car;
        [field:SerializeField]
        public Transform EffectPoint{ get; set; }
        public static Ui3D Instance{ get; set; }
        public GameObject Car{
            set{
                var rotate = Quaternion.identity;
                if (_car) rotate = _car.transform.rotation;
                var children = new List<GameObject>();
                foreach (Transform child in transform) children.Add(child.gameObject);
                children.ForEach(child => Destroy(child));
                _car = ResourceLoader.InstantiateObject(value, transform, false);
                _car.transform.localPosition = Vector3.zero;
                _car.transform.rotation = rotate;
            }
        }

        private void Awake(){
            Instance = this;
        }

        private void Update(){
            if(_car) 
                _car.transform.Rotate(Vector3.up, Time.deltaTime * 10);
        }
    }
}