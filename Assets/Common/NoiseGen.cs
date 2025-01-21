using UnityEngine;
using Unity.Mathematics;

namespace NoiseTest {

public enum NoiseType { Perlin, Grad }

public static class NoiseGen
{
    public static float Mono(NoiseType type, float coord)
      => type switch 
           { NoiseType.Perlin => PerlinNoise.GetAt(coord),
             NoiseType.Grad => GradNoise.GetAt(coord),
             _ => 0 };

    public static float Mono(NoiseType type, float2 coord)
      => type switch 
           { NoiseType.Perlin => PerlinNoise.GetAt(coord),
             NoiseType.Grad => GradNoise.GetAt(coord),
             _ => 0 };

    public static float Fractal(NoiseType type, float x, int octaves)
    {
        var acc = 0.0f;
        var w = 1.0f;
        for (var i = 0; i < octaves; i++)
        {
            acc += Mono(type, x) * w;
            x *= 2;
            w *= 0.5f;
        }
        return acc;
    }

    public static float Fractal(NoiseType type, float2 pos, int octaves)
    {
        var acc = 0.0f;
        var w = 1.0f;
        for (var i = 0; i < octaves; i++)
        {
            acc += Mono(type, pos) * w;
            pos *= 2;
            w *= 0.5f;
        }
        return acc;
    }
}

} // namespace NoiseTest
