using UnityEngine;
using Unity.Mathematics;
using NoiseTest;

namespace Noise1D {

public static class Perlin1D
{
    public static float GetAt(float x)
    {
        var i = (int)x;
        var t = math.frac(x);
        var g0 = GradPerm(i, 0, t);
        var g1 = GradPerm(i, 1, t);
        return math.lerp(g0, g1, mathex.smootherstep01(t));
    }

    static int Perm(int i)
      => ClassicPerlin.perm[i & 0xff];

    static float Grad(int hash, float x)
      => (hash & 1) == 0 ? x : -x;

    static float GradPerm(int i, int d, float t)
      => Grad(Perm(Perm(i + d)), t - d);
}

} // namespace Noise2D
