using System;
using UnityEngine;


namespace pj94.Code.Cars{
    public class ArrowView : MonoBehaviour{
        [SerializeField]
        private Renderer _renderer;
        private Gradient _gradient;
        private GradientColorKey[] _colorKey;
        private GradientAlphaKey[] _alphaKey;

        private void Awake(){
            _gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            _colorKey = new GradientColorKey[3];
            _colorKey[0].color = Color.green;
            _colorKey[0].time = 0.0f;
            _colorKey[1].color = Color.yellow;
            _colorKey[1].time = 0.5f;
            _colorKey[2].color = Color.red;
            _colorKey[2].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            _alphaKey = new GradientAlphaKey[2];
            _alphaKey[0].alpha = 1.0f;
            _alphaKey[0].time = 0.0f;
            _alphaKey[1].alpha = 0.0f;
            _alphaKey[1].time = 1.0f;

            _gradient.SetKeys(_colorKey, _alphaKey);
            
            Coloring(0);
        }

        public void Stretch(float value){
            transform.localScale = new Vector3(1, 1, value);
        }

        public void Coloring(float value){
            _renderer.material.color = _gradient.Evaluate(value);
        }
    }
}