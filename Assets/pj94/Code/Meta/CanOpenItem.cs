using System;
using Core.UI.Popups.Graph.Conditions;
using NavySpade.pj88.Meta;


namespace pj94.Code.Meta
{
    [Serializable]
    [CustomSerializeReferenceName("CanOpenItem")]
    public class CanOpenItem : ICondition
    {
        public bool Check()
        {
            return MetaGameConfig.Instance.CanUnlock;
        }
    }
}