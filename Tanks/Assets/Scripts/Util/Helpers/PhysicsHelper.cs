using System.Linq;
using UnityEngine;

namespace Util.Helpers
{
    public static class PhysicsHelper
    {
        public static bool BoxCastAndDraw(
            Vector3 center,
            Vector3 halfExtents,
            Vector3 direction,
            out RaycastHit hitInfo,
            Quaternion orientation,
            float maxDistance,
            int layerMask) => 
                BoxCastAndDraw(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, Color.magenta);

        public static bool BoxCastAndDraw(
            Vector3 center,
            Vector3 halfExtents,
            Vector3 direction,
            out RaycastHit hitInfo,
            Quaternion orientation,
            float maxDistance,
            int layerMask,
            Color color)
        {
            DebugDrawHelper.DrawBoxCastBox(center, halfExtents, direction, orientation, maxDistance, color);

            return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask);
        }

        public static bool OverlappingBoxCast(
            Vector3 center, 
            Vector3 halfExtents,
            Vector3 direction,
            out RaycastHit hitInfo,
            Quaternion orientation,
            float maxDistance,
            int layerMask)
        {
            var hit = Physics.OverlapBox(center, halfExtents, orientation, layerMask).Any();
            hit |= Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask);

            return hit;
        }

        public static bool OverlappingBoxCastAndDraw(
            Vector3 center, 
            Vector3 halfExtents,
            Vector3 direction,
            out RaycastHit hitInfo,
            Quaternion orientation,
            float maxDistance,
            int layerMask,
            Color color)
        {
            var hit = Physics.OverlapBox(center, halfExtents, orientation, layerMask).Any();
            hit |= BoxCastAndDraw(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, color);

            return hit;
        }

        public static bool OverlappingBoxCastAndDraw(
            Vector3 center,
            Vector3 halfExtents,
            Vector3 direction,
            out RaycastHit hitInfo,
            Quaternion orientation,
            float maxDistance,
            int layerMask) =>
                OverlappingBoxCastAndDraw(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, Color.magenta);
    }
}