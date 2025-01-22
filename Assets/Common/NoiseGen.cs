using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

namespace NoiseTest {

public enum NoiseType { Perlin, Grad }

[BurstCompile]
public static class NoiseGen
{
    [BurstCompile]
    public static float Mono(NoiseType type, float coord)
      => type switch 
           { NoiseType.Perlin => PerlinNoise.GetAt(coord),
             NoiseType.Grad => GradNoise.GetAt(coord),
             _ => 0 };

    [BurstCompile]
    public static float Mono(NoiseType type, in float2 coord)
      => type switch 
           { NoiseType.Perlin => PerlinNoise.GetAt(coord),
             NoiseType.Grad => GradNoise.GetAt(coord),
             _ => 0 };

    [BurstCompile]
    public static float Fractal(NoiseType type, float x, int octaves)
    {
        var acc = 0.0f;
        var w = 1.0f;
        var p = x;
        for (var i = 0; i < octaves; i++)
        {
            acc += Mono(type, p) * w;
            p *= 2;
            w *= 0.5f;
        }
        return acc;
    }

    [BurstCompile]
    public static float Fractal(NoiseType type, in float2 pos, int octaves)
    {
        var acc = 0.0f;
        var w = 1.0f;
        var p = pos;
        for (var i = 0; i < octaves; i++)
        {
            acc += Mono(type, p) * w;
            p *= 2;
            w *= 0.5f;
        }
        return acc;
    }
}

} // namespace NoiseTest
