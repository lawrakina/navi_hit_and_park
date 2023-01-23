using NavySpade.Modules.Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;


namespace pj94.Code.Tutor{
    public class TutorShopClickHandler : ExtendedMonoBehavior<TutorShopClickHandler>, IPointerDownHandler
    {
        [SerializeField] private TutorShopAction _tutorAction;
        [SerializeField] private GameObject[] _linkedObjects;

        public TutorShopAction TutorAction => _tutorAction;

        protected override void Awake(){
            base.Awake();
            SwitchState(false);
        }

        public static void Show()
        {
            foreach (var tutorClick in All)
            {
                if (tutorClick.TutorAction == TutorialShopController.Instance.CurrentTutorAction)
                {
                    tutorClick.SwitchState(true);
                }
            }
        }

        public static void Hide()
        {
            foreach (var tutorClick in All)
            {
                if (tutorClick.TutorAction == TutorialShopController.Instance.CurrentTutorAction)
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
            TutorialShopController.InvokeAction(_tutorAction);
        }
    }
}