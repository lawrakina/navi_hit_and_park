using NavySpade.Modules.Extensions;
using UnityEngine;


namespace pj94.Code.Cars{
    internal class Curv{
        private Vector3 _endPos;
        private Vector3 _middlePos;
        private Vector3 _startPos;
        private Vector3 _startRotation;
        private Vector3 _endRotation;

        public Curv(Transform startPoint, Transform endPoint, bool input, bool centerPoint){
            
            _startPos = startPoint.position;

            var newRotStart = startPoint.rotation.eulerAngles + Vector3.up * 180;
            newRotStart = new Vector3(newRotStart.x, newRotStart.y % 360, newRotStart.z);
            _startRotation = input ? newRotStart : startPoint.rotation.eulerAngles;
            
            var newRotEnd = endPoint.rotation.eulerAngles + Vector3.up * 180;
            newRotEnd = new Vector3(newRotEnd.x, newRotEnd.y % 360, newRotEnd.z);
            _endRotation = input ? endPoint.rotation.eulerAngles : newRotEnd;

            _endPos = endPoint.position;
            
            _middlePos = CuclMiddlePos();
            
            Vector3 CuclMiddlePos(){
                var aDirectionForCenterPoint = input ? -startPoint.forward : startPoint.forward;
                var bDirectionForCenterPoint = input ? endPoint.forward : -endPoint.forward;
                if (centerPoint){
                    return (_startPos + _endPos) / 2;
                } else{
                    return GeometryUtils.GetIntersectionPointCoordinates(
                        _startPos,
                        aDirectionForCenterPoint,
                        _endPos,
                        bDirectionForCenterPoint,
                        1f,
                        out bool _);
                }
            }
        }

        public Curv(Transform startPoint, Transform endPoint){
            _startPos = startPoint.position;
            _endPos = endPoint.position;
            _startRotation = startPoint.rotation.eulerAngles;
            _endRotation = endPoint.rotation.eulerAngles;
            _middlePos = GeometryUtils.GetIntersectionPointCoordinates(
                _startPos,
                startPoint.forward,
                _endPos,
                -endPoint.forward,
                1f,
                out bool _);
        }

        public Vector3 GetPosition(float t){
            return QuadraticLerp(_startPos, _middlePos, _endPos, t);
        }

        public Vector3 GetRotation(float t){
            // var aDirection = _input ? Quaternion.AngleAxis(180, Vector3.up) * _startRotation : _startRotation;
            // var bDirection = _input ? _endRotation : Quaternion.AngleAxis(180, Vector3.up) * _endRotation;

            
            
            // var result = Vector3.Lerp(_startRotation,_endRotation, t);
            var result = Vector3.Lerp(_startRotation, _endRotation, t);
            return result;
        }

        private Vector3  QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t){
            Vector3 ab = Vector3.Lerp(a, b, t);
            Vector3 bc = Vector3.Lerp(b, c, t);

            DebugExtension.DebugPoint(_startPos, Color.yellow, 2, 1);
            DebugExtension.DebugPoint(_middlePos, Color.cyan,2, 1);
            DebugExtension.DebugPoint(_endPos, Color.green,2, 1);
            
            var result = Vector3.Lerp(ab, bc, t);
            DebugExtension.DebugPoint(result, Color.red,0.4f, 1);
            return result;
        }
    }
}