using System;
using NaughtyAttributes;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using UnityEngine;
using UnityEngine.Events;


namespace NavySpade.Modules.Tutorial{
    public class TutorialController : MonoSingleton<TutorialController>{
        [Serializable] public struct TutorStep{
            public TutorAction TutorAction;
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
                SaveManager.Save("TutorDone", _tutorDone ? 1 : 0);
            }
        }

        public TutorAction CurrentTutorAction => _tutorSteps[_currentStepIndex].TutorAction;

        public static void InvokeAction(TutorAction action){
            if (InstanceExists == false)
                return;

            if (Instance.TutorDone)
                return;

            Instance.CheckProgress(action);
        }

        private void Start(){
            _tutorDone = SaveManager.Load("TutorDone", 0) != 0;
            if (TutorDone == false){
                TutorStep currentStep = _tutorSteps[_currentStepIndex];
                currentStep.OnStartAction?.Invoke();
            }
        }

        private void CheckProgress(TutorAction action){
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

        public static void ForcedAction(TutorAction action){
            foreach (var step in Instance._tutorSteps){
                if (step.TutorAction == action){
                    step.OnStartAction?.Invoke();
                    Instance._tutorDone = true;
                }
            }
        }
    }
}