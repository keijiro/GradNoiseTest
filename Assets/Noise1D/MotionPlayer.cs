using UnityEngine;

namespace NoiseTest {

public sealed class MotionPlayer : MonoBehaviour
{
    [field:SerializeField] public NoiseType NoiseType { get; set; }
    [field:SerializeField] public float Frequency { get; set; } = 10;
    [field:SerializeField] public int Octaves { get; set; } = 1;
    [field:SerializeField] public float Amplitude { get; set; } = 1;

    void Update()
    {
        var t = Time.time * Frequency;
        var y = NoiseGen.Fractal(NoiseType, t     , Octaves) * Amplitude * 0.5f;
        var r = NoiseGen.Fractal(NoiseType, t + 10, Octaves) * Amplitude * 90;
        transform.localPosition = new Vector3(0, y, 0);
        transform.localRotation = Quaternion.AngleAxis(r, Vector3.forward);
    }
}

} // namespace NoiseTest
