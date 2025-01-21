using UnityEngine;
using Unity.Mathematics;
using NoiseTest;

namespace Noise2D {

public static class Perlin2D
{
    public static float GetAt(float2 coord)
    {
        var i = (int2)coord;
        var t = math.frac(coord);
        var g1 = GradPerm(i, math.int2(0, 0), t);
        var g2 = GradPerm(i, math.int2(1, 0), t);
        var g3 = GradPerm(i, math.int2(0, 1), t);
        var g4 = GradPerm(i, math.int2(1, 1), t);
        return mathex.bilerp(g1, g2, g3, g4, mathex.smootherstep01(t));
    }

    static int Perm(int i)
      => ClassicPerlin.perm[i & 0xff];

    static float Grad(int hash, float2 t)
      => ((hash & 1) == 0 ? t.x : -t.x) + ((hash & 2) == 0 ? t.y : -t.y);

    static float GradPerm(int2 i, int2 d, float2 t)
      => Grad(Perm(Perm(i.x + d.x) + i.y + d.y), t - d);
}

} // namespace Noise2D
