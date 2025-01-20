using UnityEngine;
using Unity.Mathematics;

namespace Noise2D {

public static class Gradient360
{
    public static float GetAt(float2 coord)
    {
        coord += 1000;
        var grid = (uint2)coord;
        var uv = coord - grid;
        var grad1 = GradAt(grid + math.uint2(0, 0));
        var grad2 = GradAt(grid + math.uint2(1, 0));
        var grad3 = GradAt(grid + math.uint2(0, 1));
        var grad4 = GradAt(grid + math.uint2(1, 1));
        var g1 = math.dot(grad1, uv);
        var g2 = math.dot(grad2, math.float2(uv.x - 1,     uv.y));
        var g3 = math.dot(grad3, math.float2(    uv.x, uv.y - 1));
        var g4 = math.dot(grad4, math.float2(uv.x - 1, uv.y - 1));
        var fade = uv * uv * uv * (uv * (uv * 6 - 15) + 10);
        return math.lerp(math.lerp(g1, g2, fade.x),
                         math.lerp(g3, g4, fade.x), fade.y);
    }

    static float2 GradAt(uint2 i2)
    {
        var theta = HashAt(i2) * math.PI * 2;
        return math.float2(math.cos(theta), math.sin(theta));
    }

    static float HashAt(uint2 i2)
    {
        var n = WangHash(i2.x + (i2.y << 16));
        return math.asfloat(0x3f800000 | (n >> 9)) - 1.0f;
    }

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
