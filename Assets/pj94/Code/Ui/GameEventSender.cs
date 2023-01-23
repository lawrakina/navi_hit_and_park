using EventSystem.Runtime.Core.Managers;
using NaughtyAttributes;
using UnityEngine;


namespace pj94.Code.Ui{
    public class GameEventSender : MonoBehaviour{
        [Button("Win")]
        public void Win(){
            EventManager.Invoke(MainEnumEvent.Win);
        }

        [Button("Lose")]
        public void Fail(){
            EventManager.Invoke(MainEnumEvent.Fail);
        }

        [Button("Restart")]
        public void Restart(){
            EventManager.Invoke(GameStatesEM.Restart);
        }
    }
}