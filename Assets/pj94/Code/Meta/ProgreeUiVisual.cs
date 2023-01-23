using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace NavySpade.pj88.Meta{
    public class ProgreeUiVisual : MonoBehaviour{
        [SerializeField]
        private LvlElement[] _levelElements;
        [SerializeField]
        private Image _currentGround;
        [SerializeField]
        private Image _nextGround;
        [SerializeField]
        private TMP_Text _level;
        [field: SerializeField]
        public GameObject Root{ get; set; }
        public static ProgreeUiVisual Instance{ get; set; }
        public int Level{
            set => _level.text = $"LEVEL {value+1}";
        }

        public void SetProgress(int progress){
            for (int i = 0; i < 5; i++){
                _levelElements[i].Done.SetActive(i < progress);
                _levelElements[i].Current.SetActive(i == progress);
            }
        }

        public void SetCurrentTexture(Sprite sprite){
            _currentGround.sprite = sprite;
        }

        public void SetNextTexture(Sprite sprite){
            _nextGround.sprite = sprite;
        }
    }
}