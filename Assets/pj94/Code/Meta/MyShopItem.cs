using NavySpade.pj88.Meta;
using UnityEngine;


namespace NavySpade.pj88{
    [CreateAssetMenu(fileName = nameof(MyShopItem), menuName = "Settings/Items" + nameof(MyShopItem))]
    public class MyShopItem : ScriptableObject, IItemRewardData{
        [field: SerializeField]
        public Sprite ChestLockedIcon{ get; set; }
        [field: SerializeField]
        public Sprite ChestUnlockIcon{ get; set;}
        [field: SerializeField]
        public string Name{ get; set;}
    }
}