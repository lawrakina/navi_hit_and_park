using NaughtyAttributes;
using pj94.Code.Extensions;
using UniRx;
using UnityEngine;


namespace pj94.Code.Cars{
    public class PlayerInput : MonoBehaviour{
        [Header("Тип управления")]
        [SerializeField]
        private float _multiplierSpeed = 0.01f;

        [SerializeField]
        private Car _player;

        [SerializeField, ReadOnly]
        private Car[] _cars;
        // [Header("Options")]
        // [SerializeField]
        private float _minSwipeDistance_X = 190;
        // [SerializeField]
        private float _minSwipeDistance_Y = 220;
        protected CompositeDisposable _subscriptions = new CompositeDisposable();

        private Vector3 _initClickPosition = default;
        private bool _isOn = false;
        private ArrowView _arrow;

        public void Init(Car user){
            _player = user;
            _isOn = true;
            _cars = FindObjectsOfType<Car>();
            foreach (var car in _cars){
                car.Impulce.Subscribe(ChangeImpulce).AddTo(_subscriptions);
            }

            InstantiateArrow();
        }

        private void InstantiateArrow(){
            _arrow = ResourceLoader.InstantiateObject(PrefabSettings.Instance.Arrow, transform, true);
            _arrow.gameObject.SetActive(false);
        }

        private void ChangeImpulce(float _){
            _isOn = false;
            foreach (var car in _cars){
                if (car.Impulce.Value > 0.1f) break;
            }

            _isOn = true;
        }

        private void Start(){
            _minSwipeDistance_X = SysUtils.GetFloatValueProportionatelyTargetScreenWidth(_minSwipeDistance_X);
            _minSwipeDistance_Y = SysUtils.GetFloatValueProportionatelyTargetScreenHeight(_minSwipeDistance_Y);
        }

        private void Update(){
            if (!_isOn) return;

            var mousePosition = SysUtils.GetCorrectMousePosition();

            if (Input.GetMouseButton(0)){
                if (_initClickPosition == default){
                    _initClickPosition = mousePosition;
                }
            }

            if (GameSettings.Instance.InverseControll){
                ArrowLogic(mousePosition);
            }

            if (Input.GetMouseButtonUp(0)){
                var xSignedDistance = mousePosition.x - _initClickPosition.x;
                var ySignedDistance = mousePosition.y - _initClickPosition.y;

                var targetDirection = Vector3.zero;

                if (xSignedDistance < -_minSwipeDistance_X){
                    targetDirection.x = -1;
                } else if (xSignedDistance > _minSwipeDistance_X){
                    targetDirection.x = 1;
                } else if (ySignedDistance < -_minSwipeDistance_Y){
                    targetDirection.z = -1;
                } else if (ySignedDistance > _minSwipeDistance_Y){
                    targetDirection.z = 1;
                }

                if (targetDirection != Vector3.zero){
                    if (GameSettings.Instance.InverseControll)
                        _player.SetDirection(-targetDirection,
                            new Vector2(xSignedDistance, ySignedDistance).magnitude * _multiplierSpeed);
                    else
                        _player.SetDirection(targetDirection);
                }

                _initClickPosition = default;
                _arrow.gameObject.SetActive(false);
            }
        }

        private void ArrowLogic(Vector2 mousePosition){
            if (Input.GetMouseButton(0)){
                var mousePositionX = mousePosition.x - _initClickPosition.x;
                var mousePositionY = mousePosition.y - _initClickPosition.y;
                var direction = Vector3.zero;

                if (mousePositionX < -_minSwipeDistance_X){
                    direction.x = -1;
                } else if (mousePositionX > _minSwipeDistance_X){
                    direction.x = 1;
                } else if (mousePositionY < -_minSwipeDistance_Y){
                    direction.z = -1;
                } else if (mousePositionY > _minSwipeDistance_Y){
                    direction.z = 1;
                }

                if (direction != Vector3.zero){
                    _arrow.gameObject.SetActive(true);
                    _arrow.transform.position = _player.transform.position;
                    _arrow.transform.forward = direction;
                    var value = new Vector2(mousePositionX, mousePositionY).magnitude * _multiplierSpeed * 0.1f;
                    _arrow.Stretch(value);
                    _arrow.Coloring(value);
                }
            } else{
                if (_arrow.gameObject.activeSelf){
                    _arrow.gameObject.SetActive(false);
                }
            }
        }

        private void OnDisable(){
            _subscriptions.Dispose();
        }
    }
}