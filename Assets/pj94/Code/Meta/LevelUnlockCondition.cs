using System;
using Core.Meta.Unlocks;
using NavySpade.Core.Runtime.Levels;
using UnityEngine;


namespace NavySpade.pj88.Meta
{
    [Serializable]
    [CustomSerializeReferenceName("LevelUnlock")]
    public class LevelUnlockCondition : IUnlockCondition
    {
        [field: SerializeField] public int CountFrom { get; private set; }
        [field: SerializeField] public int CompletedLevel { get; private set; }
        
        public bool IsMatch()
        {
            return LevelManager.LevelIndex >= CompletedLevel - 1;
        }
    }
}