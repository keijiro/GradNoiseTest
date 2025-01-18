using UnityEngine;

public sealed class Motion : MonoBehaviour
{
    public enum NoiseType { Perlin, Sine }

    [SerializeField] float _frequency = 1;
    [SerializeField] Noise.Method _method = Noise.Method.Perlin;

    void Update()
    {
        var t = Time.time * _frequency;
        var y = Noise.Func(_method, t) * 0.5f;
        var r = Noise.Func(_method, t + 100) * 90;
        transform.localPosition = new Vector3(0, y, 0);
        transform.localRotation = Quaternion.AngleAxis(r, Vector3.forward);
    }
}
