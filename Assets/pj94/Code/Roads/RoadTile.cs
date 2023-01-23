using UnityEngine;


namespace pj94.Code.Roads{
    public class RoadTile : MonoBehaviour{
        [field: SerializeField]
        public RoadPoint[] Point{ get; set; }
    }
}