using UnityEngine;
using Unity.Mathematics;

namespace Noise2D {

public static class NoiseGen
{
    public enum Method { Perlin, Grad360 }

    public static float Single(Method method, float2 pos)
      => method switch 
           { Method.Perlin => ClassicPerlin.GetAt(pos),
             Method.Grad360 => Gradient360.GetAt(pos),
             _ => 0 };

    public static float Fractal(Method method, float2 pos, int octaves)
    {
        var acc = 0.0f;
        var w = 1.0f;
        for (var i = 0; i < octaves; i++)
        {
            acc += Single(method, pos) * w;
            pos *= 2;
            w *= 0.5f;
        }
        return acc;
    }
}

} // namespace Noise2D
