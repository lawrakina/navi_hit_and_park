using NavySpade.pj88.Meta;
using UnityEngine;


namespace pj94.Code.Meta{
    [CreateAssetMenu(fileName = nameof(CarItemReward), menuName = "pj94/Rewards" + nameof(CarItemReward))]
    public class CarItemReward : ScriptableObject, IItemRewardData{
        [field: SerializeField]
        public GameObject Car{ get; set; }
        [field: SerializeField]
        public Sprite ChestLockedIcon{ get; set; }
        [field: SerializeField]
        public Sprite ChestUnlockIcon{ get; set; }
        [field: SerializeField]
        public string Name{ get; set; }
    }
}