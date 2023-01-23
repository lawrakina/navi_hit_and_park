using System;
using System.Threading.Tasks;
using NaughtyAttributes;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Tutorial;
using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using UnityEngine;
using UnityEngine.Events;


namespace pj94.Code.Tutor{
    public class TutorialShopController : MonoSingleton<TutorialShopController>{
        [Serializable] public struct TutorStep{
            public TutorShopAction TutorAction;
            public UnityEvent OnStartAction;
            public UnityEvent OnEndAction;
        }

        [SerializeField]
        private TutorArrow _tutorArrow;
        [SerializeField]
        private TutorStep[] _tutorSteps;
        
        [ReadOnly,SerializeField]
        private int _currentStepIndex;
        private bool _tutorDone;

        public bool TutorDone{
            get => _tutorDone;
            set{
                _tutorDone = value;
                SaveManager.Save("TutorShopDone", _tutorDone ? 1 : 0);
            }
        }

        public TutorShopAction CurrentTutorAction => _tutorSteps[_currentStepIndex].TutorAction;

        public static void InvokeAction(TutorShopAction action){
            if (InstanceExists == false)
                return;

            if (Instance.TutorDone)
                return;

            Instance.CheckProgress(action);
        }

        private void Start(){
            _tutorDone = SaveManager.Load("TutorShopDone", 0) != 0;
            if (TutorDone == false){
                TutorStep currentStep = _tutorSteps[_currentStepIndex];
                currentStep.OnStartAction?.Invoke();
            }
        }

        private void CheckProgress(TutorShopAction action){
            TutorStep currentStep = _tutorSteps[_currentStepIndex];
            if (currentStep.TutorAction == action){
                UpdateTutorProgress();
            }
        }

        private void UpdateTutorProgress(){
            TutorStep currentStep = _tutorSteps[_currentStepIndex];
            currentStep.OnEndAction?.Invoke();

            _currentStepIndex++;
            if (_currentStepIndex >= _tutorSteps.Length){
                TutorDone = true;
                return;
            }

            TutorStep nextStep = _tutorSteps[_currentStepIndex];
            nextStep.OnStartAction?.Invoke();
        }

        public void SetArrowTarget(Transform target){
            _tutorArrow.Target = target;
        }

        public void ShowTutorClick(){
            TutorClickHandler.Show();
        }

        public void HideTutorClick(){
            TutorClickHandler.Hide();
        }

        public static void ForcedAction(TutorShopAction action){
            foreach (var step in Instance._tutorSteps){
                if (step.TutorAction == action){
                    step.OnStartAction?.Invoke();
                    Instance._tutorDone = true;
                }
            }
        }


        public void Tutr1HandHide(){
            TutrHand1.Instance.Hide();
        }

        public async void Tutr2HandShow(){
            await Task.Delay(200);
            TutrHand2.Instance.Show();
        }
        public void Tutr2HandHide(){
            TutrHand2.Instance.Hide();
        }
        public void Tutr3HandShow(){
            TutrHand3.Instance.Show();
        }
        public void Tutr3HandHide(){
            TutrHand3.Instance.Hide();
        }
    }


    public enum TutorShopAction{
        GotoShop,
        SelectCar,
        BuyCar
    }
}