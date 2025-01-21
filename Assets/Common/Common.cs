using UnityEngine;
using Unity.Mathematics;

namespace NoiseTest {

public static class mathex
{
    public static float smootherstep01(float t)
      => t * t * t * (t * (t * 6 - 15) + 10);

    public static float2 smootherstep01(float2 t)
      => t * t * t * (t * (t * 6 - 15) + 10);

    public static float bilerp(float v1, float v2, float v3, float v4, float2 p)
      => math.lerp(math.lerp(v1, v2, p.x), math.lerp(v3, v4, p.x), p.y);

    public static float rand01(uint i)
      => math.asfloat(0x3f800000 | (esgtsa_hash(i) >> 9)) - 1.0f;

    // Pseudo-random permutation function from H. Schechter and R. Bridson
    // (Evolving Sub-Grid Turbulence for Smoke Animation)
    public static uint esgtsa_hash(uint s)  
    {  
        s = (s ^ 2747636419) * 2654435769;  
        s = (s ^ s >> 16) * 2654435769;  
        s = (s ^ s >> 16) * 2654435769;  
        return s;  
    }

    public static float fake_sin(float x)
      => x * (1 - math.abs(x)) * 4;

    public static float fake_cos(float x)
      => fake_sin(0.5f - math.abs(x));

    public static float2 fake_sincos(float x)
      => math.float2(fake_sin(x), fake_cos(x));
}

} // namespace NoiseTest
