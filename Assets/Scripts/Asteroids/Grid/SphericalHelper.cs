using UnityEngine;

namespace Asteroids.Grid
{
    public static class SphericalHelper
    {
        public static Vector3 GetForward(Vector3 from, Vector3 to)
        {
            return to - from * Vector3.Dot(to, from);
        }

        public static Quaternion SphericalLookAt(Vector3 from, Vector3 to)
        {
            return Quaternion.LookRotation(GetForward(from, to), from);
        }
    }
}