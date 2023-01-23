using System;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;


namespace pj94.Code.Roads{
    public class RoadPoint : MonoBehaviour{
        [SerializeField, ReadOnly]
        private RoadPoint _nextTilePoint;
        [field: SerializeField]
        public RoadPoint NextPoint{ get; set; }

        public RoadPoint NextTilePoint{
            get => _nextTilePoint;
            set => _nextTilePoint = value;
        }

        private async void OnEnable(){
            await Task.Delay(Random.Range(100, 500));
            DebugExtension.DebugWireSphere(transform.position, 0.15f, 5);
            var hits = Physics.OverlapSphere(transform.position, 0.15f);
            foreach (var hit in hits){
                if (hit.TryGetComponent(out RoadPoint point)){
                    if (point == this) continue;
                    DebugExtension.DebugCapsule(transform.position, hit.transform.position, Color.HSVToRGB(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)), 0.1f, 5);
                    NextTilePoint = point;
                }
            }
        }
    }
}