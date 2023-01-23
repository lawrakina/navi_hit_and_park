using UnityEngine;


namespace NavySpade.pj88.Meta
{
    public interface IItemRewardData
    {
        public Sprite ChestLockedIcon { get; }
        public Sprite ChestUnlockIcon { get; }
        public string Name { get; }
    }
}