using UnityEngine;


namespace NavySpade.Modules.Tutorial{
    [RequireComponent(typeof(Animation))]
    public class PlayAnimation : MonoBehaviour
    {
        private void Awake(){
            GetComponent<Animation>().Play();
        }
    }
}
