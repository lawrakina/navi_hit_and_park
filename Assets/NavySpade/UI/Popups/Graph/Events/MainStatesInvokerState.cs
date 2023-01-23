using EventSystem.Runtime.Core.Managers;
using UnityEngine;


namespace Core.UI.Popups.Graph.Events{
    [CreateNodeMenu("Core/Events/Invoke/MainStates")]
    public class MainStatesInvokerState : EventInvokerState
    {
        [field: SerializeField]
        public MainEnumEvent GameState { get; private set; }

        public override void Run()
        {
            EventManager.Invoke(GameState);
        }
    }
}