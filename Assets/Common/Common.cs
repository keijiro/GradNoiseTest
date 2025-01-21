using UnityEngine;
using Unity.Mathematics;

namespace NoiseTest {

public static class mathex
{
    public static float2 sincos(float t)
      => math.float2(math.sin(t), math.cos(t));

    public static float smootherstep01(float t)
      => t * t * t * (t * (t * 6 - 15) + 10);

    public static float2 smootherstep01(float2 t)
      => t * t * t * (t * (t * 6 - 15) + 10);

    public static float bilerp(float v1, float v2, float v3, float v4, float2 p)
      => math.lerp(math.lerp(v1, v2, p.x), math.lerp(v3, v4, p.x), p.y);

    public static float rand01(uint i)
      => math.asfloat(0x3f800000 | (wanghash(i) >> 9)) - 1.0f;

    public static uint wanghash(uint n)
    {
        n = (n ^ 61u) ^ (n >> 16);
        n *= 9u;
        n = n ^ (n >> 4);
        n *= 0x27d4eb2du;
        n = n ^ (n >> 15);
        return n;
    }
}

} // namespace NoiseTest
