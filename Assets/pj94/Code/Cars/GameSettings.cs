using UnityEngine;


namespace pj94.Code.Cars{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = "Settings/" + nameof(GameSettings))]
    internal class GameSettings : ScriptableObject{
        public static GameSettings Instance{ get; set; }
        [field: SerializeField]
        public int GemsForTrapOnLevel{ get; set; }
        [field: SerializeField]
        public float MaxSpeedMovingCar{ get; set; } = 10;

        [field:SerializeField]
        public bool InverseControll{ get; set; }
        [field: SerializeField]
        public float CarRotateSpeed{ get; set; } = 1f;
        [field: SerializeField]
        public int LevelAfterWhatShowingShopButton{ get; set; } = 4;
    }
}