using System.Collections;
using Core.UI.Popups.Abstract;
using EventSystem.Runtime.Core.Managers;
using NavySpade.UI.Popups.Abstract;
using pj94.Code.Cars;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Core.UI.Popups{
    public class LosePopup : Popup{
        [SerializeField]
        private bool _needExitTime = false;
        [SerializeField]
        private float _exitTime = 2f;

        private Coroutine exitCoroutine = null;
        [SerializeField]
        private Image _emoji;
        [SerializeField]
        private TMP_Text _emojiText;

        public override void OnAwake(){
            SetEmoji();
        }

        private void SetEmoji(){
            _emoji.sprite = PrefabSettings.Instance.GetBadEmoji();
            _emojiText.text = PrefabSettings.Instance.GetBadMessage();
        }

        public override void OnStart(){
            if (_needExitTime)
                exitCoroutine = StartCoroutine(WaitForExit());
        }

        public void RestartLevel(){
            if (exitCoroutine != null)
                StopCoroutine(exitCoroutine);

            exitCoroutine = null;
            //SoundPlayer.PlaySoundFx("Click");
            EventManager.Invoke(GameStatesEM.Restart);
            Close();
        }

        private IEnumerator WaitForExit(){
            yield return new WaitForSecondsRealtime(_exitTime);
            RestartLevel();
        }
    }
}