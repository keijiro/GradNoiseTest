using UnityEngine;
using Unity.Mathematics;

namespace Noise1D {

public static class NoiseGen
{
    public enum Method { Perlin, Grad360 }

    public static float Single(Method method, float x)
      => method switch 
           { Method.Perlin => Perlin1D.GetAt(x),
             Method.Grad360 => Gradient360.GetAt(x),
             _ => 0 };

    public static float Fractal(Method method, float x, int octaves)
    {
        var acc = 0.0f;
        var w = 1.0f;
        for (var i = 0; i < octaves; i++)
        {
            acc += Single(method, x) * w;
            x *= 2;
            w *= 0.5f;
        }
        return acc;
    }
}

} // namespace Noise1D
