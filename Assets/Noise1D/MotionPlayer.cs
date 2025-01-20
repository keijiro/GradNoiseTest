using UnityEngine;

namespace Noise1D {

public sealed class MotionPlayer : MonoBehaviour
{
    public enum Method { Perlin1D, Gradient360 }

    [field:SerializeField] public Method NoiseType { get; set; } = Method.Perlin1D;
    [field:SerializeField] public float Frequency { get; set; } = 10;

    float GetNoiseAt(float x)
      => NoiseType switch
         { Method.Perlin1D => Perlin1D.GetAt(x),
           Method.Gradient360 => Gradient360.GetAt(x),
           _ => 0 };

    void Update()
    {
        var t = Time.time * Frequency;
        var y = GetNoiseAt(t) * 0.5f;
        var r = GetNoiseAt(t + 100) * 90;
        transform.localPosition = new Vector3(0, y, 0);
        transform.localRotation = Quaternion.AngleAxis(r, Vector3.forward);
    }
}

} // namespace Noise2D
