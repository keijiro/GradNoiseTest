using UnityEngine;
using Unity.Mathematics;

namespace Noise1D {

public static class Gradient360
{
    public static float GetAt(float x)
    {
        var i = (uint)(x + 1000);
        var t = math.frac(x);
        var g1 = Grad(i    ) * (t    );
        var g2 = Grad(i + 1) * (t - 1);
        return math.lerp(g1, g2, SmootherStep01(t));
    }

    static float SmootherStep01(float t)
      => t * t * t * (t * (t * 6 - 15) + 10);

    static float Grad(uint i)
      => math.sin(Rand01(i) * math.PI * 2);

    static float Rand01(uint i)
      => math.asfloat(0x3f800000 | (WangHash(i) >> 9)) - 1.0f;

    static uint WangHash(uint n)
    {
        n = (n ^ 61u) ^ (n >> 16);
        n *= 9u;
        n = n ^ (n >> 4);
        n *= 0x27d4eb2du;
        n = n ^ (n >> 15);
        return n;
    }
}

} // namespace Noise2D
