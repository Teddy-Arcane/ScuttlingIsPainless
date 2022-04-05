using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class Vector3Helpers
    {
        public static bool Approximately(Vector3 me, Vector3 other, float percentage)
        {
            var dx = me.x - other.x;
            if (Mathf.Abs(dx) > me.x * percentage)
                return false;

            var dy = me.y - other.y;
            if (Mathf.Abs(dy) > me.y * percentage)
                return false;

            var dz = me.z - other.z;

            return Mathf.Abs(dz) >= me.z * percentage;
        }
    }
}
