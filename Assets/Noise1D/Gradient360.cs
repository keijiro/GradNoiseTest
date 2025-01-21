using UnityEngine;
using Unity.Mathematics;
using NoiseTest;

namespace Noise1D {

public static class Gradient360
{
    public static float GetAt(float x)
    {
        var i = (int)x;
        var t = math.frac(x);
        var g1 = Grad(i    ) * (t    );
        var g2 = Grad(i + 1) * (t - 1);
        return math.lerp(g1, g2, mathex.smootherstep01(t));
    }

    static float Grad(int i)
      => math.sin(mathex.rand01((uint)i) * math.PI * 2);
}

} // namespace Noise2D
