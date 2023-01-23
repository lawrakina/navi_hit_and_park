using System;
using System.Threading.Tasks;
using Core.UI.Popups;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Tutorial;
using pj94.Code.Cars;
using pj94.Code.Extensions;
using UnityEngine;


namespace pj94.Code{
    [RequireComponent(typeof(PathGenerator))]
    public class RootCodeLevel : LevelBase{
        [SerializeField]
        private Car _user;
        private EventDisposal _disposel = new EventDisposal();

        private void Awake(){
            Instance = this;
        }

        public static RootCodeLevel Instance{ get; set; }

        public override async void Init(){
            EventManager.Invoke(GameStatesEM.LevelLoaded);
            EventManager.Add(GameStatesEM.StartGame, StartGame).AddTo(_disposel);
            EventManager.Add(GameStatesEM.OnWin, DisablingObjects).AddTo(_disposel);
            EventManager.Add(GameStatesEM.OnFail, DisablingObjects).AddTo(_disposel);
            EventManager.Add(GameStatesEM.Restart, DisablingObjects).AddTo(_disposel);

            Gui.Instance.LevelCounter = $"Level {LevelManager.ActualLevelIndex + 1}";

            //GameContext.Instance.LevelManager.UnlockNextLevel();
            //GameContext.Instance.LevelManager.RestartLevel();
            PrefabSettings.Instance = ResourceLoader.LoadConfig<PrefabSettings>();
            GameSettings.Instance = ResourceLoader.LoadConfig<GameSettings>();
            _user = FindObjectOfType<PlayerCar>().GetComponent<Car>();
            ChangeCarFromSettings();
        }

        public void ChangeCarFromSettings(){
            _user.SetSkin(PrefabSettings.Instance.SelectedSkinOfCar);
        }

        private void DisablingObjects(){
            Gui.Instance.ButtonRestart.SetActive(false);
            Gui.Instance.TutorHand.Hide();
        }

        private async void StartGame(){
            foreach (var tutorialTriggerHandlerView in FindObjectsOfType<TutorialTriggerHandlerView>()){
                tutorialTriggerHandlerView.Enable();
            }

            Gui.Instance.ButtonRestart.SetActive(true);


            _user.Init();
            GetComponent<ProgressLevelChecker>().Init(_user);
            await Task.Delay(100);
            GetComponent<PlayerInput>().Init(_user);
        }

        [Button("Win")]
        public void Win(){
            EventManager.Invoke(MainEnumEvent.Win);
        }

        [Button("Lose")]
        public void Fail(){
            EventManager.Invoke(MainEnumEvent.Fail);
        }

        public void ConsoleLog(string message){
            Debug.Log($"Message from tutor:{message}");
        }

        public void TutorHandHide(){
            Gui.Instance.TutorHand.Hide();
        }

        public void TutorHandToLeft(){
            Gui.Instance.TutorHand.AnimationTo(GameSettings.Instance.InverseControll ? Vector2.right : Vector2.left);
        }

        public void TutorHandToRight(){
            Gui.Instance.TutorHand.AnimationTo(GameSettings.Instance.InverseControll ? Vector2.left : Vector2.right);
        }

        public void TutorHandToUp(){
            Gui.Instance.TutorHand.AnimationTo(GameSettings.Instance.InverseControll ? Vector2.down : Vector2.up);
        }

        public void TutorHandToDown(){
            Gui.Instance.TutorHand.AnimationTo(GameSettings.Instance.InverseControll ? Vector2.up : Vector2.down);
        }

        private void OnDestroy(){
            _disposel?.Dispose();
        }
    }
}