using System;
using Core.UI.Popups.Graph.Conditions;
using NavySpade.Core.Runtime.Levels;
using UnityEngine;


namespace pj94.Code.Meta{
    [Serializable]
    [CustomSerializeReferenceName(nameof(CanContinueByLevel))]
    public class CanContinueByLevel : ICondition{
        [SerializeField]
        private int _level;
        public bool Check()
        {
            return LevelManager.LevelIndex >= _level;
        }
    }
}