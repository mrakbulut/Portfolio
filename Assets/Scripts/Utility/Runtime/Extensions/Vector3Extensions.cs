using UnityEngine;

namespace Portfolio.Utility
{
    public static class Vector3Extensions
    {
        public static float MagnitudeDistance(this Vector3 a, Vector3 b)
        {
            return (a - b).magnitude;
        }

        public static float MagnitudeDistanceSquared(this Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }
    }
}
