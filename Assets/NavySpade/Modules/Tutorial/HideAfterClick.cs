using UnityEngine;
using UnityEngine.EventSystems;


namespace NavySpade.Modules.Tutorial{
    public class HideAfterClick : MonoBehaviour, IPointerClickHandler{
        public void OnPointerClick(PointerEventData eventData){
            gameObject.SetActive(false);
        }
    }
}