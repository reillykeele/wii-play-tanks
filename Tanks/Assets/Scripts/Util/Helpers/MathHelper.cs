using System;
using UnityEngine;

namespace Util.Helpers
{
    public static class MathHelper
    {
        public static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

        public static float Distance2(Vector3 a, Vector3 b) =>
            Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2);

        public static float PercentOfRange(float percent, float min, float max) => percent * max + (1 - percent) * min;

        public static Vector3 Querp(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            var p0 = Vector3.Lerp(a, b, t);
            var p1 = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(p0, p1, t);
        }

        /// <summary>
        /// Rounds a normalized direction vector to an 8-directional vector.
        /// 
        /// Inspired by https://lemire.me/blog/2022/07/24/round-a-direction-vector-to-the-nearest-8-way-compass/
        /// </summary>
        /// <param name="dir">Normalized direction vector to round.</param>
        /// <returns></returns>
        public static Vector3 RoundTo8(Vector3 dir)
        {
            var outdir = RoundTo8(new Vector2(dir.x, dir.z));
            return new Vector3(outdir.x, 0f, outdir.y);
        }

        /// <summary>
        /// Rounds a normalized direction vector to an 8-directional vector.
        /// 
        /// Inspired by https://lemire.me/blog/2022/07/24/round-a-direction-vector-to-the-nearest-8-way-compass/
        /// </summary>
        /// <param name="dir">Normalized direction vector to round.</param>
        /// <returns></returns>
        public static Vector2 RoundTo8(Vector2 dir)
        {
            var absx = Mathf.Abs(dir.x);
            var absy = Mathf.Abs(dir.y);

            // if >= sin(3*pi/8), then 1, else sqrt(2)/2
            var x = (absx >= 0.923879f) ? 1 : 0.707106f;
            var y = (absy >= 0.923879f) ? 1 : 0.707106f;
            
            x = (absy >= 0.923879f) ? 0 : x;
            y = (absx >= 0.923879f) ? 0 : y;
            
            x *= dir.x < 0 ? -1 : 1;
            y *= dir.y < 0 ? -1 : 1;

            return new Vector2(x, y);
        }
    }
}