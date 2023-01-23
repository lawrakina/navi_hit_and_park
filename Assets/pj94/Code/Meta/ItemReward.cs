using System;
using AYellowpaper;
using NavySpade.Meta.Runtime.Economic.Rewards.Interfaces;
using UnityEngine;


namespace NavySpade.pj88.Meta
{
    
    [Serializable]
    [CustomSerializeReferenceName("Item")]
    public class ItemReward : IReward
    {
        [field: SerializeField] public InterfaceReference<IItemRewardData> Data { get; private set; }
        
        public void TakeReward() { }
    }
}