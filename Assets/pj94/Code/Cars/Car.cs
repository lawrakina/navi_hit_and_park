using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using PathCreation;
using pj94.Code.Extensions;
using pj94.Code.Roads;
using UniRx;
using UnityEngine;


namespace pj94.Code.Cars{
    public class Car : MonoBehaviour{
        [SerializeField]
        private Bumper[] _bumpers;

        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private ReactiveProperty<float> _impulce = new FloatReactiveProperty();
        [SerializeField]
        private float _checkRadius = 0.1f;
        [field: SerializeField]
        public float TimeBeforeStopping{ get; set; } = 0.4f;

        [SerializeField]
        private CarView _root;


        #region Properties

        public RoadPoint CurrentPoint{
            get => _targetPoint;
            set => _targetPoint = value;
        }
        public ReactiveProperty<float> Impulce{
            get => _impulce;
            private set => _impulce = value;
        }
        public bool IsStopped{ get; set; } = false;
        public bool IsStopped2{ get; set; }
        public ReactiveProperty<CarState> StateProperty{ get; set; } = new ReactiveProperty<CarState>();

        public bool IsOn => _isOn;

        [field: SerializeField]
        private Collider[] _colliders;

        private VertexPath Path{
            get => _path;
            set{
                _path = value;
                _distanceTravelled = 0;
            }
        }
        public float Speed => _speed;

        #endregion


        #region PrivateData

        private RoadPoint _targetPoint;
        private bool _isOn = false;
        private float _distanceTravelled;
        private VertexPath _path;

        #endregion


        public void Init(){
            foreach (var collider in _colliders){
                collider.enabled = true;
            }

            _isOn = true;
        }

        private void Awake(){
            foreach (var bumper in _bumpers){
                bumper.Init(this);
            }
        }

        private void FixedUpdate(){
            if (Impulce.Value < 0.1f) return;
            if (Path == null) return;

            RotationAndMoving();
            CheckDistance();
        }

        private void OnTriggerEnter(Collider other){
            if(!_isOn) return;
            if (other.TryGetComponent(out Car car)){
                car.gameObject.SetActive(false);
                ProgressLevelChecker.Instance.Player.StateProperty.Value = CarState.Trap;
                ResourceLoader.InstantiateObject(PrefabSettings.Instance.Boom, transform.position, Quaternion.identity);
            }
        }

        private void CheckDistance(){
            _distanceTravelled += _speed * Time.fixedDeltaTime * Impulce.Value;
            if (IsStopped2 && _distanceTravelled >= Path.length){
                Stop();
                IsStopped2 = false;
                return;
            }

            if (!(_distanceTravelled > Path.length)) return;
            if (!TryFindNextPosition(transform.forward))
                SlowlyStopped(Path.GetPointAtDistance(Path.length));
        }

        private void OnDrawGizmos(){
            if (Path != null && Path.localPoints.Length > 0)
                Path.localPoints.ToList().ForEach(p => {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(p, 0.1f);
                });
        }

        private void RotationAndMoving(){
            var point = Path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
            var direction = (point - transform.position).normalized;

            var forward = Path.GetDirectionAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
            var velocity = direction * _speed * Time.fixedDeltaTime * Impulce.Value;
            transform.position = velocity + transform.position;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forward),
                GameSettings.Instance.CarRotateSpeed * Time.deltaTime);
        }

        public bool SetDirection(Vector3 vector3){
            if (!TryFindNextPosition(vector3)) return false;
            transform.forward = vector3;
            Impulce.Value = 1;
            return true;
        }

        private bool TryFindNextPosition(Vector3 position){
            if (CurrentPoint != null && CurrentPoint.TryGetComponent(out RoadPoint roadPoint)){
                if (roadPoint.NextTilePoint == null){
                    Stop();
                    return true;
                }

                if (roadPoint.NextTilePoint.NextPoint == null){
                    Stop();
                    return true;
                }

                var oldPosition = CurrentPoint.transform.position;
                CurrentPoint = roadPoint.NextTilePoint.NextPoint;

                var obj1 = CurrentPoint.transform.parent.GetComponent<RoadTile>();

                if (!obj1.Point.Contains(roadPoint.NextTilePoint)){
                    SlowlyStopped(roadPoint.NextTilePoint.NextPoint.transform.position);
                    return true;
                }

                var first = transform.position;
                Vector3 last = CurrentPoint.TryGetComponent(out RoadPoint nextPoint)
                    ? nextPoint.transform.position
                    : obj1.Point[^1].transform.position;

                var middle = (first + last + obj1.transform.position) / 3;

                var wayArray = new List<Vector3>();
                // wayArray.Add(oldPosition);
                wayArray.Add(first);
                wayArray.Add(middle);
                wayArray.Add(CurrentPoint.transform.position);
                // wayArray.Add(last);
                var wayList = SortingListByDistance(wayArray.ToArray());
                Path = PathGenerator.Instance.CreatePath(wayList);

                return true;
            }

            return OverlapTryFindNextPosition(position);
        }

        private Vector3[] SortingListByDistance(Vector3[] parentPoints){
            Vector3 temp;
            for (int i = 0; i < parentPoints.Length; i++){
                for (int j = 0; j < parentPoints.Length; j++){
                    if (Vector3.Distance(transform.position, parentPoints[i]) <
                        Vector3.Distance(transform.position, parentPoints[j])){
                        temp = parentPoints[i];
                        parentPoints[i] = parentPoints[j];
                        parentPoints[j] = temp;
                    }
                }
            }

            return parentPoints;
        }

        private bool OverlapTryFindNextPosition(Vector3 direction){
            var rootPoints = Physics.OverlapSphere(transform.position, 0.1f);
            foreach (var rootPoint in rootPoints){
                if (rootPoint.transform.parent.TryGetComponent(out RoadTile tile)){
                    var checkPosition = transform.position + direction * 0.5f;
                    DebugExtension.DebugWireSphere(checkPosition, Color.blue, _checkRadius * 0.4f, 5f);
                    var hits = Physics.OverlapSphere(checkPosition, _checkRadius * 0.4f);

                    foreach (var hit in hits){
                        if (hit.TryGetComponent(out RoadPoint roadPoint)){
                            foreach (var point1 in tile.Point){
                                if (point1.transform.parent != roadPoint.transform.parent &&
                                    roadPoint.NextPoint != null &&
                                    roadPoint.NextPoint.transform.parent != tile.transform){
                                    var oldPosition = CurrentPoint != null
                                        ? CurrentPoint.transform.position
                                        : transform.position;
                                    CurrentPoint = roadPoint.NextPoint;
                                    var way = new List<Vector3>();
                                    way.Add(oldPosition);
                                    way.Add(roadPoint.transform.position);
                                    way.Add(CurrentPoint.transform.position);
                                    Path = PathGenerator.Instance.CreatePath(SortingListByDistance(way.ToArray()),
                                        false);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void RotateTo(Vector3 direction){
            var value = Vector3.Dot(transform.forward, direction);
            if (0.5f < value){
                Impulce.Value = 1;
                return;
            }

            if (value < -0.5f){
                transform.forward = -transform.forward;
                Impulce.Value = 1;
                return;
            }

            var sign = Math.Sign((transform.InverseTransformVector(direction)).x);

            transform.forward = transform.right * sign;
            Impulce.Value = 1;
        }

        public async void Off(bool correct){
            _isOn = false;
            foreach (var collider in _colliders){
                collider.enabled = false;
            }

            // await Task.Delay(500);
            Stop(correct);
        }


        #region Run/Stops

        public void Run(float carImpulce, float carSpeed){
            StartCoroutine(SlowlyRun(carImpulce, carSpeed));
        }

        private IEnumerator SlowlyRun(float carImpulce, float carSpeed){
            IsStopped = true;
            Impulce.Value = carImpulce;
            _speed = carSpeed;
            TryFindNextPosition(transform.forward);
            yield return new WaitForSeconds(TimeBeforeStopping / 10);
            IsStopped = false;
        }

        public async void Stop(bool correct = true){
            CurrentPoint = null;
            Path = null;
            Impulce.Value = 0;
            if(correct)
                CorrectionPosition();
        }

        public async void SlowlyStopped(Vector3 otherPosition){
            IsStopped = true;
            // Impulce.Value = 0.8f;
            _speed *= 0.8f;
            // DebugExtension.DebugPoint(transform.position, Color.white, 2, 5);
            // DebugExtension.DebugPoint(otherPosition, Color.yellow, 2, 5);
            var way = new List<Vector3>();
            way.Add(transform.position);
            way.Add(transform.position + transform.forward * 0.1f);
            way.Add(otherPosition);
            Path = PathGenerator.Instance.CreatePath(SortingListByDistance(way.ToArray()));
            IsStopped2 = true;
            await Task.Delay((int) (TimeBeforeStopping / 10 * 100));
            IsStopped = false;
            _speed *= 1.2f;
        }

        private void CorrectionPosition(){
            var position = transform.position;

            var newPosition = new Vector3(
                (float) (Math.Round(position.x * 2, MidpointRounding.AwayFromZero) / 2),
                position.y,
                (float) Math.Round(position.z * 2, MidpointRounding.AwayFromZero) / 2);
            // transform.position = newPosition;

            transform.DOMove(newPosition, 0.2f);
        }

        #endregion


        public void SetSkin(GameObject skin){
            if (skin == null) return;
            _root.SetCar(skin);
        }

        public void SetDirection(Vector3 direction, float multiplier){
            if (SetDirection(direction)){
                if (multiplier > GameSettings.Instance.MaxSpeedMovingCar)
                    multiplier = GameSettings.Instance.MaxSpeedMovingCar;
                _speed = multiplier;
            }
        }
    }

    public enum CarState{
        Free, Park, Trap
    }
}