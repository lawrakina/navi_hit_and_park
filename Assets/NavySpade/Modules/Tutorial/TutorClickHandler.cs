using UnityEngine;
using UnityEngine.EventSystems;


namespace NavySpade.Modules.Tutorial{
    public class TutorClickHandler : ExtendedMonoBehavior<TutorClickHandler>, IPointerDownHandler
    {
        [SerializeField] private TutorAction _tutorAction;
        [SerializeField] private GameObject[] _linkedObjects;

        public TutorAction TutorAction => _tutorAction;

        protected override void Awake(){
            base.Awake();
            SwitchState(false);
        }

        public static void Show()
        {
            foreach (var tutorClick in All)
            {
                if (tutorClick.TutorAction == TutorialController.Instance.CurrentTutorAction)
                {
                    tutorClick.SwitchState(true);
                }
            }
        }

        public static void Hide()
        {
            foreach (var tutorClick in All)
            {
                if (tutorClick.TutorAction == TutorialController.Instance.CurrentTutorAction)
                {
                    tutorClick.SwitchState(false);
                }
            }
        }

        public void SwitchState(bool show)
        {
            foreach (var linkedObject in _linkedObjects)
            {
                linkedObject.SetActive(show);
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            TutorialController.InvokeAction(_tutorAction);
        }
    }
}