using System;
using DG.Tweening;
using UnityEngine;


namespace NavySpade.Modules.Tutorial{
    public class TutorHandAnimation : MonoBehaviour{
        [SerializeField]
        private RectTransform Hand;

        private void Awake(){
            Hide();
        }

        private void Show(){
            Hand.gameObject.SetActive(true);
            Hand.anchoredPosition = Vector3.zero;
        }

        public void Hide(){
            Hand.DOKill();
            Hand.gameObject.SetActive(false);
        }

        public void AnimationTo(Vector2 direction){
            Hand.DOKill();
            Show();
            StartAnimation(direction);
        }

        private void StartAnimation(Vector2 direction){
            Hand.anchoredPosition = -direction * 100;
            Hand.DOAnchorPos(direction * 100, 1f).onComplete += () => { StartAnimation(direction); };
        }
    }
}