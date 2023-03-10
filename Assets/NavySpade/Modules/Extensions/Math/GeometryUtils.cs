using UnityEngine;

namespace NavySpade.Modules.Extensions
{
    public static class GeometryUtils
    {
        /// <summary>
        /// наход точку на прямой по заданной дистанции
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="newDistance"></param>
        /// <returns></returns>
        public static Vector3 FindPointInLineAtDistance(Vector3 a, Vector3 b, float newDistance)
        {
            return a + (b - a) * (newDistance / Vector3.Distance(a, b));
        }

        /// <summary>
        /// наход точку на прямой по заданной дистанции
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="newDistance"></param>
        /// <returns></returns>
        public static Vector2 FindPointInLineAtDistance(Vector2 a, Vector2 b, float newDistance)
        {
            return a + (b - a) * (newDistance / Vector2.Distance(a, b));
        }

        /// <summary>
        /// находит точку на прямой по процентам от дистанции
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="percentValue">100% == 1f</param>
        /// <returns></returns>
        public static Vector3 FindPointInLineAtPercent(Vector3 a, Vector3 b, float percentValue)
        {
            return a + (b - a) * percentValue;
        }

        /// <summary>
        /// находит точку на прямой по процентам от дистанции
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="percentValue">100% == 1f</param>
        /// <returns></returns>
        public static Vector2 FindPointInLineAtPercent(Vector2 a, Vector2 b, float percentValue)
        {
            return a + (b - a) * percentValue;
        }
        
        public static float Sing(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }
        
        public static bool IsPointInTriangle(this Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            var d1 = Sing(pt, v1, v2);
            var d2 = Sing(pt, v2, v3);
            var d3 = Sing(pt, v3, v1);

            var has_neg = d1 < 0 || d2 < 0 || d3 < 0;
            var has_pos = d1 > 0 || d2 > 0 || d3 > 0;

            return !(has_neg && has_pos);
        }
        public static Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found){
            float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

            if (tmp == 0){
                // No solution!
                found = false;
                return Vector2.zero;
            }

            float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

            found = true;

            return new Vector2(
                B1.x + (B2.x - B1.x) * mu,
                B1.y + (B2.y - B1.y) * mu
            );
        }

        public static Vector3 GetIntersectionPointCoordinates(Vector3 a, Vector3 aDirection, Vector3 b, Vector3 bDirection,
            float maxDistance, out bool found){
            Vector2 a1 = new Vector3(a.x, a.z);
            Vector2 a2 = a1 + new Vector2(aDirection.x, aDirection.z) * maxDistance;
            Vector2 b1 = new Vector3(b.x, b.z);
            Vector2 b2 = b1 + new Vector2(bDirection.x, bDirection.z) * maxDistance;

            var result = GetIntersectionPointCoordinates(a1, a2, b1, b2, out found);
            return new Vector3(result.x,0,result.y);
        }
    }
}
