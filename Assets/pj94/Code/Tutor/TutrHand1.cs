using UnityEngine;


namespace pj94.Code.Tutor{
    public class TutrHand1 : MonoBehaviour{
        [SerializeField]
        private GameObject _hand;

        private void Awake(){
            Instance = this;
            Hide();
        }

        public static TutrHand1 Instance{ get; set; }

        public void Show(){
            _hand.SetActive(true);
        }

        public void Hide(){
            _hand.SetActive(false);
        }
    }
}