using UnityEngine;


namespace pj94.Code.Cars{
    [CreateAssetMenu(fileName = nameof(PrefabSettings), menuName = "Settings/" + nameof(PrefabSettings))]
    internal class PrefabSettings : ScriptableObject{
        private int _index = -1;
        public static PrefabSettings Instance{ get; set; }

        [field: SerializeField]
        public GameObject CarExplosionFire{ get; set; }
        [field: SerializeField]
        public ArrowView Arrow{ get; set; }
        
        [field: SerializeField]
        public Material Ui3DMat{ get; set; }

        public GameObject SelectedSkinOfCar{ get; set; }

        [field: SerializeField]
        public GameObject EffectConfetti{ get; set; }
        [field: SerializeField]
        public GameObject EffectChangeCar{ get; set; }


        public void SetPlayerCar(GameObject car){
            SelectedSkinOfCar = car;
        }

        [field: SerializeField]
        public Sprite[] HappyEmoji{ get; set; }
        [field: SerializeField]
        public Sprite[] BadEmoji{ get; set; }

        public Sprite GetHappyEmoji(){
            return HappyEmoji[Random.Range(0, HappyEmoji.Length - 1)];
        }

        public Sprite GetBadEmoji(){
            return BadEmoji[Random.Range(0, BadEmoji.Length - 1)];
        }

        [field: SerializeField]
        public string[] HappyMessage{ get; set; }
        [field: SerializeField]
        public string[] BadMessage{ get; set; }
        [field: SerializeField]
        public GameObject Boom{ get; set; }

        public string GetHappyMessage(){
            _index++;
            if (_index > HappyMessage.Length - 1)
                _index = 0;
            return HappyMessage[_index];
        }

        public string GetBadMessage(){
            _index++;
            if (_index > BadMessage.Length - 1)
                _index = 0;
            return BadMessage[_index];
        }
    }
}