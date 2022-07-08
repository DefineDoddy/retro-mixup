using UnityEngine;

namespace Project.Scripts.Utils
{
    public static class MathUtils
    {
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return Mathf.Lerp(from2, to2, Mathf.InverseLerp(from1, to1, value));
        }
    }
}