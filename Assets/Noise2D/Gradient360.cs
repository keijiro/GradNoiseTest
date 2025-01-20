using UnityEngine;
using Unity.Mathematics;

namespace Noise2D {

public static class Gradient360
{
    public static float GetAt(float2 coord)
    {
        var i = (uint2)(coord + 1000);
        var uv = math.frac(coord).xyxy - math.float4(0, 0, 1, 1);
        var g1 = math.dot(Grad(i + math.uint2(0, 0)), uv.xy);
        var g2 = math.dot(Grad(i + math.uint2(1, 0)), uv.zy);
        var g3 = math.dot(Grad(i + math.uint2(0, 1)), uv.xw);
        var g4 = math.dot(Grad(i + math.uint2(1, 1)), uv.zw);
        return BiLerp(g1, g2, g3, g4, SmootherStep01(uv.xy));
    }

    static float BiLerp(float v1, float v2, float v3, float v4, float2 p)
      => math.lerp(math.lerp(v1, v2, p.x), math.lerp(v3, v4, p.x), p.y);

    static float2 SmootherStep01(float2 t)
      => t * t * t * (t * (t * 6 - 15) + 10);

    static float2 SinCos(float t)
      => math.float2(math.sin(t), math.cos(t));

    static float2 Grad(uint2 i2)
      => SinCos(Rand01(i2) * math.PI * 2);

    static float Rand01(uint2 i2)
      => math.asfloat(0x3f800000 | (WangHash(i2.x + (i2.y << 16)) >> 9)) - 1;

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
